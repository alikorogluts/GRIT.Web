document.addEventListener('DOMContentLoaded', function() {
    if (document.getElementById('map')) {
        initMap();
    }

    const contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', handleFormSubmit);
    }
});

// ... (DOMContentLoaded ve Listener kısımları aynı) ...

function initMap() {
    const mapElement = document.getElementById('map');

    // 1. Dil Kodunu Al (Yoksa varsayılan 'tr' olsun)
    const lang = window.formTranslations && window.formTranslations.languageCode
        ? window.formTranslations.languageCode
        : 'tr';

    // 2. Yükleniyor Yazısını Al
    const loadingText = window.formTranslations && window.formTranslations.mapLoading
        ? window.formTranslations.mapLoading
        : "Loading Map...";

    // 3. HARİTA URL'İ (OMÜ Teknopark Koordinatları ile)
    // &hl=${lang} kısmı dili değiştirir (hl=en veya hl=tr olur)
    const mapSrc = `https://maps.google.com/maps?q=OMÜ+Teknopark&t=&z=15&ie=UTF8&iwloc=&output=embed&hl=${lang}`;

    const htmlContent = `
        <div style="position: relative; width: 100%; height: 100%; background-color: #f0f0f0; border-radius: 8px; overflow: hidden;">
            
            <div id="map-loader" style="
                position: absolute; top: 0; left: 0; width: 100%; height: 100%; 
                display: flex; flex-direction: column; justify-content: center; align-items: center; 
                background-color: var(--bg-primary); z-index: 10; transition: opacity 0.5s ease;">
                
                <i class="fas fa-spinner fa-spin" style="font-size: 3rem; color: var(--primary-main); margin-bottom: 15px;"></i>
                
                <span style="color: #555; font-family: sans-serif; font-weight: 500;">
                    ${loadingText}
                </span>
            </div>

            <iframe 
                id="google-map-iframe"
                width="100%" height="100%" frameborder="0" 
                style="border:0; width:100%; height:100%; opacity: 0; transition: opacity 0.5s ease;" 
                src="${mapSrc}" 
                allowfullscreen loading="lazy" referrerpolicy="no-referrer-when-downgrade">
            </iframe>
        </div>
    `;

    mapElement.innerHTML = htmlContent;

    const iframe = document.getElementById('google-map-iframe');
    const loader = document.getElementById('map-loader');

    iframe.onload = function() {
        loader.style.opacity = '0';
        iframe.style.opacity = '1';
        setTimeout(() => {
            loader.style.display = 'none';
        }, 500);
    };
}

// ... (Diğer fonksiyonlar aynı kalacak) ...
// ... (showToast fonksiyonu aynı kalabilir) ...
function showToast(message, type = 'success') {
    const container = document.getElementById('toast-container');
    const bgColor = type === 'success' ? '#1A7E63' : '#dc3545';
    const icon = type === 'success' ? '<i class="fas fa-check-circle"></i>' : '<i class="fas fa-exclamation-circle"></i>';
    const toast = document.createElement('div');
    toast.className = 'custom-toast';
    toast.style.cssText = `background-color: ${bgColor}; color: white; padding: 15px 25px; margin-bottom: 10px; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.15); display: flex; align-items: center; gap: 10px; min-width: 300px; opacity: 0; transform: translateX(100%); transition: all 0.5s ease; font-family: sans-serif;`;
    toast.innerHTML = `${icon} <span>${message}</span>`;
    container.appendChild(toast);
    requestAnimationFrame(() => { toast.style.opacity = '1'; toast.style.transform = 'translateX(0)'; });
    setTimeout(() => { toast.style.opacity = '0'; toast.style.transform = 'translateX(100%)'; setTimeout(() => toast.remove(), 500); }, 4000);
}

async function handleFormSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const btn = form.querySelector('button[type="submit"]');
    const originalContent = btn.innerHTML;

    // Gönderiliyor yazısını al
    const sendingText = window.formTranslations && window.formTranslations.sending
        ? window.formTranslations.sending
        : "Sending...";

    btn.disabled = true;
    btn.innerHTML = `<i class="fas fa-spinner fa-spin me-2"></i>${sendingText}`;

    const formData = new FormData(form);
    const dataObj = Object.fromEntries(formData.entries());
    const csrfToken = form.querySelector('input[name="__RequestVerificationToken"]')?.value;

    // --- KRİTİK DÜZELTME: URL'i HTML'den alıyoruz ---
    // Eğer HTML'den gelmezse fallback olarak eski usul deneyebilir ama asıl çözüm apiUrl'dir.
    const postUrl = window.formTranslations && window.formTranslations.apiUrl
        ? window.formTranslations.apiUrl
        : '/Contact/SendMessage';

    try {
        const response = await fetch(postUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken
            },
            body: JSON.stringify(dataObj)
        });

        const result = await response.json();

        if (response.ok && result.isSucces) {
            showToast(result.messageText, 'success');
            form.reset();
        } else {
            const errorMsg = result.messageText || (window.formTranslations ? window.formTranslations.errorMessage : "Error");
            showToast(errorMsg, 'error');
        }

    } catch (error) {
        console.error('Fetch Hatası:', error);
        const connError = window.formTranslations ? window.formTranslations.connectionError : "Connection Error";
        showToast(connError, 'error');
    } finally {
        btn.disabled = false;
        btn.innerHTML = originalContent;
    }
}