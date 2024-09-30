// Mostrar el spinner cuando el DOM está completamente cargado
document.addEventListener("DOMContentLoaded", function () {
    showLoading();
});

// Ocultar el spinner cuando todos los recursos de la página hayan terminado de cargarse
window.addEventListener("load", function () {
    setTimeout(hideLoading, 500); // Espera un poco para asegurarse de que todo esté cargado
});

// Mostrar el spinner
function showLoading() {
    document.getElementById("loadingBackground").style.display = "flex";
}

// Ocultar el spinner y mostrar el contenido
function hideLoading() {
    document.getElementById("loadingBackground").style.display = "none";
    document.getElementById("content").style.display = "block"; // Mostrar el contenido
}
