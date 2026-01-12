using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using GRIT.Web.Data;

namespace GRIT.Web.Data 
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. appsettings.json dosyasını manuel olarak bul ve oku
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. Connection string'i al
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // --- DÜZELTİLEN KISIM BAŞLANGIÇ ---
            // Pomelo kütüphanesinin yeni sürümlerinde ServerVersion belirtmek zorunludur.
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            // --- DÜZELTİLEN KISIM BİTİŞ ---

            // 3. AppDbContext'i oluştur
            return new AppDbContext(builder.Options, configuration);
        }
    }
}