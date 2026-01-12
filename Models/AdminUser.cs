using System.ComponentModel.DataAnnotations;
namespace GRIT.Web.Models;

public class AdminUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } // Gerçek projede burası şifreli (Hash) tutulur

    [MaxLength(150)]
    public string FullName { get; set; } // Örn: "Halil Akıncı"
}