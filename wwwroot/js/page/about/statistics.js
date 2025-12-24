
    function initStatisticsSection() {
    const counters = document.querySelectorAll('.stat-number');
    if (counters.length === 0) return;

    const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
    if (entry.isIntersecting) {
    const counter = entry.target;
    const target = +counter.getAttribute('data-count');
    const duration = 2000; // 2 saniye

    // Daha yumuşak animasyon için başlangıç zamanı
    let startTime = null;

    const animate = (currentTime) => {
    if (!startTime) startTime = currentTime;
    const progress = Math.min((currentTime - startTime) / duration, 1);

    // Ease-out efekti (yavaşça durma)
    const easeProgress = 1 - Math.pow(1 - progress, 3);

    counter.textContent = Math.floor(easeProgress * target);

    if (progress < 1) {
    requestAnimationFrame(animate);
} else {
    counter.textContent = target; // Emin olmak için son değeri set et
}
};

    requestAnimationFrame(animate);
    observer.unobserve(counter); // Tekrar çalışmasını engelle
}
});
}, { threshold: 0.5 }); // %50'si görünür olunca başla

    counters.forEach(counter => observer.observe(counter));
}

    document.addEventListener('DOMContentLoaded', function() {
    initStatisticsSection();
});
