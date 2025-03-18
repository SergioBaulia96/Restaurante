using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    {var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };
        ViewBag.ClienteID = selectListItems.OrderBy(t => t.Text).ToList();

        var clientes = _context.Clientes.ToList();

        clientes.Add(new Cliente { ClienteID = 0, Apellido = "*CLIENTES*"});

        ViewBag.buscarApellido = new SelectList(clientes.OrderBy(c => c.NombreCompleto), "ClienteID", "NombreCompleto");
        return View();
    }

    public JsonResult ListadoClientes(int? ClienteID, int? buscarApellido)
    {
        var listadoClientes = _context.Clientes.ToList();
            listadoClientes = _context.Clientes.OrderBy(l => l.Nombre).ToList();

        if (buscarApellido != 0)
        {
            listadoClientes = listadoClientes.Where(t => t.ClienteID == buscarApellido).ToList();
        }

        if(ClienteID != null)
        {
            listadoClientes = _context.Clientes.Where(l => l.ClienteID == ClienteID).ToList();
        }
        return Json(listadoClientes);
    }

        public JsonResult GuardarCliente (int ClienteID, string Nombre, string Apellido, string Email, string Telefono, bool Activo)
    {
        string resultado = "";

        Nombre = Nombre.ToUpper();
        Apellido = Apellido.ToUpper();

        var clienteExistente = _context.Clientes
            .Where(c => c.Nombre == Nombre && c.Apellido == Apellido && c.ClienteID != ClienteID) // Excluir el cliente actual si está en modo de edición
            .SingleOrDefault();

        if (clienteExistente != null)
        {
            // Si ya existe un cliente con ese nombre, devolver un mensaje de error
            return Json("El cliente ya existe.");
        }

        if(ClienteID == 0)
        {
            var nuevoCliente = new Cliente
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Email = Email,
                Telefono = Telefono,
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

    public JsonResult CambiarEstadoCliente(int ClienteID, bool Activo)
{
    var cliente = _context.Clientes.Find(ClienteID);
    if (cliente != null)
    {
        cliente.Activo = Activo;
        _context.SaveChanges();
        return Json("Estado del cliente actualizado.");
    }
    return Json("Cliente no encontrado.");
}
}