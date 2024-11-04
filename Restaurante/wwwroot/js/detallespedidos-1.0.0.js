document.addEventListener('DOMContentLoaded', function(){
    var pedidoID = document.getElementById("PedidoID").value;
    CargarDetalles(pedidoID);
});

function GuardarDetalle() {
    var pedidoID = $('#PedidoID').val(); // Asegúrate de que el ID del pedido esté en un campo oculto.
    var platoID = $('#PlatoID').val();
    var cantidad = $('#Cantidad').val();


    if (platoID == 0) {
        $('#errorMensajePlato').show();
        return;
    } else {
        $('#errorMensajePlato').hide();
    }

    if (cantidad <= 0) {
        $('#errorMensajeCantidad').show();
        return;
    } else {
        $('#errorMensajeCantidad').hide();
    }

    // Llamada AJAX para guardar el detalle del pedido
    $.ajax({
        url: '/Pedidos/GuardarDetalle',
        type: 'POST',
        data: { PedidoID: pedidoID, PlatoID: platoID, Cantidad: cantidad },
        success: function (response) {
            if (response.exito) {
                // Actualiza la tabla de detalles del pedido
                CargarDetalles(pedidoID);
                $('#ModalDetalle').modal('hide'); // Cierra el modal
            } else {
                alert(response.mensaje);
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function CargarDetalles(pedidoID) {
    // Llamada AJAX para obtener el listado de detalles del pedido
    $.ajax({
        url: '/Pedidos/ListadoDetalle',
        type: 'GET',
        data: { PedidoID: pedidoID },
        success: function (response) {
            var tbody = $('#tbody-detallespedidos');
            tbody.empty(); // Limpiar el cuerpo de la tabla
            
            var subtotal = 0;

            // Itera sobre los detalles del pedido y agrega filas a la tabla
            $.each(response, function (index, detalle) {

                let precioFormateado = `$ ${parseFloat(detalle.precioPlato).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
                let subtotalFormateado = `$ ${parseFloat(detalle.subtotal).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;

                var fila = `
                <tr>
                    <td style="text-align: center;">${detalle.plato}</td>
                    <td style="text-align: center;">${detalle.cantidad}</td>
                    <td class="d-none d-md-table-cell" style="text-align: center;">${precioFormateado}</td>
                    <td class="d-none d-md-table-cell" style="text-align: center;">${subtotalFormateado}</td>
                    <td><button type="button" class="delete-button" onclick="EliminarDetalle(${detalle.detallePedidoID})"><svg class="delete-svgIcon" viewBox="0 0 448 512">
                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
                    </svg></button></td>
                </tr>
                `;
                
                tbody.append(fila);

                subtotal += detalle.subtotal;
            });

            // Mostrar el subtotal en algún elemento (puedes agregarlo a la tabla o un div aparte)
            $('#Subtotal').text('Subtotal: $' + subtotal.toFixed(2));
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function LimpiarModal(){
    document.getElementById("PedidoID").value = 0;
    document.getElementById("MenuID").value = 0; 
    document.getElementById("PlatoID").value = 0;
    document.getElementById("Cantidad").value = "";
}

function EliminarDetalle(detallePedidoID) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Esta acción no se puede deshacer",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Llamada AJAX para eliminar el detalle del pedido
            $.ajax({
                url: '/Pedidos/EliminarDetalle',
                type: 'POST',
                data: { DetallePedidoID: detallePedidoID },
                success: function (response) {
                    if (response.exito) {
                        Swal.fire({
                            icon: 'success',
                            title: '¡Eliminado!',
                            text: 'El detalle del pedido ha sido eliminado correctamente.',
                            showConfirmButton: false,
                            timer: 1500
                        });
                        var pedidoID = $('#PedidoID').val();
                        CargarDetalles(pedidoID); // Recarga los detalles después de eliminar
                    }
                },
                error: function (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Hubo un problema al eliminar el detalle del pedido.',
                    });
                    console.log(error);
                }
            });
        }
    });
}



function actualizarSubtotal(pedidoID) {
    $.ajax({
        url: '/Pedidos/CalcularSubtotal',
        type: 'GET',
        data: { PedidoID: pedidoID },
        success: function(response) {
            $('#Subtotal').text('Subtotal: $' + response.subtotal);
        }
    });
}
