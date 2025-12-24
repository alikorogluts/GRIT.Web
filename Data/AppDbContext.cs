using GRIT.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Gerekli
namespace GRIT.Web.Data
{
    

        public class AppDbContext : DbContext
        {
            
            private readonly IConfiguration _configuration;
        
            // 2. Constructor'da veriyi alıyoruz

            public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
            {
               
                // Gelen veriyi (configuration), içerideki değişkene (_configuration) atamazsan null kalır.
                _configuration = configuration; 
            }

            // Veritabanındaki Tablolarımız
            public DbSet<AdminUser> AdminUsers { get; set; }
            public DbSet<NewsItem> NewsItems { get; set; }
            
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminName = _configuration["SeedData:AdminUser:FullName"];
            var adminEmail = _configuration["SeedData:AdminUser:Email"];
            var adminPassword = _configuration["SeedData:AdminUser:PasswordHash"];
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {   Id = 1,
                    FullName =adminName??"Admin",
                    Email = adminEmail,
                    Password = adminPassword
                }
                
                );

           modelBuilder.Entity<NewsItem>().HasData(

            new NewsItem
            {
                Id = 1,
                Date = DateTime.Parse("2025-08-01"),

                Image = "/image/halil_2.jpg",
                ExternalUrl = "https://sigortacigazetesi.com.tr/orman-yangini-riskini-azaltmak-icin-haritalar-ve-modellemeler-hayati-rol-oynuyor/",
                VideoEmbed = null,

                // TR
                TitleTr = "Orman yangını riskini azaltmak için haritalar ve modellemeler hayati rol oynuyor",
                AuthorTr = "Sigortacı Gazetesi",
                CategoryTr = "Röportaj",
                SummaryTr = "GRİT’in kurucusu Prof. Dr. Halil Akıncı, orman yangını duyarlılık haritalarına duyulan ihtiyacı anlattı.",
                DescriptionTr = "<p>Prof. Dr. Halil Akıncı, iklim değişikliği ile artan orman yangını risklerinin önemini anlattı.</p>",

                // EN
                TitleEn = "Maps and modeling play a vital role in reducing wildfire risks",
                AuthorEn = "Sigortacı Gazetesi",
                CategoryEn = "Interview",
                SummaryEn = "Prof. Dr. Halil Akıncı explained the growing need for wildfire susceptibility maps.",
                DescriptionEn = "<p>Prof. Dr. Halil Akıncı discussed the increasing importance of wildfire risk due to climate change.</p>",
            },

            new NewsItem
            {
                Id = 2,
                Date = DateTime.Parse("2025-09-09"),

                Image = "/image/halil_2.jpg",
                ExternalUrl = "https://www.sigortadunyasi.com.tr/2025/09/09/orman-yangini-riskinin-yonetiminde-sigortaciliga-bilimsel-modelleme-katkisi/",
                VideoEmbed = null,

                // TR
                TitleTr = "Orman yangını duyarlılık haritaları sigorta sektörü için ciddi katkı sağlıyor",
                AuthorTr = "Sigorta Dünyası",
                CategoryTr = "Röportaj",
                SummaryTr = "Prof. Dr. Halil AKINCI, yeni 10m çözünürlüklü haritaların sektörde ilgi gördüğünü belirtti.",
                DescriptionTr = "<p>Yeni nesil haritalama teknolojileri sayesinde riskli alanlar nokta atışı tespit edilebiliyor.</p>",

                // EN
                TitleEn = "Wildfire susceptibility maps provide major value for the insurance sector",
                AuthorEn = "Sigorta Dünyası",
                CategoryEn = "Interview",
                SummaryEn = "Prof. Dr. Halil Akıncı stated that new 10m resolution maps attract strong interest in the sector.",
                DescriptionEn = "<p>With new generation mapping technologies, high-risk areas can be identified precisely.</p>",
            },

            new NewsItem
            {
                Id = 3,
                Date = DateTime.Parse("2025-08-19"),

                Image = "https://img.youtube.com/vi/hVF3FMabwl0/maxresdefault.jpg",
                ExternalUrl = "https://www.youtube.com/watch?v=hVF3FMabwl0",
                VideoEmbed = "https://www.youtube-nocookie.com/embed/hVF3FMabwl0?autoplay=1",

                // TR
                TitleTr = "Doğal Afetler ve Sigorta “AKADEMİK PERSPEKTİF“ Orman Yangınları",
                AuthorTr = "Sigorta Ekranı",
                CategoryTr = "Canlı Yayın",
                SummaryTr = "Prof. Dr. Halil AKINCI, orman yangını duyarlılık modellerinin teknik detaylarını anlattı.",
                DescriptionTr = "<p>Canlı yayında veri setleri ve algoritmalar detaylandırıldı.</p>",

                // EN
                TitleEn = "Natural Disasters and Insurance 'ACADEMIC PERSPECTIVE' Wildfires",
                AuthorEn = "Sigorta Ekranı",
                CategoryEn = "Live Broadcast",
                SummaryEn = "Prof. Dr. Halil Akıncı discussed technical details of wildfire susceptibility models.",
                DescriptionEn = "<p>The datasets and algorithms were explained in detail during the live broadcast.</p>",
            },

            new NewsItem
            {
                Id = 4,
                Date = DateTime.Parse("2025-10-10"),

                Image = "/image/sedat.jpg",
                ExternalUrl = "https://sigortacigazetesi.com.tr/detayli-risk-haritalari-sigortacilik-icin-cok-onemli-hale-geldi/",
                VideoEmbed = null,

                // TR
                TitleTr = "Detaylı risk haritaları sigortacılık için çok önemli hale geldi",
                AuthorTr = "Sigortacı Gazetesi",
                CategoryTr = "Röportaj",
                SummaryTr = "Prof. Dr. Sedat Doğan, poliçe fiyatlamasında risk haritalarının önemini anlattı.",
                DescriptionTr = "<p>Risk haritaları mağduriyetlerin önüne geçiyor.</p>",

                // EN
                TitleEn = "Detailed risk maps have become crucial for the insurance industry",
                AuthorEn = "Sigortacı Gazetesi",
                CategoryEn = "Interview",
                SummaryEn = "Prof. Dr. Sedat Doğan highlighted the importance of risk maps in policy pricing.",
                DescriptionEn = "<p>Risk maps help prevent potential losses and customer grievances.</p>",
            },

            new NewsItem
            {
                Id = 5,
                Date = DateTime.Parse("2025-10-10"),

                Image = "/image/cem.jpg",
                ExternalUrl = "https://sigortacigazetesi.com.tr/heyelanlara-karsi-sigorta-yaptirmak-artik-kacinilmaz/",
                VideoEmbed = null,

                // TR
                TitleTr = "Heyelanlara karşı sigorta yaptırmak artık kaçınılmaz!",
                AuthorTr = "Sigortacı Gazetesi",
                CategoryTr = "Röportaj",
                SummaryTr = "Doç. Dr. Cem Kılıçoğlu, heyelan sigortasının önemini vurguladı.",
                DescriptionTr = "<p>Artan yağışlar heyelan riskini artırıyor.</p>",

                // EN
                TitleEn = "Getting insured against landslides is now inevitable!",
                AuthorEn = "Sigortacı Gazetesi",
                CategoryEn = "Interview",
                SummaryEn = "Assoc. Prof. Cem Kılıçoğlu emphasized the importance of landslide insurance.",
                DescriptionEn = "<p>Increasing rainfall continues to raise landslide risks.</p>",
            },

            new NewsItem
            {
                Id = 6,
                Date = DateTime.Parse("2025-08-26"),

                Image = "https://img.youtube.com/vi/YZZq0qqq7tk/maxresdefault.jpg",
                ExternalUrl = "https://www.youtube.com/watch?v=YZZq0qqq7tk",
                VideoEmbed = "https://www.youtube.com/embed/YZZq0qqq7tk?autoplay=1",

                // TR
                TitleTr = "Sel ve taşkınlar sigorta sektörünü neden yakından ilgilendiriyor?",
                AuthorTr = "Sigorta Ekranı",
                CategoryTr = "Canlı Yayın",
                SummaryTr = "Prof. Dr. Sedat DOĞAN, taşkın modellerinin finansal etkilerini anlattı.",
                DescriptionTr = "<p>Taşkın maliyetleri her yıl artıyor.</p>",

                // EN
                TitleEn = "Why do floods and overflows closely concern the insurance sector?",
                AuthorEn = "Sigorta Ekranı",
                CategoryEn = "Live Broadcast",
                SummaryEn = "Prof. Dr. Sedat Doğan explained the financial impacts of flood models.",
                DescriptionEn = "<p>The financial cost of floods increases every year.</p>",
            }
        );
        }
        }
}