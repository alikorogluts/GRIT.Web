using GRIT.Web.Data;
using GRIT.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GRIT.Web.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly AppDbContext _db;

    public NewsRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<NewsItem>> GetAllAsync()
    {
        return await _db.NewsItems
            .OrderByDescending(x => x.Date)
            .ToListAsync();
    }

    public async Task<NewsItem?> GetByIdAsync(int id)
    {
        return await _db.NewsItems.FindAsync(id);
    }

    public async Task<int> CreateAsync(NewsItem entity)
    {
        _db.NewsItems.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id; // oluşturulan id’yi döner
    }

    public async Task<bool> UpdateAsync(NewsItem entity)
    {
        _db.NewsItems.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.NewsItems.FindAsync(id);
        if (item == null) return false;

        _db.NewsItems.Remove(item);
        return await _db.SaveChangesAsync() > 0;
    }
}