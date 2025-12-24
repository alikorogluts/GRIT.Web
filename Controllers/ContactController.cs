using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GRIT.Web.Models;
using GRIT.Web.Models.DTO;
using GRIT.Web.Services;
using Microsoft.AspNetCore.Identity;

namespace GRIT.Web.Controllers;

public class ContactController : Controller
{
    private readonly ILogger<ContactController> _logger;
    private readonly IEmailService  _emailService;

    public ContactController(ILogger<ContactController> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage([FromBody] ContactMessage model)
    {
        // 1. Validasyon Hatası Varsa
        if (!ModelState.IsValid)
        {
            return BadRequest(new MessageDto
            {
                IsSucces = false, // Not: Yazım hatası var, genelde 'IsSuccess' olur ama senin modeline uydum.
                MessageText = "Lütfen tüm alanları eksiksiz doldurun.",
                StatusCode = 400
            });
        }

        // 2. Servise Gönder (Await kullanmalısın)
        var serviceResponse = await _emailService.SendEmailAsync(model);

        // 3. Servisten gelen cevabı kontrol et
        if (!serviceResponse.IsSucces)
        {
            // Servis içi bir hata (SMTP vs)
            return StatusCode(500, serviceResponse);
        }

        // 4. Başarılı
        return Ok(serviceResponse);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}