using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization; // IStringLocalizer için
using Microsoft.Extensions.Localization;

namespace GRIT.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IStringLocalizer<ErrorController> _localizer;

        public ErrorController(IStringLocalizer<ErrorController> localizer)
        {
            _localizer = localizer;
        }

        // 🔥 DEĞİŞİKLİK BURADA: Rotanın başına {culture} ekledik.
        // Artık bu sayfa normal bir içerik sayfası gibi çalışır.
        [Route("{culture}/Error/Page/{statusCode}")]
        public IActionResult Page(int statusCode)
        {
            // ViewBag.CurrentCulture'a gerek kalmadı, View zaten RouteData'dan okuyacak.
            ViewBag.StatusCode = statusCode;

            // Mesajları yine basitçe burada yönetebiliriz veya .resx kullanabilirsin
            // Örnek olarak basit if-else ile:
            var culture = RouteData.Values["culture"]?.ToString() ?? "tr";

            string message = "";
            string btnText = "";

            if (culture == "en")
            {
                message = statusCode switch
                {
                    404 => "Page Not Found",
                    500 => "Server Error",
                    403 => "Access Denied",
                    _ => "An Error Occurred"
                };
                btnText = "Back to Home";
            }
            else
            {
                message = statusCode switch
                {
                    404 => "Sayfa Bulunamadı",
                    500 => "Sunucu Hatası",
                    403 => "Erişim Reddedildi",
                    _ => "Bir Hata Oluştu"
                };
                btnText = "Ana Sayfaya Dön";
            }

            ViewBag.ErrorMessage = message;
            ViewBag.ButtonText = btnText;

            return View("NotFound");
        }

        // 500 Hataları için
        [Route("Error/General")]
        public IActionResult General()
        {
            // 500 hatasında varsayılan olarak tr'ye veya o anki dile yönlendirebiliriz
            // Şimdilik varsayılan tr olsun:
            return RedirectToAction("Page", new { culture = "tr", statusCode = 500 });
        }
    }
}