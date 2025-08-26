﻿document.addEventListener('DOMContentLoaded', () => {
    const slides = document.querySelectorAll('.slider-fondo .slide');
    let index = 0;

    setInterval(() => {
        slides[index].classList.remove('active');
        index = (index + 1) % slides.length;
        slides[index].classList.add('active');
    }, 6000); // Cambia cada 6 segundos
});
