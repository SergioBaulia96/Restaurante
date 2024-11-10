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
        reservacion.FechaReservacion = DateTime.Now; // Agrega la fecha actual o la seleccionada en la vista
        _context.Reservaciones.Add(reservacion);
        _context.SaveChanges();
        return Ok();
    }
    return BadRequest("Datos inválidos");
}




}