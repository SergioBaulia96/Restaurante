using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

public class ReservacionesController : Controller
{
    private ApplicationDbContext _context;

    public ReservacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        return View();
    }

    public IActionResult HorariosDisponibles(DateTime fecha)
{
    // Obtener mesas disponibles para la fecha seleccionada
    var mesasDisponibles = _context.Mesas
        .Where(m => !m.Reservaciones.Any(r => r.FechaReservacion.Date == fecha.Date))
        .ToList();

    // Definir horarios disponibles (puedes ajustar estos valores según tu lógica)
    List<string> horarios = new List<string> 
    {
        "12:00 PM - 2:00 PM", 
        "2:00 PM - 4:00 PM", 
        "4:00 PM - 6:00 PM", 
        "6:00 PM - 8:00 PM"
    };

    // Crear el ViewModel y pasar los datos
    var viewModel = new VistaReservacion
    {
        Mesas = mesasDisponibles,
        HorariosDisponibles = horarios,
        FechaSeleccionada = fecha
    };

    return View(viewModel);
}

public IActionResult ConfirmarReservacion(DateTime fecha, string horario)
{
    // Aquí puedes manejar la confirmación de la reservación con la fecha y el horario seleccionados
    ViewBag.Fecha = fecha;
    ViewBag.Horario = horario;

    return View();
}




    [HttpPost]
    public JsonResult ObtenerHorariosDisponibles(DateTime fecha)
    {
        // Aquí definimos los horarios disponibles por defecto
        var horariosDisponibles = new List<string> {
            "12:00 PM", "1:00 PM", "2:00 PM", "6:00 PM", "7:00 PM", "8:00 PM"
        };

        // Filtramos las reservaciones existentes en la fecha seleccionada
        var reservacionesExistentes = _context.Reservaciones
            .Where(r => r.FechaReservacion.Date == fecha.Date)
            .Select(r => r.FechaReservacion.TimeOfDay) // Extraemos solo la hora
            .ToList();

        // Filtramos los horarios disponibles que no estén ya reservados
        var horariosFiltrados = horariosDisponibles
            .Where(h => !reservacionesExistentes
                .Any(r => TimeSpan.Parse(h) == r)) // Comparamos horas
            .ToList();

        // Devolvemos los horarios disponibles como JSON
        return Json(horariosFiltrados);
    }

    [HttpPost]
public JsonResult CrearReservacion(Reservacion nuevaReservacion)
{
    if (ModelState.IsValid)
    {
        _context.Reservaciones.Add(nuevaReservacion);
        _context.SaveChanges();
        
        return Json(new { success = true, message = "Reservación creada con éxito." });
    }

    return Json(new { success = false, message = "Error al crear la reservación." });
}


}