

    // ======= REFERENCES SECTION ANIMATION =======
    function initReferences() {
    // Logo container'ın görünürlüğünü kontrol et
    if (!document.querySelector('#references')) return;

    // Timeline oluştur
    const tl = gsap.timeline({
    scrollTrigger: {
    trigger: "#references",
    start: "top 80%", // Ekranın %80'ine gelince başla
    toggleActions: "play none none reverse"
}
});

    // Glass container animasyonu (Genişleyerek açılma)
    tl.from(".glass-container", {
    scale: 0.9,
    opacity: 0,
    duration: 1,
    ease: "power3.out"
})
    // Logoların sırayla gelmesi (Stagger)
    .from(".ref-item", {
    y: 50,
    opacity: 0,
    duration: 0.8,
    stagger: 0.2, // 0.2 saniye arayla gelsinler
    ease: "back.out(1.7)" // Hafif yaylanma efekti
}, "-=0.5"); // Container animasyonu bitmeden 0.5sn önce başla
}
