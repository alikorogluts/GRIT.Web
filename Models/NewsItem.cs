using System.ComponentModel.DataAnnotations;

namespace GRIT.Web.Models
{
    public class NewsItem
    {
        [Key]
        public int Id { get; set; }

        // --- ORTAK ALANLAR ---
        [Required]
        public DateTime Date { get; set; }

        // Opsiyonel medya alanları
        public string? Image { get; set; } 
        public string? ExternalUrl { get; set; }
        public string? VideoEmbed { get; set; }

        // --- TÜRKÇE ALANLAR ---
        [Required]
        public string TitleTr { get; set; }
        public string? AuthorTr { get; set; }  
        public string? CategoryTr { get; set; }
        public string? SummaryTr { get; set; }
        public string? DescriptionTr { get; set; } 

        // --- İNGİLİZCE ALANLAR ---
        public string? TitleEn { get; set; }
        public string? AuthorEn { get; set; }
        public string? CategoryEn { get; set; }
        public string? SummaryEn { get; set; }
        public string? DescriptionEn { get; set; }
    }
}