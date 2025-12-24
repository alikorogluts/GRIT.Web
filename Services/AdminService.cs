using GRIT.Web.Models;
using GRIT.Web.Models.DTO;
using GRIT.Web.Repositories;

namespace GRIT.Web.Services
{
    public class AdminService : IAdminService
    {
        private readonly INewsRepository _repo;

        public AdminService(INewsRepository repo)
        {
            _repo = repo;
        }

        // -----------------------------------------------------------
        // LIST
        // -----------------------------------------------------------
        public async Task<List<GetNewsItemDto>> GetAdminNewsAsync(string culture)
        {
            bool isEn = culture == "en";
            var items = await _repo.GetAllAsync();

            return items
                .OrderByDescending(x => x.Date)
                .Select(n => new GetNewsItemDto
                {
                    Id = n.Id,
                    Date = n.Date.ToString("dd MMM yyyy"),
                    Image = n.Image,
                    VideoEmbed = n.VideoEmbed,

                    Title = isEn ? n.TitleEn : n.TitleTr,
                    Author = isEn ? n.AuthorEn : n.AuthorTr,
                    Category = isEn ? n.CategoryEn : n.CategoryTr,
                    Summary = isEn ? n.SummaryEn : n.SummaryTr,
                    Description = isEn ? n.DescriptionEn : n.DescriptionTr,
                })
                .ToList();
        }

        // -----------------------------------------------------------
        // CREATE
        // -----------------------------------------------------------
        public async Task<MessageDto> CreateAsync(CreateNewsItemDto dto)
        {
            try
            {
                var validation = ValidateLanguageFields(dto);
                if (!validation.IsSucces)
                    return validation;

                var entity = new NewsItem
                {
                    Date = dto.Date,
                    Image = dto.Image,
                    ExternalUrl = dto.ExternalUrl,
                    VideoEmbed = dto.VideoEmbed,

                    TitleTr = dto.TitleTr,
                    AuthorTr = dto.AuthorTr,
                    CategoryTr = dto.CategoryTr,
                    SummaryTr = dto.SummaryTr,
                    DescriptionTr = dto.DescriptionTr,

                    TitleEn = dto.TitleEn,
                    AuthorEn = dto.AuthorEn,
                    CategoryEn = dto.CategoryEn,
                    SummaryEn = dto.SummaryEn,
                    DescriptionEn = dto.DescriptionEn,
                };

                var createdId = await _repo.CreateAsync(entity);

                return new MessageDto
                {
                    IsSucces = true,
                    MessageText = "Haber başarıyla oluşturuldu.",
                    CreatedId = createdId,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new MessageDto
                {
                    IsSucces = false,
                    MessageText = $"Hata: {ex.Message}",
                    StatusCode = 500
                };
            }
        }

        // -----------------------------------------------------------
        // UPDATE
        // -----------------------------------------------------------
        public async Task<MessageDto> UpdateAsync(int id, CreateNewsItemDto dto)
        {
            try
            {
                var validation = ValidateLanguageFields(dto);
                if (!validation.IsSucces)
                    return validation;

                var n = await _repo.GetByIdAsync(id);
                if (n == null)
                {
                    return new MessageDto
                    {
                        IsSucces = false,
                        MessageText = "Haber bulunamadı.",
                        StatusCode = 404
                    };
                }

                n.Date = dto.Date;
                n.Image = dto.Image;
                n.ExternalUrl = dto.ExternalUrl;
                n.VideoEmbed = dto.VideoEmbed;

                n.TitleTr = dto.TitleTr;
                n.AuthorTr = dto.AuthorTr;
                n.CategoryTr = dto.CategoryTr;
                n.SummaryTr = dto.SummaryTr;
                n.DescriptionTr = dto.DescriptionTr;

                n.TitleEn = dto.TitleEn;
                n.AuthorEn = dto.AuthorEn;
                n.CategoryEn = dto.CategoryEn;
                n.SummaryEn = dto.SummaryEn;
                n.DescriptionEn = dto.DescriptionEn;

                await _repo.UpdateAsync(n);

                return new MessageDto
                {
                    IsSucces = true,
                    MessageText = "Haber başarıyla güncellendi.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new MessageDto
                {
                    IsSucces = false,
                    MessageText = "Hata: " + ex.Message,
                    StatusCode = 500
                };
            }
        }

        // -----------------------------------------------------------
        // DELETE
        // -----------------------------------------------------------
        public async Task<MessageDto> DeleteAsync(int id)
        {
            try
            {
                var result = await _repo.DeleteAsync(id);
                if (!result)
                {
                    return new MessageDto
                    {
                        IsSucces = false,
                        MessageText = "Silinecek haber bulunamadı.",
                        StatusCode = 404
                    };
                }

                return new MessageDto
                {
                    IsSucces = true,
                    MessageText = "Haber başarıyla silindi.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new MessageDto
                {
                    IsSucces = false,
                    MessageText = "Hata: " + ex.Message,
                    StatusCode = 500
                };
            }
        }

        // -----------------------------------------------------------
        // VALIDATION
        // -----------------------------------------------------------
        private MessageDto ValidateLanguageFields(CreateNewsItemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TitleTr))
                return Error("Türkçe başlık zorunludur.");
            if (string.IsNullOrWhiteSpace(dto.TitleEn))
                return Error("English title is required.");

            if (string.IsNullOrWhiteSpace(dto.AuthorTr))
                return Error("Türkçe yazar zorunludur.");
            if (string.IsNullOrWhiteSpace(dto.AuthorEn))
                return Error("English author is required.");

            if (string.IsNullOrWhiteSpace(dto.CategoryTr))
                return Error("Türkçe kategori zorunludur.");
            if (string.IsNullOrWhiteSpace(dto.CategoryEn))
                return Error("English category is required.");

            if (string.IsNullOrWhiteSpace(dto.SummaryTr))
                return Error("Türkçe özet zorunludur.");
            if (string.IsNullOrWhiteSpace(dto.SummaryEn))
                return Error("English summary is required.");

            if (string.IsNullOrWhiteSpace(dto.DescriptionTr))
                return Error("Türkçe açıklama zorunludur.");
            if (string.IsNullOrWhiteSpace(dto.DescriptionEn))
                return Error("English description is required.");

            

            return new MessageDto { IsSucces = true };
        }

        private MessageDto Error(string msg) =>
            new MessageDto
            {
                IsSucces = false,
                MessageText = msg,
                StatusCode = 400
            };
    }
}