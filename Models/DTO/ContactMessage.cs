using System.ComponentModel.DataAnnotations;

namespace GRIT.Web.Models.DTO
{
    public class ContactMessage
    {   [Required]
        public string NameSurname { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}