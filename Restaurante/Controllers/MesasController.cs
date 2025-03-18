
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

[Authorize]
public class MesasController : Controller
{
    private ApplicationDbContext _context;

    public MesasController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoMesas(int? MesaID)
    {
        var listadoMesas = _context.Mesas.ToList();
            listadoMesas = _context.Mesas.OrderBy(l => l.Numero).ToList();

        if(MesaID != null)
        {
            listadoMesas = _context.Mesas.Where(l => l.MesaID == MesaID).ToList();
        }
        return Json(listadoMesas);
    }

        public JsonResult GuardarMesa(int MesaID, string Numero, int Capacidad, bool Disponible)
{
    string resultado = "";

    Numero = Numero.ToUpper();

    // Verificar si ya existe una mesa con el mismo nombre
    var mesaExistente = _context.Mesas
        .Where(m => m.Numero == Numero && m.MesaID != MesaID)
        .SingleOrDefault();

    if (mesaExistente != null)
    {
        return Json("La mesa ya existe.");
    }

    if (MesaID == 0)
    {
        // Crear una nueva mesa
        var nuevaMesa = new Mesa
        {
            Numero = Numero,
            Capacidad = Capacidad,
            Disponible = Disponible
        };
        _context.Add(nuevaMesa);
        _context.SaveChanges();
        resultado = "Mesa Guardada";
    }
    else
    {
        // Editar una mesa existente
        var editarMesa = _context.Mesas.Where(e => e.MesaID == MesaID).SingleOrDefault();

        if (editarMesa != null)
        {
            // Verificar si la mesa está habilitada
            if (editarMesa.Disponible)
            {
                return Json("No se puede editar la mesa porque está habilitada.");
            }

            // Actualizar los datos de la mesa
            editarMesa.Numero = Numero;
            editarMesa.Capacidad = Capacidad;
            editarMesa.Disponible = Disponible;
            _context.SaveChanges();
            resultado = "Mesa Editada";
        }
    }
    return Json(resultado);
}

    [HttpPost]
public IActionResult VerificarMesaAsociada(int mesaID)
{
    // Verificar si existe alguna reserva asociada a la mesa
    bool tieneReservas = _context.Reservaciones.Any(r => r.MesaID == mesaID);

    // Verificar si existe algún pedido asociado a la mesa
    bool tienePedidos = _context.Pedidos.Any(p => p.MesaID == mesaID);

    if (tieneReservas || tienePedidos)
    {
        // Si la mesa está asociada a una reserva o a un pedido, devolver un objeto JSON con estado "asociada"
        return Json(new { estado = "asociada" });
    }
    else
    {
        // Si la mesa no está asociada a ninguna reserva ni a ningún pedido, devolver un objeto JSON con estado "no_asociada"
        return Json(new { estado = "no_asociada" });
    }
}

    // Método para eliminar una mesa
    [HttpPost]
public IActionResult EliminarMesa(int mesaID)
{
    // Verificar si existe alguna reserva asociada a la mesa
    bool tieneReservas = _context.Reservaciones.Any(r => r.MesaID == mesaID);

    // Verificar si existe algún pedido asociado a la mesa
    bool tienePedidos = _context.Pedidos.Any(p => p.MesaID == mesaID);

    if (tieneReservas || tienePedidos)
    {
        // Si la mesa está asociada a una reserva o a un pedido, devolver un mensaje de error
        return Json("No se puede eliminar la mesa porque está asociada a una reserva o a un pedido.");
    }

    // Buscar la mesa en la base de datos
    var mesa = _context.Mesas.FirstOrDefault(m => m.MesaID == mesaID);

    if (mesa == null)
    {
        // Si la mesa no existe, devolver un mensaje de error
        return Json("La mesa no existe.");
    }

    // Eliminar la mesa
    _context.Mesas.Remove(mesa);
    _context.SaveChanges();

    // Devolver un mensaje de éxito
    return Json("Mesa eliminada correctamente.");
}


    public JsonResult CambiarEstadoMesa(int MesaID, bool Disponible)
{
    var mesa = _context.Mesas.Find(MesaID);
    if (mesa != null)
    {
        mesa.Disponible = Disponible;
        _context.SaveChanges();
        return Json("Estado de la mesa actualizado.");
    }
    return Json("Mesa no encontrada.");
}
}