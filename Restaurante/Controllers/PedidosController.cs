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

        ViewBag.MeseroID = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.MesaID = selectListItems.OrderBy(t => t.Text).ToList();


        var meseros = _context.Meseros.ToList();
        var mesas = _context.Mesas.ToList();



        meseros.Add(new Mesero { MeseroID = 0, Apellido = "[MESERO...]" });
        ViewBag.MeseroID = new SelectList(meseros.OrderBy(c => c.NombreCompleto), "MeseroID", "NombreCompleto");

        mesas.Add(new Mesa { MesaID = 0, Numero = "[MESA...]" });
        ViewBag.MesaID = new SelectList(mesas.OrderBy(c => c.Numero), "MesaID", "Numero");

        return View();
    }

    public JsonResult ListadoPedidos(DateTime? fechaListado)
    {
        List<VistaPedido> pedidosMostrar = new List<VistaPedido>();

        var pedidos = _context.Pedidos.ToList();

        if (fechaListado != null)
        {
            pedidos = pedidos.Where(p => p.FechaPedido.Date == fechaListado.Value.Date).ToList();
        }

            // Verificar si hay pedidos después de aplicar el filtro
        if (!pedidos.Any())
        {
            return Json(pedidosMostrar); // Retorna una lista vacía si no hay pedidos en la fecha
        }


        var meseros = _context.Meseros.ToList();
        var mesas = _context.Mesas.ToList();

        foreach (var p in pedidos)
        {

            var mesero = meseros.Where(t => t.MeseroID == p.MeseroID).Single();
            var mesa = mesas.Where(t => t.MesaID == p.MesaID).Single();

            var pedidoMostrar = new VistaPedido
            {
                PedidoID = p.PedidoID,
                MeseroID = p.MeseroID,
                MesaID = p.MesaID,
                Estado = Enum.GetName(typeof(Estado), p.Estado),

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

public JsonResult GuardarPedido(int? PedidoID,  int MeseroID, int MesaID, Estado Estado, DateTime FechaPedido)
{
    string resultado = "Error al guardar el pedido";
    bool exito = false;

    DateTime fechaActual = DateTime.Now;
    if (PedidoID == 0)
    {
        if (MesaID > 0 && MeseroID > 0)
        {
            var pedido = new Pedido
            {

                MeseroID = MeseroID,
                MesaID = MesaID,
                Estado = Estado.Preparando,
                FechaPedido = fechaActual,
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
            // Validar si el pedido está en estado "Listo"
            if (editarPedido.Estado == Estado.Listo)
            {
                return Json(new { exito = false, mensaje = "No se puede editar un pedido que está en estado 'Listo'." });
            }


            editarPedido.MeseroID = MeseroID;
            editarPedido.MesaID = MesaID;
            editarPedido.Estado = Estado;
            editarPedido.FechaPedido = fechaActual;
            _context.SaveChanges();
            exito = true;
            resultado = "Pedido actualizado con éxito";
        }
    }
    return Json(new { exito, mensaje = resultado });
}

public JsonResult EliminarPedido(int PedidoID)
{
    try
    {
        var pedido = _context.Pedidos.Find(PedidoID);
        if (pedido == null)
        {
            return Json(new { exito = false, mensaje = "El pedido no existe." });
        }

        // Validar si el pedido está en estado "Listo"
        if (pedido.Estado == Estado.Listo)
        {
            return Json(new { exito = false, mensaje = "No se puede eliminar un pedido que está en estado 'Listo'." });
        }

        var detalles = _context.DetallesPedidos.Where(d => d.PedidoID == PedidoID).ToList();
        if (detalles.Any())
        {
            _context.DetallesPedidos.RemoveRange(detalles);
        }

        _context.Pedidos.Remove(pedido);
        _context.SaveChanges();

        return Json(new { exito = true });
    }
    catch (Exception ex)
    {
        // Log the exception (ex) here if needed
        return Json(new { exito = false, mensaje = "Ocurrió un error al eliminar el pedido." });
    }
}

    public IActionResult DetallePedido(int id)
    {


        var pedido = _context.Pedidos.Find(id);

        if (pedido == null)
        {
            return Redirect("/Pedidos");
        }

        ViewBag.PedidoID = pedido?.PedidoID;

        ViewBag.Fecha = pedido?.FechaPedido.ToString("dd/MM/yyyy hh:mm tt");
        ViewBag.Mesero = _context.Meseros.Find(pedido?.MeseroID);
        ViewBag.Mesa = _context.Mesas.Find(pedido?.MesaID);

        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };

        ViewBag.MenuID = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.PlatoID = selectListItems.OrderBy(t => t.Text).ToList();

        var menus = _context.Menus.OrderBy(m => m.Nombre).ToList();
        ViewBag.Menus = menus;
        var platos = _context.Platos.ToList();

        
        ViewBag.MenuID = new SelectList(menus.OrderBy(c => c.Nombre), "MenuID", "Nombre");

        
        ViewBag.PlatoID = new SelectList(platos.OrderBy(c => c.Nombre), "PlatoID", "Nombre");


        return View();
    }

    public IActionResult ObtenerPlatosPorMenu(int menuID)
{
    var platos = _context.Platos
                        .Where(p => p.MenuID == menuID)
                        .Select(p => new { p.PlatoID, p.Nombre })
                        .OrderBy(p => p.Nombre)
                        .ToList();

    return Json(platos);
}


    // Listar los detalles del pedido
public JsonResult ListadoDetalle(int PedidoID)
{
    var detalles = _context.DetallesPedidos
        .Where(d => d.PedidoID == PedidoID)
        .Select(d => new 
        {
            d.DetallePedidoID,
            d.Cantidad,
            d.Subtotal,
            Plato = d.Platos.Nombre,  // Nombre del plato
            PrecioPlato = d.PrecioUnitario
        }).ToList();

    return Json(detalles);
}

// Eliminar un detalle del pedido
public JsonResult EliminarDetalle(int DetallePedidoID)
{
    try
    {
        var detalle = _context.DetallesPedidos.Find(DetallePedidoID);
        if (detalle == null)
        {
            return Json(new { exito = false, mensaje = "El detalle del pedido no existe." });
        }

        var pedido = _context.Pedidos.Find(detalle.PedidoID);
        if (pedido?.Estado == Estado.Listo)
        {
            return Json(new { exito = false, mensaje = "No se pueden eliminar detalles de un pedido que ya está listo." });
        }

        if (pedido != null)
        {
            pedido.Total -= detalle.Subtotal;
        }

        _context.DetallesPedidos.Remove(detalle);
        _context.SaveChanges();

        return Json(new { exito = true });
    }
    catch (Exception ex)
    {
        // Log the exception (ex) here if needed
        return Json(new { exito = false, mensaje = "Ocurrió un error al eliminar el detalle del pedido." });
    }
}



    public JsonResult GuardarDetalle(int PedidoID, int PlatoID, int Cantidad)
{
    string resultado = "Error al guardar el detalle del pedido";
    bool exito = false;

    var pedidos = _context.Pedidos.Find(PedidoID);
    if (pedidos == null)
    {
        return Json(new { exito = false, mensaje = "No se encontró el pedido" });
    }

    // Verificar si el estado del pedido es "Listo"
    if (pedidos.Estado == Estado.Listo)
    {
        return Json(new { exito = false, mensaje = "No se pueden agregar detalles a un pedido que ya está listo." });
    }

    // Buscar el plato para obtener su precio
    var plato = _context.Platos.SingleOrDefault(p => p.PlatoID == PlatoID);
    
    if (plato != null && Cantidad > 0)
    {
        var pedido = _context.Pedidos.Find(PedidoID);
        if (pedido == null)
        {
            return Json(new { exito = false, mensaje = "No se encontro el pedido"});
        }

        var detalle = new DetallePedido
        {
            PedidoID = pedido.PedidoID,
            PlatoID = PlatoID,
            Cantidad = Cantidad,
            PrecioUnitario = plato.Precio,
            Subtotal = plato.Precio * Cantidad
        };

        pedido.Total += detalle.Subtotal;

        _context.DetallesPedidos.Add(detalle);
        _context.SaveChanges();

        exito = true;
        resultado = "Detalle del pedido guardado exitosamente";
    }

    return Json(new { exito, mensaje = resultado });
}

public JsonResult CalcularSubtotal(int PedidoID)
{
    var detalles = _context.DetallesPedidos.Where(d => d.PedidoID == PedidoID).ToList();
    var subtotal = detalles.Sum(d => d.Subtotal);

    return Json(new { subtotal });
}


}