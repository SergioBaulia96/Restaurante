using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

[Authorize]
public class PedidosController : Controller
{
    private ApplicationDbContext _context;

    public PedidosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };

        var enumValues = Enum.GetValues(typeof(Estado)).Cast<Estado>();
        
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));
        ViewBag.Estado = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.ClienteID = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.MeseroID = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.MesaID = selectListItems.OrderBy(t => t.Text).ToList();

        var clientes = _context.Clientes.ToList();
        var meseros = _context.Meseros.ToList();
        var mesas = _context.Mesas.ToList();

        clientes.Add(new Cliente { ClienteID = 0, Apellido = "[CLIENTE...]"});
        ViewBag.ClienteID = new SelectList(clientes.OrderBy(c => c.NombreCompleto), "ClienteID", "NombreCompleto");

        meseros.Add(new Mesero { MeseroID = 0, Apellido = "[MESERO...]" });
        ViewBag.MeseroID = new SelectList(meseros.OrderBy(c => c.NombreCompleto), "MeseroID", "NombreCompleto");

        mesas.Add(new Mesa { MesaID = 0, Numero = "[MESA...]" });
        ViewBag.MesaID = new SelectList(mesas.OrderBy(c => c.Numero), "MesaID", "Numero");

        return View();
    }

    public JsonResult ListadoPedidos(int? id)
    {
        List<VistaPedido> pedidosMostrar = new List<VistaPedido>();

        var pedidos = _context.Pedidos.AsQueryable();

        if (id != null)
        {
            pedidos = pedidos.Where(t => t.PedidoID == id);
        }

        var clientes = _context.Clientes.ToList();
        var meseros = _context.Meseros.ToList();
        var mesas = _context.Mesas.ToList();

        foreach (var p in pedidos)
        {
            var cliente = clientes.Where(t => t.ClienteID == p.ClienteID).Single();
            var mesero = meseros.Where(t => t.MeseroID == p.MeseroID).Single();
            var mesa = mesas.Where(t => t.MesaID == p.MesaID).Single();

            var pedidoMostrar = new VistaPedido
            {
                PedidoID = p.PedidoID,
                ClienteID = p.ClienteID,
                MeseroID = p.MeseroID,
                MesaID = p.MesaID,
                Estado = Enum.GetName(typeof(Estado), p.Estado),
                NombreCliente = cliente.Nombre,
                ApellidoCliente = cliente.Apellido,
                NombreMesero = mesero.Nombre,
                ApellidoMesero = mesero.Apellido,
                NumeroMesa = mesa.Numero,
                FechaPedido = p.FechaPedido.ToString("yyyy-MM-ddTHH:mm"),
                Total = p.Total.ToString("C", new System.Globalization.CultureInfo("es-AR"))
            };

            pedidosMostrar.Add(pedidoMostrar);
        }


        return Json(pedidosMostrar);
    }

     public JsonResult TraerPedidosAlModal(int? PedidoID)
    {
        var pedidosPorID = _context.Pedidos.ToList();
        if (PedidoID != null)
        {
            pedidosPorID = pedidosPorID.Where(e => e.PedidoID == PedidoID).ToList();
        }

        return Json(pedidosPorID.ToList());
    }

        public JsonResult GuardarPedido(int? PedidoID, int ClienteID, int MeseroID, int MesaID, Estado Estado, DateTime FechaPedido)
{
    string resultado = "Error al guardar el pedido";
    bool exito = false;
    string estadoTexto = Estado.ToString().Replace("_", " ");
    if (PedidoID == 0)
    {
        if (ClienteID > 0 && MesaID > 0 && MeseroID > 0)
        {
            var pedido = new Pedido
            {
                ClienteID = ClienteID,
                MeseroID = MeseroID,
                MesaID = MesaID,
                Estado = Estado,
                FechaPedido = FechaPedido,
            };
            _context.Add(pedido);
            _context.SaveChanges();
            exito = true;
            resultado = "Pedido guardado con éxito";
        }
    }
    else
    {
        var editarPedido = _context.Pedidos.Where(e => e.PedidoID == PedidoID).SingleOrDefault();
        if (editarPedido != null)
        {
            editarPedido.ClienteID = ClienteID;
            editarPedido.MeseroID = MeseroID;
            editarPedido.MesaID = MesaID;
            editarPedido.Estado = Estado;
            editarPedido.FechaPedido = FechaPedido;
            _context.SaveChanges();
            exito = true;
            resultado = "Pedido actualizado con éxito";
        }
    }
    return Json(new { exito, mensaje = resultado });
}

        public JsonResult EliminarPedido(int PedidoID)
    {
        var pedido = _context.Pedidos.Find(PedidoID);
        _context.Remove(pedido);
        _context.SaveChanges();

        return Json(true);
    }

    public IActionResult DetallePedido(int id)
    {
        var pedido = _context.Pedidos.Find(id);

        if (pedido == null)
        {
            return Redirect("/Pedidos");
        }

        ViewBag.PedidoID = pedido?.PedidoID;

        ViewBag.Fecha = pedido?.FechaPedido.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.Cliente = _context.Clientes.Find(pedido?.ClienteID);
        ViewBag.Mesero = _context.Meseros.Find(pedido?.MeseroID);
        ViewBag.Mesa = _context.Mesas.Find(pedido?.MesaID);

        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };

        ViewBag.MenuID = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.PlatoID = selectListItems.OrderBy(t => t.Text).ToList();

        var menus = _context.Menus.ToList();
        var platos = _context.Platos.ToList();

        menus.Add(new Menu { MenuID = 0, Nombre = "[MENUS...]" });
        ViewBag.MenuID = new SelectList(menus.OrderBy(c => c.Nombre), "MenuID", "Nombre");

        platos.Add(new Plato { PlatoID = 0, Nombre = "[PLATOS...]" });
        ViewBag.PlatoID = new SelectList(platos.OrderBy(c => c.Nombre), "PlatoID", "Nombre");


        return View();
    }

    public JsonResult ListadoDetalle(int PedidoID)
    {
        var detallePedido = _context.DetallesPedidos.Where(d => d.PedidoID == PedidoID).ToList();

        return Json(detallePedido);
    }

    public JsonResult ListadoDetallesPedido(int PedidoID)
{
    List<VistaDetallePedido> detallesMostrar = new List<VistaDetallePedido>();
    var detallesPedidos = _context.DetallesPedidos.Where(d => d.PedidoID == PedidoID).ToList(); // Filtrar por PedidoID

    var platos = _context.Platos.ToList();

    foreach (var d in detallesPedidos)
    {
        var plato = platos.Where(t => t.PlatoID == d.PlatoID).Single();
        var detalleMostrar = new VistaDetallePedido
        {
            DetallePedidoID = d.DetallePedidoID,
            PlatoID = d.PlatoID,
            NombrePlato = plato.Nombre,
            Cantidad = d.Cantidad,
            PedidoID = d.PedidoID,
            PrecioPlato = plato.Precio,
            Subtotal = d.Cantidad * plato.Precio
        };
        detallesMostrar.Add(detalleMostrar);
    }
    return Json(detallesMostrar);
}


    public JsonResult GuardarDetalle(int DetallePedidoID, int PlatoID, string NombrePlato, int Cantidad, decimal PrecioPlato, decimal Subtotal, int PedidoID)
{
    string resultado = "Error al guardar el detalle del pedido";
    if (DetallePedidoID == 0)
    {
        if (PlatoID > 0 && NombrePlato.Length > 0 && Cantidad > 0 && PrecioPlato > 0)
        {
            var detalle = new DetallePedido
            {
                PlatoID = PlatoID,
                Cantidad = Cantidad,
                PrecioUnitario = PrecioPlato,
                PedidoID = PedidoID // Asegurarse de asignar el PedidoID
            };
            _context.Add(detalle);
            _context.SaveChanges();
            resultado = "Detalle de Pedido Guardado";
        }
    }
    return Json(resultado);
}

public JsonResult EliminarDetalle(int DetallePedidoID)
{
    var detalle = _context.DetallesPedidos.Find(DetallePedidoID);
    if (detalle != null)
    {
        _context.Remove(detalle);
        _context.SaveChanges();
        return Json(new { exito = true, mensaje = "Detalle eliminado con éxito" });
    }
    return Json(new { exito = false, mensaje = "Error al eliminar el detalle" });
}




}