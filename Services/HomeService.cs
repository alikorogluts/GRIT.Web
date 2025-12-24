using GRIT.Web.Models.DTO;
using GRIT.Web.Repositories;

namespace GRIT.Web.Services
{
    public class HomeService : IHomeService
    {
        private readonly INewsRepository _repo;

        public HomeService(INewsRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<GetNewsItemDto>> GetHomeNewsAsync(string culture)
        {
            bool isEn = culture == "en";

            var items = await _repo.GetAllAsync(); // Repository’den ham entity gelir

            return items
                .OrderByDescending(x => x.Date)
                .Select(n => new GetNewsItemDto
                {
                    Id = n.Id,
                    Date = n.Date.ToString("dd MMM yyyy"),
                    Image = n.Image,
                    VideoEmbed = n.VideoEmbed,
                    ExternalUrl = n.ExternalUrl,
                    

                    Title = isEn ? n.TitleEn : n.TitleTr,
                    Author = isEn ? n.AuthorEn : n.AuthorTr,
                    Category = isEn ? n.CategoryEn : n.CategoryTr,
                    Summary = isEn ? n.SummaryEn : n.SummaryTr,
                    Description = isEn ? n.DescriptionEn : n.DescriptionTr,
                })
                .ToList();
        }
    }
}