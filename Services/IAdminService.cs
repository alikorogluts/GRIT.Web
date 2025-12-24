using GRIT.Web.Models.DTO;

namespace GRIT.Web.Services
{
    public interface IAdminService
    {
        Task<List<GetNewsItemDto>> GetAdminNewsAsync(string culture);
        Task<MessageDto> CreateAsync(CreateNewsItemDto dto);
        Task<MessageDto> UpdateAsync(int id, CreateNewsItemDto dto);
        Task<MessageDto> DeleteAsync(int id);
    }
}