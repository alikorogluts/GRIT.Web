using GRIT.Web.Models;
using System.Threading.Tasks;

namespace GRIT.Web.Repositories
{
    public interface INewsRepository
    {
        Task<List<NewsItem>> GetAllAsync();
        Task<NewsItem?> GetByIdAsync(int id);
        Task<int> CreateAsync(NewsItem entity);
        Task<bool> UpdateAsync(NewsItem entity);
        Task<bool> DeleteAsync(int id);
    }
}