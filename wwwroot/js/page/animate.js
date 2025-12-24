let scene, camera, renderer, particles;
const count = window.innerWidth <= 768 ? 4000 : 12000;
let currentState = 'sphere';

// --- YENİ EKLENEN KISIM: KELİME LİSTESİ ---
const textList = ["GRIT", "Akıl", "Bilim", "Güven"]; // Buraya istediğin kelimeleri ekle
let textIndex = 0; // Hangi kelimede olduğumuzu tutar

// SABİT RENK AYARLARI
const STATIC_COLOR = {
    hueStart: 0.14,       // Logonuzun Zeytin Yeşili tonu
    hueRange: 0.03,       // Sabit ve kararlı bir renk
    saturation: 0.70,     // Logonuzdaki zeytin tonunun doygunluğu
    lightnessBase: 0.30,  // Koyu ve net duruş
    lightnessRange: 0.10  // Hafif ışık oyunları
};

function init() {
    const heroSection = document.getElementById('hero');
    if (!heroSection) return;

    scene = new THREE.Scene();

    const width = window.innerWidth;
    const height = heroSection.clientHeight || window.innerHeight;

    camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);

    renderer = new THREE.WebGLRenderer({
        antialias: true,
        alpha: true
    });
    renderer.setSize(width, height);
    renderer.setClearColor(0x000000, 0);

    renderer.domElement.style.position = 'absolute';
    renderer.domElement.style.top = '0';
    renderer.domElement.style.left = '0';
    renderer.domElement.style.width = '100%';
    renderer.domElement.style.height = '100%';
    renderer.domElement.style.pointerEvents = 'none';
    renderer.domElement.style.zIndex = '1';

    heroSection.appendChild(renderer.domElement);

    // Responsive Kamera Ayarları
    if (window.innerWidth <= 768) {
        camera.position.x = 0;
        camera.position.y = -12;
        camera.position.z = 45;
    }
    else {
        camera.position.x = -15;
        camera.position.y = 0;
        camera.position.z = 25;
    }

    createParticles();

    // --- GÜNCELLEME: Listeden ilk kelime ile başla ---
    morphToText(textList[textIndex]);

    animate();
}

function createParticles() {
    const geometry = new THREE.BufferGeometry();
    const positions = new Float32Array(count * 3);
    const colors = new Float32Array(count * 3);

    function sphericalDistribution(i) {
        const phi = Math.acos(-1 + (2 * i) / count);
        const theta = Math.sqrt(count * Math.PI) * phi;
        const radius = 8;

        return {
            x: radius * Math.cos(theta) * Math.sin(phi),
            y: radius * Math.sin(theta) * Math.sin(phi),
            z: radius * Math.cos(phi)
        };
    }

    for (let i = 0; i < count; i++) {
        const point = sphericalDistribution(i);

        positions[i * 3] = point.x + (Math.random() - 0.5) * 0.5;
        positions[i * 3 + 1] = point.y + (Math.random() - 0.5) * 0.5;
        positions[i * 3 + 2] = point.z + (Math.random() - 0.5) * 0.5;

        const color = new THREE.Color();
        const depth = Math.sqrt(point.x * point.x + point.y * point.y + point.z * point.z) / 8;

        color.setHSL(
            STATIC_COLOR.hueStart + depth * STATIC_COLOR.hueRange,
            STATIC_COLOR.saturation,
            STATIC_COLOR.lightnessBase + depth * STATIC_COLOR.lightnessRange
        );

        colors[i * 3] = color.r;
        colors[i * 3 + 1] = color.g;
        colors[i * 3 + 2] = color.b;
    }

    geometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));
    geometry.setAttribute('color', new THREE.BufferAttribute(colors, 3));

    const material = new THREE.PointsMaterial({
        size: 0.1,
        vertexColors: true,
        blending: THREE.AdditiveBlending,
        transparent: true,
        opacity: 0.9,
        sizeAttenuation: true
    });

    if (particles) scene.remove(particles);
    particles = new THREE.Points(geometry, material);
    scene.add(particles);
}

