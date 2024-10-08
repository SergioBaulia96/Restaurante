window.onload = ListadoPlatos();

function ListadoPlatos()
{
    $.ajax({
        url: '../../Menus/ListadoPlatos',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function(listadoPlatos){
            $("#ModalPlato").modal("hide");
            LimpiarModal();
            
            let tabla = ``

            $.each(listadoPlatos, function(index, platos){
                let estadoPlatos = platos.disponible ? "Disponible" : "Sin Stock";
                let precioFormateado = `$ ${parseFloat(platos.precio).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;

    tabla += `
    <tr>
        <td style="text-align: center;">${platos.nombreMenu}</td>
        <td style="text-align: center;">${platos.nombrePlato}</td>
        <td style="text-align: center;">${platos.descripcion}</td>
        <td style="text-align: center;">${precioFormateado}</td>
        <td style="text-align: center;">${estadoPlatos}</td>
        <td><button type="button" class="edit-button" onclick="ModalEditar(${platos.platoID})"><svg class="edit-svgIcon" viewBox="0 0 512 512">
        <path d="M410.3 231l11.3-11.3-33.9-33.9-62.1-62.1L291.7 89.8l-11.3 11.3-22.6 22.6L58.6 322.9c-10.4 10.4-18 23.3-22.2 37.4L1 480.7c-2.5 8.4-.2 17.5 6.1 23.7s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L387.7 253.7 410.3 231zM160 399.4l-9.1 22.7c-4 3.1-8.5 5.4-13.3 6.9L59.4 452l23-78.1c1.4-4.9 3.8-9.4 6.9-13.3l22.7-9.1v32c0 8.8 7.2 16 16 16h32zM362.7 18.7L348.3 33.2 325.7 55.8 314.3 67.1l33.9 33.9 62.1 62.1 33.9 33.9 11.3-11.3 22.6-22.6 14.5-14.5c25-25 25-65.5 0-90.5L453.3 18.7c-25-25-65.5-25-90.5 0zm-47.4 168l-144 144c-6.2 6.2-16.4 6.2-22.6 0s-6.2-16.4 0-22.6l144-144c6.2-6.2 16.4-6.2 22.6 0s6.2 16.4 0 22.6z"></path>
        </svg></button></td>
        <td><button type="button" class="delete-button" onclick="ValidarEliminacion(${platos.platoID})"><svg class="delete-svgIcon" viewBox="0 0 448 512">
        <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"></path>
        </svg></button></td>
    </tr>
    `;
            });
            document.getElementById("tbody-platos").innerHTML = tabla;                                
        },
        error: function(xhr, status){
            console.log('Problemas al cargar la tabla');
        }
    });
}

function LimpiarModal(){
    document.getElementById("MenuID").value = 0;
    document.getElementById("PlatoID").value = 0;
    document.getElementById("NombrePlato").value = "";
    document.getElementById("Descripcion").value = "";
    document.getElementById("Precio").value = "";
    document.getElementById("Disponible").value = "";
    document.getElementById("errorMensajeTipoMenu").style.display = "none"
    document.getElementById("errorMensajeNombre").style.display = "none"
    document.getElementById("errorMensajeDescripcion").style.display = "none"
    document.getElementById("errorMensajePrecio").style.display = "none"
}

function NuevoPlato(){
    $("#tituloModal").text("Nuevo Plato");
}

function GuardarPlato(){
    let menuID = document.getElementById("MenuID").value;
    let platoID = document.getElementById("PlatoID").value;
    let nombrePlato = document.getElementById("NombrePlato").value;
    let descripcion = document.getElementById("Descripcion").value;
    let precio = document.getElementById("Precio").value;
    let disponible = document.getElementById("Disponible").checked;
    let isValid = true;

    if (menuID === "0") {
        document.getElementById("errorMensajeTipoMenu").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeTipoMenu").style.display = "none";
    }

    if (nombrePlato === "") {
        document.getElementById("errorMensajeNombre").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeNombre").style.display = "none";
    }

    if (descripcion === "") {
        document.getElementById("errorMensajeDescripcion").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajeDescripcion").style.display = "none";
    }

    if (precio === "") {
        document.getElementById("errorMensajePrecio").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("errorMensajePrecio").style.display = "none";
    }

    if (!isValid) {
        return;  // Detener la ejecución aquí si isValid es false
    }
    


    $.ajax({
        url: '../../Menus/GuardarPlato',
        data: {
            menuID : menuID,
            platoID : platoID,
            nombrePlato : nombrePlato,
            descripcion : descripcion,
            precio : precio,
            disponible : disponible
        },
        type: 'POST',
        dataType: 'json',
        success: function(resultado){
            if(resultado != "") {
                alert(resultado)
            }
            ListadoPlatos();
            
        },
        error: function(xhr, status){
            console.log('Problemas al guardar Plato');
        },
    });
}

function ModalEditar(PlatoID){
    $.ajax({
        url: '../../Menus/TraerPlatosAlModal',
        data: { platoID : PlatoID },
        type: 'POST',
        dataType: 'json',
        success: function(listadoPlatos){
            let listadoPlato = listadoPlatos[0];
            
            document.getElementById("PlatoID").value = PlatoID
            $("#tituloModal").text("Editar Plato");
            document.getElementById("MenuID").value = listadoPlato.menuID;
            document.getElementById("NombrePlato").value = listadoPlato.nombrePlato;
            document.getElementById("Descripcion").value = listadoPlato.descripcion;
            document.getElementById("Precio").value = listadoPlato.precio;
            document.getElementById("Disponible").value = listadoPlato.disponible;
            $("#ModalPlato").modal("show");
        },
        error: function(xhr, status){
            console.log('Problemas al cargar Plato');
        }
    });
}

function ValidarEliminacion(PlatoID)
{
    var elimina = confirm("¿Esta seguro que desea eliminar?");
    if(elimina == true)
        {
            EliminarPlato(PlatoID);
        }
}

function EliminarPlato(PlatoID){
    $.ajax({
        url: '../../Menus/EliminarPlato',
        data: { platoID: PlatoID },
        type: 'POST',
        dataType: 'json',
        success: function(EliminarPlato){
            ListadoPlatos()
        },
        error: function(xhr, status){
            console.log('Problemas al eliminar Plato');
        }
    });
}