using GRIT.Web.Repositories;
using GRIT.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing; // ✅ BU NAMESPACE ŞART
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GRIT.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// --- SERVİSLER ---
builder.Services.AddHttpClient();

// Localization Servisleri
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Veritabanı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Kendi Servislerin
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "GRIT.Admin.Auth";
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// --- DİL AYARLARI (DÜZELTİLEN KISIM) ---
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // 🚀 İŞTE ÇÖZÜM BURADA:
    // Bu satır, sistemin URL'deki {culture} parametresini (örn: /en/Home)
    // okumasını ve onu en öncelikli dil kuralı yapmasını sağlar.
    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
});
var app = builder.Build();

// --- 1. AKILLI HATA YÖNETİMİ (CUSTOM MIDDLEWARE) ---
// Bu blok, standart UseStatusCodePagesWithReExecute yerine geçer.
// Hata olduğunda dili tespit eder ve doğru dildeki hata sayfasına yönlendirir.
app.Use(async (context, next) =>
{
    await next(); // Önce sayfayı çalıştırmayı dene...

    // Eğer sayfa yoksa (404) ve response daha yazılmadıysa:
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        // Orijinal URL'i al
        string originalPath = context.Request.Path.Value ?? "";
        
        // Varsayılan dil
        string culture = "tr";

        // URL "/en" ile başlıyorsa dili İngilizce yap
        if (originalPath.StartsWith("/en", StringComparison.OrdinalIgnoreCase))
        {
            culture = "en";
        }

        // URL'i "/en/Error/Page/404" formatına çevir
        context.Request.Path = $"/{culture}/Error/Page/404";
        
        // Pipeline'ı bu yeni adresle tekrar çalıştır
        await next();
    }
});

// 500 Hataları için
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/General"); // Burası da güncellenebilir ama şimdilik kalsın
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UseRouting();
app.UseSession();
app.UseRequestLocalization(); // Dil servisi
app.UseAuthentication();
app.UseAuthorization();

// --- ROTA TANIMLARI ---
// (Burada değişiklik yok, aynı kalabilir)
app.MapControllerRoute(
    name: "admin_area",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { area = "Admin", controller = "Admin" }
);

app.MapControllerRoute(
    name: "localized",
    pattern: "{culture}/{controller=Home}/{action=Index}/{id?}",
    defaults: new { culture = "tr" },
    constraints: new { culture = "tr|en" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { culture = "tr" }
);

app.Run();