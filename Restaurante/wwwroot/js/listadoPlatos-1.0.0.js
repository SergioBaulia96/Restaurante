window.onload = ListadosPlatos();

function ListadosPlatos() {

    $.ajax({
        url: '../../Menus/ListadosPlatos',
        data: {
        },
        type: 'POST',
        dataType: 'json',
        success: function (menusMostrar) {

            let contenidoTabla = ``;

            $.each(menusMostrar, function (index, menu) {

                contenidoTabla += `
                <tr>
                    <td>${menu.nombre}</td>
                    <td colspan="4"></td>
                </tr>
            `;

                $.each(menu.listadoPlatos, function (index, plato) {
                    let precioFormateado = `$ ${parseFloat(plato.precio).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                    contenidoTabla += `
                        <tr>
                            <td></td>
                            <td>${plato.nombrePlato}</td>
                            <td class="d-none d-md-table-cell">${plato.descripcion}</td>
                            <td>${precioFormateado}</td>
                        </tr>
                    `;


                });

            });

            document.getElementById("tbody-listadoPlatos").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function ImprimirListado() {
    var doc = new jsPDF("p", "mm", "a4");// Tamaño A4 horizontal

    // Cargar el logo
    var logo = new Image();
    logo.src = '/img/LogoRedondo.png'; // Ruta correcta al logo

    // Esperar a que la imagen se cargue antes de agregarla al PDF
    logo.onload = function () {
        // Agregar el logo al PDF
        doc.addImage(logo, 'PNG', 15, 10, 30, 30); // Posición (x, y) y tamaño (ancho, alto)

        // Continuar con el resto del código...
        var totalPagesExp = "{total_pages_count_string}";

        var pageContent = function (data) {
            var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
            var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();

            // HEADER
            doc.setFontSize(20);
            doc.setFont("helvetica", "bold");
            doc.setTextColor(0, 143, 81); // Color verde
            doc.text("MENÚ", pageWidth / 2, 50, { align: "center" });

            // FOOTER
            doc.setFontSize(10);
            doc.setTextColor(100); // Color gris
            doc.setFontStyle("normal");
            var str = "Página " + data.pageCount;
            if (typeof doc.putTotalPages === "function") {
                str = str + " de " + totalPagesExp;
            }
            doc.text(str, pageWidth - 20, pageHeight - 10, { align: "right" });

            // Información adicional en el footer
            doc.setFontSize(10);
            doc.text("Dirección: Lapalma 365, Brinkmann", 20, pageHeight - 10);
            doc.text("Teléfono: +54 9 3562-503748", 20, pageHeight - 5);
        };

        // Obtener la tabla HTML
        var elem = document.getElementById("ListadoPlatos");
        var res = doc.autoTableHtmlToJson(elem);

        // Configuración de la tabla
        doc.autoTable(res.columns, res.data, {
            addPageContent: pageContent,
            theme: "grid",
            styles: {
                fillColor: [0, 143, 81], // Color de fondo del encabezado
                textColor: [255, 255, 255], // Color del texto del encabezado
                fontStyle: "bold",
                halign: "center",
                fontSize: 10,
            },
            columnStyles: {
                0: { halign: "center", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 1
                1: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 2
                2: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 3
                3: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 4
                4: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0], cellWidth: 30 }, // Columna 5
                5: { halign: "right", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 6
                6: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 7
                7: { halign: "left", fillColor: [255, 255, 255], fontSize: 10, textColor: [0, 0, 0] }, // Columna 8
            },
            margin: { top: 60 }, // Margen superior para el encabezado y el logo
            didDrawPage: pageContent, // Dibujar header y footer en cada página
        });

        // Total de páginas
        if (typeof doc.putTotalPages === "function") {
            doc.putTotalPages(totalPagesExp);
        }

        // Abrir el PDF en una nueva ventana
        var string = doc.output("datauristring");
        var iframe = "<iframe width='100%' height='100%' src='" + string + "'></iframe>";
        var x = window.open();
        x.document.open();
        x.document.write(iframe);
        x.document.close();
    };

    // Si la imagen no se carga, mostrar un error
    logo.onerror = function () {
        console.error("Error al cargar el logo.");
    };
}