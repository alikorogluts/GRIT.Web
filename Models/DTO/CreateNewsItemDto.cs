using System.ComponentModel.DataAnnotations;

namespace GRIT.Web.Models.DTO
{

    public class CreateNewsItemDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required] public string Image { get; set; }

        public string ExternalUrl { get; set; }
        public string VideoEmbed { get; set; }

        // --- TR ---
        [Required] public string TitleTr { get; set; }

        [Required] public string AuthorTr { get; set; }

        [Required] public string CategoryTr { get; set; }

        [Required] public string SummaryTr { get; set; }

        [Required] public string DescriptionTr { get; set; }


        // --- EN ---
        [Required] public string TitleEn { get; set; }

        [Required] public string AuthorEn { get; set; }

        [Required] public string CategoryEn { get; set; }

        [Required] public string SummaryEn { get; set; }

        [Required] public string DescriptionEn { get; set; }

    }
}