// ======= HEADER & LAYOUT JS =======

document.addEventListener('DOMContentLoaded', function() {
    initHeader();
    initFooter();
});
if (typeof AOS !== 'undefined') {
    AOS.init({
        duration: 1000,
        once: true,
        offset: 100
    });
}

function initHeader() {
    const header = document.getElementById('header');
    const navbarCollapse = document.getElementById('navbarNav');
    const navbarToggler = document.querySelector('.navbar-toggler');
    const navLinks = document.querySelectorAll('.nav-link, .dropdown-item');

    // --- YENİ EKLENEN KISIM BAŞLANGIÇ (AKILLI HEADER) ---
    let lastScrollTop = 0; // Son konumu hafızada tutmak için

    window.addEventListener('scroll', function() {
        let currentScroll = window.scrollY || document.documentElement.scrollTop;

        // 1. Şeffaflık Ayarı (Glassmorphism)
        // Sayfa en tepede değilse (.scrolled ekle)
        if (currentScroll > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
            // En tepedeysek header KESİNLİKLE görünür olmalı (gizlemeyi iptal et)
            header.classList.remove('header-hidden');
        }

        // 2. Gizle / Göster Mantığı (Smart Sticky)
        // Mobil menü açıksa bu özelliği devre dışı bırak
        if (!navbarCollapse.classList.contains('show')) {

            // Eğer aşağı kaydırıyorsak VE belli bir miktar indiysek (>100px)
            if (currentScroll > lastScrollTop && currentScroll > 100) {
                // AŞAĞI İNİYOR: Gizle
                header.classList.add('header-hidden');
            } else {
                // YUKARI ÇIKIYOR: Göster
                header.classList.remove('header-hidden');
            }
        }

        // iOS'te sayfa en tepedeyken yukarı çekince oluşan negatif değeri engelle
        lastScrollTop = currentScroll <= 0 ? 0 : currentScroll;
    });
    // --- YENİ EKLENEN KISIM BİTİŞ ---


    // 3. Akıllı Link Yönetimi (Sayfa içi kaydırma - BURASI ESKİSİYLE AYNI)
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            const href = this.getAttribute('href');
            if (href && href.includes('#')) {
                const [path, hash] = href.split('#');
                const currentPath = window.location.pathname;

                if (path === currentPath || path === '' || href.startsWith('#')) {
                    const targetElement = document.getElementById(hash);
                    if (targetElement) {
                        e.preventDefault();

                        if (navbarCollapse.classList.contains('show')) {
                            if (typeof bootstrap !== 'undefined') {
                                const bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse) || new bootstrap.Collapse(navbarCollapse);
                                bsCollapse.hide();
                            }
                        }

                        if (typeof gsap !== 'undefined') {
                            gsap.to(window, {
                                duration: 1.2,
                                scrollTo: { y: targetElement, offsetY: 80 },
                                ease: "power2.inOut"
                            });
                        } else {
                            const elementPosition = targetElement.getBoundingClientRect().top + window.scrollY;
                            window.scrollTo({
                                top: elementPosition - 80,
                                behavior: 'smooth'
                            });
                        }
                    }
                }
            }
        });
    });

    // 4. Ekranın boş yerine tıklayınca menüyü kapatma (BURASI ESKİSİYLE AYNI)
    document.addEventListener('click', function(event) {
        if (navbarCollapse.classList.contains('show')) {
            if (!navbarCollapse.contains(event.target) && !navbarToggler.contains(event.target)) {
                if (typeof bootstrap !== 'undefined') {
                    const bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse);
                    if (bsCollapse) {
                        bsCollapse.hide();
                    }
                }
            }
        }
    });
}
function initFooter() {
    // Footer içindeki tüm linkleri seç
    const footerLinks = document.querySelectorAll('.footer-links a');

    footerLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            const href = this.getAttribute('href');

            // Eğer link boşsa işlem yapma
            if (!href || !href.includes('#')) return;

            // Linki parçala: Yol (/products) ve Hash (#webrisk)
            const [targetPath, targetHash] = href.split('#');

            // Şu anki sayfanın yolu
            const currentPath = window.location.pathname;

            // KONTROL: Hedef sayfa ile şu anki sayfa AYNI MI?
            // (Not: targetPath boşsa veya '/' ise veya currentPath ile eşleşiyorsa)
            const isSamePage = targetPath === currentPath || targetPath === '' || (targetPath === '/' && currentPath === '/');

            if (isSamePage) {
                // AYNI SAYFADAYIZ: Kaydırma işlemi yap
                const targetElement = document.getElementById(targetHash);

                if (targetElement) {
                    e.preventDefault(); // Sayfa yenilenmesini durdur

                    // Header yüksekliğini (100px) hesaba katarak kaydır
                    const headerOffset = 100;
                    const elementPosition = targetElement.getBoundingClientRect().top;
                    const offsetPosition = elementPosition + window.pageYOffset - headerOffset;

                    window.scrollTo({
                        top: offsetPosition,
                        behavior: "smooth"
                    });

                    // URL'in sonuna #webrisk ekle (Sayfa yenilenmeden)
                    history.pushState(null, null, '#' + targetHash);
                }
            }
            // FARKLI SAYFADAYSAK: Hiçbir şey yapma (else durumuna gerek yok),
            // Tarayıcı doğal olarak o sayfaya gidecek ve ID'yi bulacaktır.
        });
    });
}