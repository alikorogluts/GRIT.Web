// ======= HERO SECTION =======

function initHeroSection() {
    // Element kontrolü
    if (!document.querySelector('.hero-section')) return;

    // Hero text animations (GSAP)
    const heroTimeline = gsap.timeline({ defaults: { duration: 1.2, ease: "power3.out" } });

    heroTimeline.from('.hero-title', { y: 100, opacity: 0 })
        .from('.hero-subtitle', { y: 50, opacity: 0, delay: -0.8 })
        .from('.hero-description', { y: 30, opacity: 0, delay: -0.8 })
        .from('.hero-actions', { y: 20, opacity: 0, delay: -0.8 })
        .from('#heroAnimation', { scale: 0.8, opacity: 0, duration: 1.5, ease: "back.out(1.2)" }, 0.5);

    // Scroll indicator functionality
    const scrollIndicator = document.querySelector('.hero-scroll-indicator');
    if (scrollIndicator) {
        scrollIndicator.addEventListener('click', () => {
            // Gradient text animation
            gsap.to(".hero-title", {
                backgroundPosition: "100% 50%",
                duration: 2, // yavaşça gradient’e dönüş
                ease: "power2.inOut",
                repeat: -1, // sonsuz döngü
                yoyo: true, // geri dönüş efekti
                repeatDelay: 2 // 2 saniye bekle
            });
        });
    }

    // Lottie Animasyonu başlat
    const lottieContainer = document.getElementById('heroAnimation');
    if (lottieContainer && typeof lottie !== 'undefined') {
        lottie.loadAnimation({
            container: lottieContainer,
            renderer: 'svg',
            loop: true,
            autoplay: true,
            path: 'https://assets.codepen.io/35984/tapered_hello.json1' // Örnek Sci-Fi Data Loop
        });
    }
}