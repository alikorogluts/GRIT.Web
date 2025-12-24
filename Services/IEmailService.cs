using GRIT.Web.Models.DTO;

namespace GRIT.Web.Services
{
    public interface IEmailService
    {
        Task<MessageDto> SendEmailAsync(ContactMessage contact);
    }
}