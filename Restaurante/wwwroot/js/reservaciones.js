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

function VerHorariosDisponibles(fechaSeleccionada) {
    $.ajax({
      url: '/Reservaciones/ObtenerHorariosDisponibles', // Controlador que obtendrá los horarios
      type: 'POST',
      data: { fecha: fechaSeleccionada },
      success: function(horarios) {
        // Mostrar los horarios en un div o modal
        var horariosHTML = '<h3>Horarios disponibles para ' + fechaSeleccionada + ':</h3>';
        horarios.forEach(function(horario) {
          horariosHTML += '<button onclick="Reservar(\'' + fechaSeleccionada + '\', \'' + horario + '\')">' + horario + '</button>';
        });
        document.getElementById('horariosDisponibles').innerHTML = horariosHTML;
      },
      error: function(error) {
        console.error('Error al obtener los horarios:', error);
      }
    });
  }

function Reservar(fecha, horario) {
  $.ajax({
    url: '/Reservaciones/CrearReservacion',
    type: 'POST',
    data: {
      ClienteID: 1, // Puedes obtener este valor desde el usuario logueado o un campo
      MesaID: 2,    // Puedes tener una lógica para asignar la mesa
      FechaReservacion: `${fecha} ${horario}`, // Combinas la fecha y el horario
      Notas: 'Preferencia de ubicación cerca de la ventana'
    },
    success: function(response) {
      alert('¡Reservación realizada con éxito!');
      // Aquí puedes actualizar la vista o mostrar un mensaje de éxito
    },
    error: function(error) {
      console.error('Error al realizar la reservación:', error);
    }
  });
}

function redirigirHorarios(fecha) {
    // Redirigir a la vista de horarios disponibles
    window.location.href = `/Reservaciones/HorariosDisponibles?fecha=${fecha}`;
}