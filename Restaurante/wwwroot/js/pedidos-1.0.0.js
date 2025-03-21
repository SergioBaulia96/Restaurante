window.onload = ListadoPedidos();



function ListadoPedidos() {
    let fechaListado = document.getElementById("FechaListado").value || null;
    $.ajax({
        url: '../../Pedidos/ListadoPedidos',
        data: {
            fechaListado: fechaListado
        },
        type: 'POST',
        dataType: 'json',
        success: function (listadoPedidos) {
            if (listadoPedidos && listadoPedidos.length > 0) {
                $("#modalPedido").modal("hide");
                LimpiarModal();

                let tabla = '';

                $.each(listadoPedidos, function (index, pedidos) {
                    let fechaHora = new Date(pedidos.fechaPedido); // Convertimos a objeto Date
                    let fecha = fechaHora.toLocaleDateString('es-AR', { day: '2-digit', month: '2-digit', year: 'numeric' }); // Formato: DD/MM/YYYY
                    let hora = fechaHora.toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }); // Formato: HH:mm

                    // Verificar si el pedido está en estado "Listo"
                    let estadoListo = pedidos.estado === "Listo";
                    let botonEditar = estadoListo
                        ? '<button type="button" class="edit-button" disabled><svg class="edit-svgIcon" viewBox="0 0 512 512"><path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path></svg></button>'
                        : `<button type="button" class="edit-button" onclick="ModalEditar(${pedidos.pedidoID})"><svg class="edit-svgIcon" viewBox="0 0 512 512"><path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path></svg></button>`;

                    let botonEliminar = estadoListo
                        ? '<button type="button" class="delete-button" disabled><svg class="delete-svgIcon" viewBox="0 0 448 512"><path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path></svg></button>'
                        : `<button type="button" class="delete-button" onclick="ValidarEliminacion(${pedidos.pedidoID})"><svg class="delete-svgIcon" viewBox="0 0 448 512"><path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path></svg></button>`;

                    tabla += `
                    <tr>
                        <td><a type="button" class="btn btn-success" href="/Pedidos/DetallePedido/${pedidos.pedidoID}" title="Ver Detalle">
                        <i class="lni lni-eye"></i> Detalle</a>
                        <td class="d-none d-md-table-cell" style="text-align: center;">${pedidos.nombreMesero} ${pedidos.apellidoMesero}</td>
                        <td class="d-none d-md-table-cell" style="text-align: center;">${pedidos.numeroMesa}</td>
                        <td class="d-none d-md-table-cell" style="text-align: center;">${fecha}<br><span style="font-size: 0.8em;">${hora}</span></td>
                        <td class="d-none d-md-table-cell" style="text-align: center;">${pedidos.estado}</td>
                        <td class="d-none d-md-table-cell" style="text-align: center;">${pedidos.total}</td>
                        <td>${botonEditar}</td>
                        <td>${botonEliminar}</td>
                    </tr>`;
                });

                document.getElementById("tbody-pedidos").innerHTML = tabla;
            } else {
                console.log('La lista de pedidos está vacía o no se pudo cargar.');
            }
        },
        error: function (xhr, status) {
            console.error('Problemas al cargar la tabla. Estado:', status, 'Detalles:', xhr);
        }
    });
}


function LimpiarModal(){
    document.getElementById("MeseroID").value = 0;
    document.getElementById("MesaID").value = 0;
    document.getElementById("Estado").value = 0;
    document.getElementById("FechaPedido").value = "";
    document.getElementById("EditarCampos").style.display = "none";
}

function NuevoPedido(){
    $("#tituloModal").text("Nuevo Pedido");
    LimpiarModal();
    $("#modalPedido").modal("show");
}

function GuardarPedido() {
    let meseroID = document.getElementById("MeseroID").value;
    let mesaID = document.getElementById("MesaID").value;
    let estado = document.getElementById("Estado").value;
    let fechaPedido = document.getElementById("FechaPedido").value;
    let isValid = true;

    if (meseroID === "0") {
        document.getElementById("errorMensajeMesero").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeMesero").style.display = "none";
    }

    if (mesaID === "0") {
        document.getElementById("errorMensajeMesa").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeMesa").style.display = "none";
    }

    if (!isValid) {
        return;  // Detener la ejecución si isValid es false
    }

    $.ajax({
        url: '../../Pedidos/GuardarPedido',
        data: {
            PedidoID: document.getElementById("PedidoID").value,
            MeseroID: meseroID,
            MesaID: mesaID,
            Estado: estado,
            FechaPedido: fechaPedido
        },
        type: 'POST',
        dataType: 'json',
        success: function(resultado) {
            if (resultado.exito) {
                ListadoPedidos();
                Swal.fire({
                    icon: 'success',
                    title: 'Pedido Guardado',
                    text: resultado.mensaje,
                    showConfirmButton: false,
                    timer: 2000
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al guardar',
                    text: resultado.mensaje
                });
            }
        },
        error: function(xhr, status) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Hubo un problema al guardar el pedido. Intente nuevamente.',
            });
            console.log('Problemas al guardar Pedido');
        },
    });
}


function ModalEditar(PedidoID) {
    $.ajax({
        url: '../../Pedidos/TraerPedidosAlModal',
        data: { pedidoID: PedidoID },
        type: 'POST',
        dataType: 'json',
        success: function (listadoPedidos) {
            // Verifica si el listadoPedidos contiene datos y muestra alerta si está vacío
            if (!listadoPedidos || listadoPedidos.length === 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al cargar',
                    text: 'No se encontraron datos del pedido. Intente de nuevo.',
                });
                return;  // Detener la ejecución si no hay datos
            }

            // Asignación de datos si todo está bien
            let listadoPedido = listadoPedidos[0];
            document.getElementById("PedidoID").value = PedidoID;
            $("#tituloModal").text("Editar Pedido");

            document.getElementById("MeseroID").value = listadoPedido.meseroID;
            document.getElementById("MesaID").value = listadoPedido.mesaID;
            document.getElementById("Estado").value = listadoPedido.estado;
            document.getElementById("FechaPedido").value = listadoPedido.fechaPedido;

            // Mostrar campos solo si estamos editando
            document.getElementById("EditarCampos").style.display = "block";
            $("#modalPedido").modal("show");
        },
        error: function (xhr, status) {
            // Error en la llamada AJAX
            Swal.fire({
                icon: 'error',
                title: 'Error al cargar',
                text: 'No se pudo cargar el pedido. Intente de nuevo.',
            });
            console.log('Problemas al cargar Pedido:', status, xhr.responseText);
        }
    });
}


function ValidarEliminacion(PedidoID) {
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
            EliminarPedido(PedidoID);
        }
    });
}

function EliminarPedido(PedidoID) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../Pedidos/EliminarPedido',
                data: { pedidoID: PedidoID },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.exito) {
                        Swal.fire({
                            icon: 'success',
                            title: '¡Eliminado!',
                            text: 'El pedido ha sido eliminado correctamente.',
                            showConfirmButton: false,
                            timer: 1500
                        });
                        ListadoPedidos();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.mensaje || 'No se pudo eliminar el pedido. Intente de nuevo.',
                        });
                    }
                },
                error: function (xhr, status) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Hubo un problema al eliminar el pedido.',
                    });
                    console.log('Problemas al eliminar Pedido');
                }
            });
        }
    });
}
