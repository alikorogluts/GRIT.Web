using GRIT.Web.Models.DTO;

namespace GRIT.Web.Services
{
    public interface IHomeService
    {
        Task<List<GetNewsItemDto>> GetHomeNewsAsync(string culture);
    }
}

