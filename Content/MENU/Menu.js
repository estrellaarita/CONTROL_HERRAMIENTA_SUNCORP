// Add this to your existing JavaScript file or create a new one

// Toggle the lateral menu on click
document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.querySelector(".navbar-toggler");
    const menu = document.querySelector(".navbar-nav");

    toggleButton.addEventListener("click", function () {
        menu.classList.toggle("show");
    });
});