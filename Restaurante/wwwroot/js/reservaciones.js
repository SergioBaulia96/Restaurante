document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var today = new Date().toISOString().split('T')[0]; // Fecha actual en formato YYYY-MM-DD

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth', // Vista inicial: Mes
        locale: 'es',
        selectable: true, // Permitir selección de fechas
        dateClick: function (info) { // Evento que se dispara al hacer clic en un día
            if (info.dateStr < today) {
                return; // No hacer nada si es una fecha pasada
            }
            var fechaSeleccionada = info.dateStr; // Formato 'YYYY-MM-DD'

            // Redirige a la vista de horarios disponibles, pasando la fecha como parámetro
            window.location.href = `/Reservaciones/HorariosDisponibles?fecha=${fechaSeleccionada}`;
        },
        dateClassNames: function (arg) {
            // Asignar clase "disabled-date" a las fechas pasadas
            if (arg.date < new Date()) {
                return ['disabled-date'];
            }
            return [];
        }
    });

    calendar.render();
});

// Función para abrir el modal y pasar los datos de la mesa seleccionada
function openReservationModal(mesaID, mesaNumero) {
    console.log(`Abriendo modal para Mesa ID: ${mesaID}, Número: ${mesaNumero}`);
    document.getElementById('mesaID').value = mesaID;
    document.getElementById('mesaNumero').textContent = mesaNumero;

    const reservationModal = new bootstrap.Modal(document.getElementById('reservationModal'));
    reservationModal.show();
}

// Manejar el envío del formulario de reservación
document.getElementById('reservationForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const params = new URLSearchParams(window.location.search);
    const fechaSeleccionada = params.get('fecha'); // Obtener la fecha desde la URL

    const mesaID = document.getElementById('mesaID').value;
    const clienteID = document.getElementById('ClienteID').value;

    if (clienteID == "0") {
        document.getElementById('errorMensajeCliente').style.display = 'block';
        return; // Detener el envío si no hay cliente seleccionado
    }

    // Ocultar el mensaje de error si todo es válido
    document.getElementById('errorMensajeCliente').style.display = 'none';

    fetch('/Reservaciones/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ MesaID: mesaID, ClienteID: clienteID, FechaReservacion: fechaSeleccionada })
    })
        .then(response => {
            if (response.ok) {
                Swal.fire({
                    icon: 'success',
                    title: 'Reservación realizada con éxito',
                    text: `Reservación para el ${fechaSeleccionada} creada.`,
                }).then(() => location.reload());
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se pudo realizar la reservación. Inténtelo nuevamente.',
                });
            }
        })
        .catch(error => console.error('Error:', error));
});

function cancelarReservacion(mesaID) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "Esta acción cancelará la reservación de la mesa seleccionada.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sí, cancelar",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Reservaciones/CancelarReservacion`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ mesaID: mesaID })
            })
            .then(response => {
                console.log("Status:", response.status);  // Verifica el código de estado HTTP
                if (response.ok) {
                    Swal.fire(
                        "Reservación Cancelada",
                        "La reservación ha sido eliminada correctamente.",
                        "success"
                    ).then(() => location.reload());
                } else {
                    return response.text().then((text) => {
                        Swal.fire(
                            "Error",
                            `No se pudo cancelar la reservación. Error: ${text}`,
                            "error"
                        );
                    });
                }
            })
            .catch(error => {
                Swal.fire(
                    "Error",
                    "Ocurrió un error inesperado.",
                    "error"
                );
                console.error(error);
            });
            
        }
    });
}



