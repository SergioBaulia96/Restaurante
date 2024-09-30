using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

[Authorize]

public class ClientesController : Controller
{
    private ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoClientes(int? ClienteID)
    {
        var listadoClientes = _context.Clientes.ToList();
            listadoClientes = _context.Clientes.OrderBy(l => l.Nombre).ToList();

        if(ClienteID != null)
        {
            listadoClientes = _context.Clientes.Where(l => l.ClienteID == ClienteID).ToList();
        }
        return Json(listadoClientes);
    }

        public JsonResult GuardarCliente (int ClienteID, string Nombre, string Apellido, string Email, string Telefono)
    {
        string resultado = "";

        Nombre = Nombre.ToUpper();
        Apellido = Apellido.ToUpper();

        if(ClienteID == 0)
        {
            var nuevoCliente = new Cliente
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Email = Email,
                Telefono = Telefono
            };
            _context.Add(nuevoCliente);
            _context.SaveChanges();
            resultado = "Cliente Guardado";
        }
        else
        {
            var editarCliente = _context.Clientes.Where(e => e.ClienteID == ClienteID).SingleOrDefault();
            
            if(editarCliente != null)
            {
                editarCliente.Nombre = Nombre;
                editarCliente.Apellido = Apellido;
                editarCliente.Email = Email;
                editarCliente.Telefono = Telefono;
                _context.SaveChanges();
                resultado = "Club Editado";
            }
        }
        return Json(resultado);
    }

    public JsonResult EliminarCliente(int ClienteID)
    {
        var eliminarCliente = _context.Clientes.Find(ClienteID);
        _context.Remove(eliminarCliente);
        _context.SaveChanges();

        return Json(eliminarCliente);
    }
}