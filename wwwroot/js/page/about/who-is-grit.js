
    function initAboutPreviewSection() {
    const featureItems = document.querySelectorAll('.feature-item');
    if(featureItems.length === 0) return;

    featureItems.forEach(item => {
    // Mouse Enter: Hafif sağa kayma ve border parlaması (CSS hallediyor ama JS ile desteklenebilir)
    item.addEventListener('mouseenter', () => {
    if(typeof gsap !== 'undefined') {
    // İkonun hafif büyümesi
    gsap.to(item.querySelector('.feature-icon'), {
    scale: 1.1, duration: 0.3, ease: "back.out(1.7)"
});
}
});

    // Mouse Leave: Reset
    item.addEventListener('mouseleave', () => {
    if(typeof gsap !== 'undefined') {
    gsap.to(item.querySelector('.feature-icon'), {
    scale: 1, duration: 0.3, ease: "power2.out"
});
}
});
});
}

    document.addEventListener('DOMContentLoaded', function() {
    initAboutPreviewSection();
});
