using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

[Authorize]
public class MeserosController : Controller
{
    private ApplicationDbContext _context;

    public MeserosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoMeseros(int? MeseroID)
    {
        var listadoMeseros = _context.Meseros.ToList();
            listadoMeseros = _context.Meseros.OrderBy(l => l.Nombre).ToList();

        if(MeseroID != null)
        {
            listadoMeseros = _context.Meseros.Where(l => l.MeseroID == MeseroID).ToList();
        }
        return Json(listadoMeseros);
    }

        public JsonResult GuardarMesero (int MeseroID, string Nombre, string Apellido, string Telefono)
    {
        string resultado = "";

        Nombre = Nombre.ToUpper();
        Apellido = Apellido.ToUpper();

        var meseroExistente = _context.Meseros
            .Where(m => m.Nombre == Nombre && m.Apellido == Apellido && m.MeseroID != MeseroID) // Excluir el mesero actual si está en modo de edición
            .SingleOrDefault();

        if (meseroExistente != null)
        {
            // Si ya existe un mesero con ese nombre, devolver un mensaje de error
            return Json("El mesero ya existe.");
        }

        if(MeseroID == 0)
        {
            var nuevoMesero = new Mesero
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Telefono = Telefono
            };
            _context.Add(nuevoMesero);
            _context.SaveChanges();
            resultado = "Mesero Guardado";
        }
        else
        {
            var editarMesero = _context.Meseros.Where(e => e.MeseroID == MeseroID).SingleOrDefault();
            
            if(editarMesero != null)
            {
                editarMesero.Nombre = Nombre;
                editarMesero.Apellido = Apellido;
                editarMesero.Telefono = Telefono;
                _context.SaveChanges();
                resultado = "Mesero Editado";
            }
        }
        return Json(resultado);
    }

    public JsonResult EliminarMesero(int MeseroID)
    {
        var eliminarMesero = _context.Meseros.Find(MeseroID);
        _context.Remove(eliminarMesero);
        _context.SaveChanges();

        return Json(eliminarMesero);
    }
}