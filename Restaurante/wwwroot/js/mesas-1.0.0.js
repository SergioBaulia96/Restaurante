window.onload = ListadoMesas();

function ListadoMesas()
{
    $.ajax({
        url: '../../Mesas/ListadoMesas',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function(listadoMesas){
            $("#ModalMesa").modal("hide");
            LimpiarModal();
            
            let tabla = ``

            $.each(listadoMesas, function(index, mesas){
                let estadoMesa = mesas.disponible ? "Disponible" : "Reservada";
                let botonEstado = mesas.disponible ? 
                    `<button type="button" class="btn btn-danger" onclick="CambiarEstadoMesa(${mesas.mesaID}, false)">Deshabilitar</button>` :
                    `<button type="button" class="btn btn-success" onclick="CambiarEstadoMesa(${mesas.mesaID}, true)">Habilitar</button>`;

                tabla += `
                <tr>
                    <td>${mesas.numero}</td>
                    <td>${mesas.capacidad}</td>
                    <td>${botonEstado}</td>
                    <td><button type="button" class="edit-button" onclick="ModalEditar(${mesas.mesaID})"><svg class="edit-svgIcon" viewBox="0 0 512 512">
                    <path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path>
                    </svg></button></td>
                    <td><button type="button" class="delete-button" onclick="ValidarEliminacion(${mesas.mesaID})"><svg class="delete-svgIcon" viewBox="0 0 448 512">
                    <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
                    </svg></button></td>
                </tr>
                `;
            });
            document.getElementById("tbody-mesas").innerHTML = tabla;                                
        },
        error: function(xhr, status){
            console.log('Problemas al cargar la tabla');
        }
    });
}

function CambiarEstadoMesa(MesaID, Disponible) {
    $.ajax({
        url: '../../Mesas/CambiarEstadoMesa',
        data: { mesaID: MesaID, disponible: Disponible },
        type: 'POST',
        dataType: 'json',
        success: function(resultado) {
            Swal.fire({
                icon: 'success',
                title: 'Estado Actualizado',
                text: resultado,
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                ListadoMesas();
            });
        },
        error: function(xhr, status) {
            console.log('Problemas al cambiar el estado de la mesa');
        }
    });
}

function LimpiarModal(){
    document.getElementById("MesaID").value = 0;
    document.getElementById("Numero").value = "";
    document.getElementById("Capacidad").value = "";
    document.getElementById("errorMensajeNombre").style.display = "none"
    document.getElementById("errorMensajeCapacidad").style.display = "none"
}

function NuevaMesa(){
    $("#tituloModal").text("Nueva Mesa");
}

function GuardarMesa() {
    let mesaID = document.getElementById("MesaID").value;
    let numero = document.getElementById("Numero").value;
    let capacidad = document.getElementById("Capacidad").value;
    let isValid = true;

    // Validación del campo "Número"
    if (numero === "") {
        document.getElementById("errorMensajeNombre").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeNombre").style.display = "none";
    }

    // Validación del campo "Capacidad"
    if (capacidad === "") {
        document.getElementById("errorMensajeCapacidad").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeCapacidad").style.display = "none";
    }

    if (!isValid) {
        return;  // Detener la ejecución aquí si isValid es false
    }

    // Enviar la solicitud AJAX
    $.ajax({
        url: '../../Mesas/GuardarMesa',
        data: {
            mesaID: mesaID,
            numero: numero,
            capacidad: capacidad,
        },
        type: 'POST',
        dataType: 'json',
        success: function(resultado) {
            // Si ya existe una mesa con el mismo número, mostrar una alerta de error
            if (resultado === "La mesa ya existe.") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: resultado
                });
            } else {
                // Mostrar una alerta de éxito con SweetAlert2
                Swal.fire({
                    icon: 'success',
                    title: 'Mesa Guardada',
                    text: resultado,
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    // Refrescar la tabla de mesas
                    ListadoMesas();
                    // Cerrar el modal
                    $("#ModalMesa").modal("hide");
                });
            }
        },
        error: function(xhr, status) {
            console.log('Problemas al guardar Mesa');
        }
    });
}


function ModalEditar(MesaID){
    $.ajax({
        url: '../../Mesas/ListadoMesas',
        data: { mesaID : MesaID },
        type: 'POST',
        dataType: 'json',
        success: function(listadoMesas){
            let listadoMesa = listadoMesas[0];
            
            document.getElementById("MesaID").value = MesaID
            $("#tituloModal").text("Editar Mesa");
            document.getElementById("Numero").value = listadoMesa.numero;
            document.getElementById("Capacidad").value = listadoMesa.capacidad;
            $("#ModalMesa").modal("show");
        },
        error: function(xhr, status){
            console.log('Problemas al cargar Mesa');
        }
    });
}

function ValidarEliminacion(MesaID)
{
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
            EliminarMesa(MesaID);
        }
    });
}

function EliminarMesa(MesaID){
    $.ajax({
        url: '../../Mesas/EliminarMesa',
        data: { mesaID: MesaID },
        type: 'POST',
        dataType: 'json',
        success: function(EliminarMesa){
            Swal.fire({
                icon: 'success',
                title: '¡Eliminado!',
                text: 'La mesa ha sido eliminado correctamente.',
                showConfirmButton: false,
                timer: 1500
            });
            ListadoMesas();
        },
        error: function(xhr, status) {
            console.log('Problemas al eliminar Menu');
        }
    });
}