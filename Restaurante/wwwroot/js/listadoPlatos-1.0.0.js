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
                    let estadoPlatos = plato.disponible ? "Disponible" : "Sin Stock";
                let precioFormateado = `$ ${parseFloat(plato.precio).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                    contenidoTabla += `
                        <tr>
                            <td></td>
                            <td>${plato.nombrePlato}</td>
                            <td class="d-none d-md-table-cell">${plato.descripcion}</td>
                            <td>${precioFormateado}</td>
                            <td class="d-none d-md-table-cell">${estadoPlatos}</td>
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