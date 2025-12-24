using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GRIT.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoEmbed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleTr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorTr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryTr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryTr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionTr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "Email", "FullName", "Password" },
                values: new object[] { 1, "halil.akinci@artvin.edu.tr", "GRIT Yönetici", "$2a$11$uJ0Qr2YYPbDCBK1d26Qt7ewRkw6n9ii333yqeELipP/zdtpfR2tpW" });

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "Id", "AuthorEn", "AuthorTr", "CategoryEn", "CategoryTr", "Date", "DescriptionEn", "DescriptionTr", "ExternalUrl", "Image", "SummaryEn", "SummaryTr", "TitleEn", "TitleTr", "VideoEmbed" },
                values: new object[,]
                {
                    { 1, "Sigortacı Gazetesi", "Sigortacı Gazetesi", "Interview", "Röportaj", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>Prof. Dr. Halil Akıncı discussed the increasing importance of wildfire risk due to climate change.</p>", "<p>Prof. Dr. Halil Akıncı, iklim değişikliği ile artan orman yangını risklerinin önemini anlattı.</p>", "https://sigortacigazetesi.com.tr/orman-yangini-riskini-azaltmak-icin-haritalar-ve-modellemeler-hayati-rol-oynuyor/", "/image/halil_2.jpg", "Prof. Dr. Halil Akıncı explained the growing need for wildfire susceptibility maps.", "GRİT’in kurucusu Prof. Dr. Halil Akıncı, orman yangını duyarlılık haritalarına duyulan ihtiyacı anlattı.", "Maps and modeling play a vital role in reducing wildfire risks", "Orman yangını riskini azaltmak için haritalar ve modellemeler hayati rol oynuyor", null },
                    { 2, "Sigorta Dünyası", "Sigorta Dünyası", "Interview", "Röportaj", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>With new generation mapping technologies, high-risk areas can be identified precisely.</p>", "<p>Yeni nesil haritalama teknolojileri sayesinde riskli alanlar nokta atışı tespit edilebiliyor.</p>", "https://www.sigortadunyasi.com.tr/2025/09/09/orman-yangini-riskinin-yonetiminde-sigortaciliga-bilimsel-modelleme-katkisi/", "/image/halil_2.jpg", "Prof. Dr. Halil Akıncı stated that new 10m resolution maps attract strong interest in the sector.", "Prof. Dr. Halil AKINCI, yeni 10m çözünürlüklü haritaların sektörde ilgi gördüğünü belirtti.", "Wildfire susceptibility maps provide major value for the insurance sector", "Orman yangını duyarlılık haritaları sigorta sektörü için ciddi katkı sağlıyor", null },
                    { 3, "Sigorta Ekranı", "Sigorta Ekranı", "Live Broadcast", "Canlı Yayın", new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>The datasets and algorithms were explained in detail during the live broadcast.</p>", "<p>Canlı yayında veri setleri ve algoritmalar detaylandırıldı.</p>", "https://www.youtube.com/watch?v=hVF3FMabwl0", "https://img.youtube.com/vi/hVF3FMabwl0/maxresdefault.jpg", "Prof. Dr. Halil Akıncı discussed technical details of wildfire susceptibility models.", "Prof. Dr. Halil AKINCI, orman yangını duyarlılık modellerinin teknik detaylarını anlattı.", "Natural Disasters and Insurance 'ACADEMIC PERSPECTIVE' Wildfires", "Doğal Afetler ve Sigorta “AKADEMİK PERSPEKTİF“ Orman Yangınları", "https://www.youtube-nocookie.com/embed/hVF3FMabwl0?autoplay=1" },
                    { 4, "Sigortacı Gazetesi", "Sigortacı Gazetesi", "Interview", "Röportaj", new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>Risk maps help prevent potential losses and customer grievances.</p>", "<p>Risk haritaları mağduriyetlerin önüne geçiyor.</p>", "https://sigortacigazetesi.com.tr/detayli-risk-haritalari-sigortacilik-icin-cok-onemli-hale-geldi/", "/image/sedat.jpg", "Prof. Dr. Sedat Doğan highlighted the importance of risk maps in policy pricing.", "Prof. Dr. Sedat Doğan, poliçe fiyatlamasında risk haritalarının önemini anlattı.", "Detailed risk maps have become crucial for the insurance industry", "Detaylı risk haritaları sigortacılık için çok önemli hale geldi", null },
                    { 5, "Sigortacı Gazetesi", "Sigortacı Gazetesi", "Interview", "Röportaj", new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>Increasing rainfall continues to raise landslide risks.</p>", "<p>Artan yağışlar heyelan riskini artırıyor.</p>", "https://sigortacigazetesi.com.tr/heyelanlara-karsi-sigorta-yaptirmak-artik-kacinilmaz/", "/image/cem.jpg", "Assoc. Prof. Cem Kılıçoğlu emphasized the importance of landslide insurance.", "Doç. Dr. Cem Kılıçoğlu, heyelan sigortasının önemini vurguladı.", "Getting insured against landslides is now inevitable!", "Heyelanlara karşı sigorta yaptırmak artık kaçınılmaz!", null },
                    { 6, "Sigorta Ekranı", "Sigorta Ekranı", "Live Broadcast", "Canlı Yayın", new DateTime(2025, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>The financial cost of floods increases every year.</p>", "<p>Taşkın maliyetleri her yıl artıyor.</p>", "https://www.youtube.com/watch?v=YZZq0qqq7tk", "https://img.youtube.com/vi/YZZq0qqq7tk/maxresdefault.jpg", "Prof. Dr. Sedat Doğan explained the financial impacts of flood models.", "Prof. Dr. Sedat DOĞAN, taşkın modellerinin finansal etkilerini anlattı.", "Why do floods and overflows closely concern the insurance sector?", "Sel ve taşkınlar sigorta sektörünü neden yakından ilgilendiriyor?", "https://www.youtube.com/embed/YZZq0qqq7tk?autoplay=1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "NewsItems");
        }
    }
}
