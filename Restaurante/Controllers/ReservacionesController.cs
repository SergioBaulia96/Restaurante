using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };
        ViewBag.ClienteID = selectListItems.OrderBy(t => t.Text).ToList();
        var clientes = _context.Clientes.ToList();
        clientes.Add(new Cliente { ClienteID = 0, Apellido = "[CLIENTE...]"});
        ViewBag.ClienteID = new SelectList(clientes.OrderBy(c => c.NombreCompleto), "ClienteID", "NombreCompleto");

    var mesas = _context.Mesas
        .ToList();

    var reservaciones = _context.Reservaciones.Include(r => r.Clientes).Include(m => m.Mesas).Where(r => r.FechaReservacion.Date == fecha.Date).ToList();

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
        Mesas = mesas,
        Reservaciones = reservaciones,
        HorariosDisponibles = horarios,
        FechaSeleccionada = fecha
    };

    return View(viewModel);
}



[HttpPost]
public IActionResult Create([FromBody] Reservacion reservacion)
{
    if (ModelState.IsValid)
    {
        // Verificar si el cliente ya tiene una reservación en la misma fecha
        bool existeReservacion = _context.Reservaciones
            .Any(r => r.ClienteID == reservacion.ClienteID && r.FechaReservacion.Date == reservacion.FechaReservacion.Date);

        if (existeReservacion)
        {
            return BadRequest("El cliente ya tiene una reservación en esta fecha.");
        }

        bool mesaReservada = _context.Reservaciones
        .Any(r => r.MesaID == reservacion.MesaID && r.FechaReservacion.Date == reservacion.FechaReservacion.Date);

        if (mesaReservada)
        {
            return BadRequest("La mesa ya está reservada para esta fecha.");
        }

        
        _context.Reservaciones.Add(reservacion);
        _context.SaveChanges();
        return Ok();
    }
    return BadRequest("Datos inválidos");
}

[HttpDelete]
public JsonResult CancelarReservacion(int reservacionID)
{
    Console.WriteLine($"ReservacionID recibido: {reservacionID}"); // Depuración

    // Validación del ID
    if (reservacionID <= 0)
    {
        return Json(new { success = false, message = "ID de reservación no válido." });
    }

    try
    {
        var cancelarReservacion = _context.Reservaciones.Find(reservacionID);
        if (cancelarReservacion != null)
        {
            _context.Reservaciones.Remove(cancelarReservacion);
            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                return Json(new { success = true, message = "Reservación cancelada correctamente." });
            }
            else
            {
                return Json(new { success = false, message = "No se pudo eliminar la reservación." });
            }
        }
        return Json(new { success = false, message = "No se encontró la reservación." });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al cancelar la reservación: {ex.Message}");
        return Json(new { success = false, message = "Ocurrió un error al cancelar la reservación." });
    }
}

}