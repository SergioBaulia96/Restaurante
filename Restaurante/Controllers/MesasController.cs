
using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

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

        public JsonResult GuardarMesa (int MesaID, int Numero, int Capacidad, bool Disponible)
    {
        string resultado = "";

        if(MesaID == 0)
        {
            var nuevaMesa = new Mesa
            {
                Numero = Numero,
                Capacidad = Capacidad,
                Disponible = Disponible
                
            };
            _context.Add(nuevaMesa);
            _context.SaveChanges();
            resultado = "Mesa Guardado";
        }
        else
        {
            var editarMesa = _context.Mesas.Where(e => e.MesaID == MesaID).SingleOrDefault();
            
            if(editarMesa != null)
            {
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
        var eliminarMesa = _context.Meseros.Find(MesaID);
        _context.Remove(eliminarMesa);
        _context.SaveChanges();

        return Json(eliminarMesa);
    }
}