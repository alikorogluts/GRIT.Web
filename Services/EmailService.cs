using MailKit.Net.Smtp;
using MimeKit;
using GRIT.Web.Models.DTO;
using System.Globalization; // Kültürü kontrol etmek için gerekli

namespace GRIT.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<MessageDto> SendEmailAsync(ContactMessage contact)
        {
            // Kullanıcı şu an İngilizce mi geziyor?
            bool isEnglish = CultureInfo.CurrentCulture.Name.StartsWith("en");

            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_config["EmailSettings:UserName"]));
                email.To.Add(MailboxAddress.Parse(_config["EmailSettings:SendToUser"])); 

                // --- 1. SİZE GELEN MAİL (HEP TÜRKÇE) ---
                email.Subject = $"İletişim Formu: {contact.Subject}";
                email.ReplyTo.Add(MailboxAddress.Parse(contact.Email));

                string htmlBody = $@"
                    <html>
                    <body>
                        <h2>Yeni İletişim Mesajı</h2>
                        <p><strong>Ad Soyad:</strong> {contact.NameSurname}</p>
                        <p><strong>Email:</strong> {contact.Email}</p>
                        <p><strong>Telefon:</strong> {contact.Phone}</p>
                        <p><strong>Konu:</strong> {contact.Subject}</p>
                        <hr/>
                        <p><strong>Mesaj:</strong></p>
                        <p>{contact.Message}</p>
                        <br/>
                        <small>Bu mesaj {DateTime.Now} tarihinde web sitesinden gönderilmiştir.</small>
                    </body>
                    </html>";

                email.Body = new TextPart("html") { Text = htmlBody };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _config["EmailSettings:Host"],
                    int.Parse(_config["EmailSettings:Port"]),
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSettings:UserName"],
                    _config["EmailSettings:Password"]
                );

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                // --- 2. KULLANICIYA DÖNEN MESAJ (DİNAMİK) ---
                // Resx dosyası kullanmadan, if/else ile dili belirliyoruz.
                
                string successMessage;
                if (isEnglish)
                {
                    successMessage = "Your message has been sent successfully. We will get back to you as soon as possible.";
                }
                else
                {
                    successMessage = "Mesajınız başarıyla bize ulaştı. En kısa sürede dönüş yapacağız.";
                }

                return new MessageDto
                {
                    IsSucces = true,
                    MessageText = successMessage,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                // Hata mesajını da dile göre ayarlayalım
                string errorMessage;
                if (isEnglish)
                {
                    errorMessage = "An error occurred while sending the email. Please try again later.";
                }
                else
                {
                    errorMessage = "Mail gönderilirken bir hata oluştu: " + ex.Message;
                }

                return new MessageDto
                {
                    IsSucces = false,
                    MessageText = errorMessage,
                    StatusCode = 500
                };
            }
        }
    }
}