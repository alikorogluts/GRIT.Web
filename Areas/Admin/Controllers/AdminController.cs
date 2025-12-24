using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GRIT.Web.Models;
using GRIT.Web.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;
using GRIT.Web.Data;
using GRIT.Web.Services;

namespace GRIT.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _context;

        public AdminController(
            IAdminService adminService,
            AppDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IHttpClientFactory httpClientFactory,
            ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // ==========================================
        // GİRİŞ İŞLEMLERİ (LOGIN)
        // ==========================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // Zaten giriş yapmışsa direkt Index'e yönlendir
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Login(LoginVM model, string? returnUrl = null) // ✅ LoginVM olarak aldığımıza emin olun
        {
            // ReturnUrl'i tekrar View'e taşı ki hata olursa link kaybolmasın
            ViewData["ReturnUrl"] = returnUrl;

            // Model validasyonu (Boş veri kontrolü)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 1) Kullanıcıyı bul
                var user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user == null)
                {
                    // Model state hatası ekle
                    ModelState.AddModelError(string.Empty, "E-posta adresi veya şifre hatalı!");
                    return View(model);
                }

                // 2) Şifre Doğrulama
                bool isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, "E-posta adresi veya şifre hatalı!");
                    return View(model);
                }

                // 3) Cookie ve Claims İşlemleri
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email ?? ""),
                    new Claim("AdminId", user.Id.ToString()),
                    new Claim("AdminName", user.FullName ?? user.Email ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                // Yönlendirme
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                ModelState.AddModelError(string.Empty, "Sunucu hatası oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.Identity?.Name;

            await HttpContext.SignOutAsync("CookieAuth");
            HttpContext.Session.Clear();

            _logger.LogInformation($"Admin logout: {userEmail}");

            return RedirectToAction("Login");
        }

        // ==========================================
        // HABER YÖNETİMİ (CRUD)
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var newsItems = await _context.NewsItems
                    .OrderByDescending(x => x.Date)
                    .ToListAsync();

                return View(newsItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading news items");
                TempData["Error"] = "İçerikler yüklenirken hata oluştu.";
                return View(new List<NewsItem>());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CreateNewsItemDto dto, IFormFile? ImageFile, string? ImageUrl)
        {
            try
            {
                string? oldImagePath = null;

                // GÜNCELLEME İŞLEMİ VE YENİ RESİM YÜKLENİYORSA ESKİ RESMİ BUL
                if (dto.Id > 0 && (ImageFile != null || !string.IsNullOrWhiteSpace(ImageUrl)))
                {
                    var existingItem = await _context.NewsItems
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == dto.Id);

                    if (existingItem != null && !string.IsNullOrEmpty(existingItem.Image))
                    {
                        // Sadece local dosyaları sil (URL'leri silme)
                        if (!existingItem.Image.StartsWith("http"))
                        {
                            oldImagePath = existingItem.Image;
                        }
                    }
                }

                // 1️⃣ DOSYA YÜKLENDİYSE
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Boyut kontrolü
                    if (ImageFile.Length > 100 * 1024 * 1024)
                    {
                        TempData["Error"] = "Görsel 100MB'dan büyük olamaz!";
                        return RedirectToAction("Index");
                    }

                    // Uzantı kontrolü
                    var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    
                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["Error"] = "Geçersiz dosya formatı! Sadece jpg, jpeg, png, webp kabul edilir.";
                        return RedirectToAction("Index");
                    }

                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "news");
                    if (!Directory.Exists(uploadsFolder)) 
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        DeleteOldImage(oldImagePath);
                    }

                    // Yeni yolu ata
                    dto.Image = $"/uploads/news/{fileName}";
                }
                // 2️⃣ URL GİRİLDİYSE
                else if (!string.IsNullOrWhiteSpace(ImageUrl))
                {
                    // URL'yi normalize et
                    var normalizedUrl = ImageUrl.Trim();
                    
                    // YouTube URL'sini thumbnail'e çevir
                    var youtubeMatch = Regex.Match(normalizedUrl, @"(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&\s]+)");
            if (youtubeMatch.Success)
            {
                normalizedUrl = $"https://img.youtube.com/vi/{youtubeMatch.Groups[1].Value}/maxresdefault.jpg";
            }
            
            // Eski local dosyayı sil (eğer varsa)
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                DeleteOldImage(oldImagePath);
            }
            
            dto.Image = normalizedUrl;
        }
        // 3️⃣ HİÇBİRİ YOKSA
        else if (string.IsNullOrEmpty(dto.Image))
        {
            // Yeni kayıtsa placeholder ata
            if (dto.Id == 0)
            {
                dto.Image = "/image/placeholder.png";
            }
            // Güncellemede mevcut resmi koru (DTO'dan gelecek)
        }

        // Tarih kontrolü
        if (dto.Date == default || dto.Date < new DateTime(1753, 1, 1))
        {
            dto.Date = DateTime.Now;
        }

        MessageDto result;
        if (dto.Id == 0)
        {
            result = await _adminService.CreateAsync(dto);
        }
        else
        {
            result = await _adminService.UpdateAsync(dto.Id, dto);
        }

        TempData[result.IsSucces ? "Success" : "Error"] = result.MessageText;
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Save error");
        TempData["Error"] = $"Kayıt hatası: {ex.Message}";
        return RedirectToAction("Index");
    }
}

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Resmi silmek için kayıt gerekiyor
                var item = await _context.NewsItems.FindAsync(id);
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        DeleteOldImage(item.Image);
                    }

                    var result = await _adminService.DeleteAsync(id);

                    if (result.IsSucces)
                    {
                        TempData["Success"] = result.MessageText;
                        _logger.LogInformation($"News item deleted: {id}");
                    }
                    else
                    {
                        TempData["Error"] = result.MessageText;
                    }
                }
                else
                {
                    TempData["Error"] = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete error");
                TempData["Error"] = "Silme işleminde hata oluştu.";
            }

            return RedirectToAction("Index");
        }



        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                // 1. URL başındaki "/" işaretini kaldır (örn: "/uploads/..." -> "uploads/...")
                var relativePath = imageUrl.TrimStart('/');

                // 2. WebRootPath (wwwroot) ile birleştir
                var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                // 3. Slash yönlerini işletim sistemine uygun hale getir (Windows için \ Linux için /)
                absolutePath = absolutePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    .Replace("\\", Path.DirectorySeparatorChar.ToString());

                // 4. Dosya var mı kontrol et ve sil
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                    _logger.LogInformation($"Eski görsel silindi: {absolutePath}");
                }
            }
            catch (Exception ex)
            {
                // Hata olsa bile akışı bozma, sadece logla
                _logger.LogWarning(ex, $"Eski görsel silinemedi: {imageUrl}");
            }
        }
    }
}