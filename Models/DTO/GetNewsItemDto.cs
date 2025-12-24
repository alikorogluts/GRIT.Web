namespace GRIT.Web.Models.DTO
{
    public class GetNewsItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ExternalUrl { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string VideoEmbed { get; set; }
        public string Description { get; set; } 
        
    }
}