using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using GRIT.Web.Data;

namespace GRIT.Web.Data // ⚠️ AppDbContext ile aynı namespace olduğundan emin ol
{
    // Bu sınıf SADECE "dotnet ef" komutları çalışırken devreye girer.
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

            builder.UseSqlServer(connectionString);

            // 3. AppDbContext'i, IConfiguration parametresiyle beraber manuel oluştur
            return new AppDbContext(builder.Options, configuration);
        }
    }
}