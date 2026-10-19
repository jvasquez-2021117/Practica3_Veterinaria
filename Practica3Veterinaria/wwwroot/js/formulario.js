/* Cambia únicamente la presentación de los campos; la validación real es del servidor. */
(function () {
    "use strict";
    function iniciarFormulario() {
        const selector = document.querySelector("#tipo-mascota");
        if (!selector) return;
        const secciones = document.querySelectorAll(".especie-extra");
        const aviso = document.querySelector("#species-prompt");

        function refrescarEspecie() {
            const seleccion = selector.value;
            let haySeccion = false;
            secciones.forEach((seccion) => {
                const activo = seccion.dataset.tipo === seleccion;
                seccion.hidden = !activo;
                if (activo) haySeccion = true;
                seccion.querySelectorAll("[data-campo-especie]").forEach((campo) => {
                    campo.disabled = !activo;
                });
            });
            if (aviso) aviso.hidden = haySeccion;
        }

        selector.addEventListener("change", refrescarEspecie);
        refrescarEspecie();
    }
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", iniciarFormulario);
    } else {
        iniciarFormulario();
    }
})();
