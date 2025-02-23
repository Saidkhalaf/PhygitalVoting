animateSlogan();
function animateSlogan() {
    const slogan = document.querySelector('.slogan') as HTMLElement;
    let show = true;

    setInterval(() => {
        if (show) {
            slogan.style.width = `${slogan.scrollWidth}px`;
            slogan.style.opacity = '1';
        } else {
            slogan.style.width = '0';
            slogan.style.opacity = '0';
        }
        show = !show;
    }, 3000);
}

function initializeAnimations() {
    animateSlogan();

    const cards = document.querySelectorAll('.instruction-card') as NodeListOf<HTMLElement>;

    cards.forEach(card => {
        card.addEventListener('mousemove', (e: MouseEvent) => {
            if (card.classList.contains('spinning')) return;

            const rect = card.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            const centerX = rect.width / 2;
            const centerY = rect.height / 2;

            const rotateX = (y - centerY) / centerY * 10;
            const rotateY = (x - centerX) / centerX * 10;

            card.style.transform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
        });

        card.addEventListener('mouseleave', () => {
            card.style.transform = 'rotateX(0) rotateY(0)';
        });

        card.addEventListener('click', () => {
            card.style.transition = 'transform 0.6s';
            card.style.transform += ' rotateY(360deg)';

            setTimeout(() => {
                card.style.transition = '';
                card.style.transform = 'rotateX(0) rotateY(0)';
            }, 600);
        });

    });
}

initializeAnimations();