function createTextPoints(text) {
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');

    // Font boyutunu kelime uzunluğuna göre dinamik ayarlayabilirsin istersen
    // Şimdilik sabit:
    const fontSize = 100;
    const padding = 20;

    ctx.font = `bold ${fontSize}px Arial`;
    const textMetrics = ctx.measureText(text);
    const textWidth = textMetrics.width;
    const textHeight = fontSize;

    canvas.width = textWidth + padding * 2;
    canvas.height = textHeight + padding * 2;

    ctx.fillStyle = 'white';
    ctx.font = `bold ${fontSize}px Arial`;
    ctx.textBaseline = 'middle';
    ctx.textAlign = 'center';
    ctx.fillText(text, canvas.width / 2, canvas.height / 2);

    const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    const pixels = imageData.data;
    const points = [];
    const threshold = 128;

    for (let i = 0; i < pixels.length; i += 4) {
        if (pixels[i] > threshold) {
            const x = (i / 4) % canvas.width;
            const y = Math.floor((i / 4) / canvas.width);

            const density = 0.3;

            if (Math.random() < density) {
                points.push({
                    x: (x - canvas.width / 2) / (fontSize / 10),
                    y: -(y - canvas.height / 2) / (fontSize / 10)
                });
            }
        }
    }

    return points;
}

function morphToText(text) {
    currentState = 'text';
    const textPoints = createTextPoints(text);
    const positions = particles.geometry.attributes.position.array;
    const targetPositions = new Float32Array(count * 3);

    gsap.to(particles.rotation, {
        x: 0,
        y: 0,
        z: 0,
        duration: 0.5
    });

    const maxDistance = 50;
    const minDistance = 20;
    const verticalRange = 35;
    const depthRange = 30;

    for (let i = 0; i < count; i++) {
        if (i < textPoints.length) {
            targetPositions[i * 3] = textPoints[i].x;
            targetPositions[i * 3 + 1] = textPoints[i].y;
            targetPositions[i * 3 + 2] = 0;
        } else {
            const angle = Math.random() * Math.PI * 2;
            const distance = Math.random() * maxDistance + minDistance;
            const verticalBias = (Math.random() - 0.3) * verticalRange;

            targetPositions[i * 3] = Math.cos(angle) * distance;
            targetPositions[i * 3 + 1] = Math.sin(angle) * distance + verticalBias;
            targetPositions[i * 3 + 2] = (Math.random() - 0.5) * depthRange;
        }
    }

    // Pozisyon animasyonu
    for (let i = 0; i < positions.length; i += 3) {
        gsap.to(particles.geometry.attributes.position.array, {
            [i]: targetPositions[i],
            [i + 1]: targetPositions[i + 1],
            [i + 2]: targetPositions[i + 2],
            duration: 2.5,
            ease: "power3.out",
            onUpdate: () => {
                particles.geometry.attributes.position.needsUpdate = true;
            }
        });
    }

    // 4 saniye sonra küreye geri dön
    
}


function animate() {
    requestAnimationFrame(animate);

    if (currentState === 'sphere') {
        particles.rotation.y += 0.002;
    }

    renderer.render(scene, camera);
}

let resizeTimeout;
window.addEventListener('resize', () => {
    clearTimeout(resizeTimeout);
    resizeTimeout = setTimeout(() => {
        if (!renderer || !camera) return;

        const heroSection = document.getElementById('hero');
        const width = window.innerWidth;
        const height = heroSection ? heroSection.clientHeight : window.innerHeight;

        camera.aspect = width / height;
        camera.updateProjectionMatrix();
        renderer.setSize(width, height);

        // Mobile özel yeniden konumlandırma mantığı buraya da eklenebilir 
        // ancak init'teki ilk yükleme genelde yeterlidir.
    }, 250);
});

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
} else {
    init();
}