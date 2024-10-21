function GuardarDetalle() {
    var pedidoID = $('#PedidoID').val(); // Asegúrate de que el ID del pedido esté en un campo oculto.
    var platoID = $('#PlatoID').val();
    var cantidad = $('#Cantidad').val();

    // Validaciones básicas
    // if (platoID == 0) {
    //     $('#errorMensajePlato').show();
    //     return;
    // } else {
    //     $('#errorMensajePlato').hide();
    // }

    // if (cantidad <= 0) {
    //     $('#errorMensajeCantidad').show();
    //     return;
    // } else {
    //     $('#errorMensajeCantidad').hide();
    // }

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
                var fila = '<tr>' +
                    '<td style="text-align: center;">' + detalle.nombrePlato + '</td>' +
                    '<td style="text-align: center;">' + detalle.cantidad + '</td>' +
                    '<td style="text-align: center;">' + detalle.precioPlato + '</td>' +
                    '<td style="text-align: center;">' + detalle.subtotal + '</td>' +
                    '<td style="text-align: center;">' +
                    '<button class="btn btn-danger btn-sm" onclick="EliminarDetalle(' + detalle.DetallePedidoID + ')">' +
                    '<i class="lni lni-trash-can"></i></button>' +
                    '</td>' +
                    '</tr>';
                
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

function EliminarDetalle(detallePedidoID) {
    // Llamada AJAX para eliminar el detalle del pedido
    $.ajax({
        url: '/Pedidos/EliminarDetalle',
        type: 'POST',
        data: { DetallePedidoID: detallePedidoID },
        success: function (response) {
            if (response.exito) {
                var pedidoID = $('#PedidoID').val();
                CargarDetalles(pedidoID); // Recarga los detalles después de eliminar
            }
        },
        error: function (error) {
            console.log(error);
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
