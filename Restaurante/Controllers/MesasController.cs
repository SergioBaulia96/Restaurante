
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

    public JsonResult EliminarMesa(int MesaID)
{
    var mesa = _context.Mesas.Find(MesaID);
    if (mesa == null)
    {
        return Json("Mesa no encontrada.");
    }

    // Verificar si la mesa está habilitada
    if (mesa.Disponible)
    {
        return Json("No se puede eliminar la mesa porque está habilitada.");
    }

    // Si pasa las validaciones, eliminar la mesa
    _context.Mesas.Remove(mesa);
    _context.SaveChanges();

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