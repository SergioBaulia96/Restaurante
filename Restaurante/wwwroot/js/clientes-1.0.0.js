window.onload = ListadoClientes();

function ListadoClientes()
{
    $.ajax({
        url: '../../Clientes/ListadoClientes',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function(listadoClientes){
            $("#ModalCliente").modal("hide");
            LimpiarModal();
            
            let tabla = ``

            $.each(listadoClientes, function(index, clientes){

                tabla += `
                <tr>
                    <td>${clientes.nombre}</td>
                    <td>${clientes.apellido}</td>
                    <td class="d-none d-md-table-cell">${clientes.email}</td>
                    <td class="d-none d-md-table-cell">${clientes.telefono}</td>
                    <td><button type="button" class="edit-button" onclick="ModalEditar(${clientes.clienteID})"><svg class="edit-svgIcon" viewBox="0 0 512 512">
                    <path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path>
                    </svg></button></td>
                    <td><button type="button" class="delete-button" onclick="ValidarEliminacion(${clientes.clienteID})"><svg class="delete-svgIcon" viewBox="0 0 448 512">
                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
                    </svg></button></td>
                </tr>
                `;
            });
            document.getElementById("tbody-clientes").innerHTML = tabla;                                
        },
        error: function(xhr, status){
            console.log('Problemas al cargar la tabla');
        }
    });
}

function LimpiarModal(){
    document.getElementById("ClienteID").value = 0;
    document.getElementById("Nombre").value = "";
    document.getElementById("Apellido").value = "";
    document.getElementById("Email").value = "";
    document.getElementById("Telefono").value = "";
    document.getElementById("errorMensajeNombre").style.display = "none"
    document.getElementById("errorMensajeApellido").style.display = "none"
    document.getElementById("errorMensajeEmail").style.display = "none"
    document.getElementById("errorMensajeTelefono").style.display = "none"
}

function NuevoCliente(){
    $("#tituloModal").text("Nuevo Cliente");
}

function GuardarCliente() {
    let clienteID = document.getElementById("ClienteID").value;
    let nombre = document.getElementById("Nombre").value;
    let apellido = document.getElementById("Apellido").value;
    let email = document.getElementById("Email").value;
    let telefono = document.getElementById("Telefono").value;
    let isValid = true;

    // Validaciones
    if (nombre === "") {
        document.getElementById("errorMensajeNombre").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeNombre").style.display = "none";
    }

    if (apellido === "") {
        document.getElementById("errorMensajeApellido").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeApellido").style.display = "none";
    }

    if (telefono === "") {
        document.getElementById("errorMensajeTelefono").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeTelefono").style.display = "none";
    }

    if (email === "") {
        document.getElementById("errorMensajeEmail").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeEmail").style.display = "none";
    }

    if (!isValid) {
        return;  // Detener la ejecución si isValid es false
    }

    // Envío de datos por AJAX
    $.ajax({
        url: '../../Clientes/GuardarCliente',
        data: {
            clienteID: clienteID,
            nombre: nombre,
            apellido: apellido,
            email: email,
            telefono: telefono
        },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            if (resultado === "El cliente ya existe.") {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: resultado,
                });
            } else {
                Swal.fire({
                    icon: 'success',
                    title: '¡Cliente guardado!',
                    text: 'Los datos del cliente se han guardado correctamente.',
                    showConfirmButton: false,
                    timer: 1500
                });
                ListadoClientes();
                $("#ModalCliente").modal("hide");
            }
        },
        error: function (xhr, status) {
            console.log('Problemas al guardar Cliente');
        },
    });
}

function ModalEditar(ClienteID) {
    $.ajax({
        url: '../../Clientes/ListadoClientes',
        data: { clienteID: ClienteID },
        type: 'POST',
        dataType: 'json',
        success: function (listadoClientes) {
            let listadoCliente = listadoClientes[0];

            document.getElementById("ClienteID").value = ClienteID;
            $("#tituloModal").text("Editar Cliente");
            document.getElementById("Nombre").value = listadoCliente.nombre;
            document.getElementById("Apellido").value = listadoCliente.apellido;
            document.getElementById("Email").value = listadoCliente.email;
            document.getElementById("Telefono").value = listadoCliente.telefono;
            $("#ModalCliente").modal("show");
        },
        error: function (xhr, status) {
            console.log('Problemas al cargar Cliente');
        }
    });
}


function ValidarEliminacion(ClienteID) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "No podrás revertir esta acción",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            EliminarCliente(ClienteID);
        }
    });
}

function EliminarCliente(ClienteID) {
    $.ajax({
        url: '../../Clientes/EliminarCliente',
        data: { clienteID: ClienteID },
        type: 'POST',
        dataType: 'json',
        success: function(EliminarCliente) {
            Swal.fire({
                icon: 'success',
                title: '¡Eliminado!',
                text: 'El cliente ha sido eliminado correctamente.',
                showConfirmButton: false,
                timer: 1500
            });
            ListadoClientes();
        },
        error: function(xhr, status) {
            console.log('Problemas al eliminar Cliente');
        }
    });
}