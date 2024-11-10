document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth', // Vista inicial: Mes
        locale: 'es',
        selectable: true, // Permitir selección de fechas
        dateClick: function(info) { // Evento que se dispara al hacer clic en un día
            var fechaSeleccionada = info.dateStr; // Formato 'YYYY-MM-DD'

            // Redirige a la vista de horarios disponibles, pasando la fecha como parámetro
            window.location.href = `/Reservaciones/HorariosDisponibles?fecha=${fechaSeleccionada}`;
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
document.getElementById('reservationForm').addEventListener('submit', function(event) {
  event.preventDefault();
  
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
      body: JSON.stringify({ MesaID: mesaID, ClienteID: clienteID })
  })
  .then(response => {
      if (response.ok) {
          alert('Reservación realizada con éxito');
          location.reload(); // Recargar la página para ver los cambios
      } else {
          alert('Error al realizar la reservación');
      }
  })
  .catch(error => console.error('Error:', error));
});

