namespace  GRIT.Web.Models.DTO
{
    
    public class MessageDto
    {
        public bool IsSucces { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public int? StatusCode { get; set; } // Hata tipini taşır (404, 500 vb.)
        public int? CreatedId { get; set; }  // POST işleminde oluşturulan ID'yi taşır
    }
};