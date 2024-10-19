
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

[Authorize]
public class MenusController : Controller
{
    private ApplicationDbContext _context;

    public MenusController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoMenus(int? MenuID)
    {
        var listadoMenus = _context.Menus.ToList();
            listadoMenus = _context.Menus.OrderBy(l => l.Nombre).ToList();

        if(MenuID != null)
        {
            listadoMenus = _context.Menus.Where(l => l.MenuID == MenuID).ToList();
        }
        return Json(listadoMenus);
    }

    public JsonResult GuardarMenu(int MenuID, string Nombre)
{
    string resultado = "";
    
    Nombre = Nombre.ToUpper();
    
    // Verificar si ya existe un menú con el mismo nombre
    var menuExistente = _context.Menus
        .Where(m => m.Nombre == Nombre && m.MenuID != MenuID) // Excluir el menú actual si está en modo de edición
        .SingleOrDefault();

    if (menuExistente != null)
    {
        // Si ya existe un menú con ese nombre, devolver un mensaje de error
        return Json("El nombre del menú ya existe.");
    }
    
    if (MenuID == 0)
    {
        // Guardar un nuevo menú
        var nuevoMenu = new Menu
        {
            Nombre = Nombre,
        };
        _context.Add(nuevoMenu);
        _context.SaveChanges();
        resultado = "Menú guardado exitosamente";
    }
    else
    {
        // Editar un menú existente
        var editarMenu = _context.Menus.Where(e => e.MenuID == MenuID).SingleOrDefault();
        
        if (editarMenu != null)
        {
            editarMenu.Nombre = Nombre;
            _context.SaveChanges();
            resultado = "Menú editado exitosamente";
        }
    }
    
    return Json(resultado);
}


    public JsonResult EliminarMenu(int MenuID)
    {
        var eliminarMenu = _context.Menus.Find(MenuID);
        _context.Remove(eliminarMenu);
        _context.SaveChanges();

        return Json(eliminarMenu);
    }

    public IActionResult Platos()
    {
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
        };
        ViewBag.MenuID = selectListItems.OrderBy(t => t.Text).ToList();
        var menus = _context.Menus.ToList();

        menus.Add(new Menu { MenuID = 0, Nombre = "[MENUS...]" });
        ViewBag.MenuID = new SelectList(menus.OrderBy(c => c.Nombre), "MenuID", "Nombre");
        return View();
    }



    public JsonResult ListadoPlatos(int? id)
    {
        List<VistaPlato> platosMostrar = new List<VistaPlato>();

        var platos = _context.Platos.AsQueryable();

        if (id != null)
        {
            platos = platos.Where(t => t.PlatoID == id);
        }

        var menus = _context.Menus.ToList();

        foreach (var p in platos)
        {
            var menu = menus.Where(t => t.MenuID == p.MenuID).Single();

            var platoMostrar = new VistaPlato
            {
                PlatoID = p.PlatoID,
                NombrePlato = p.Nombre,
                Precio = p.Precio,
                MenuID = p.MenuID,
                NombreMenu = menu.Nombre,
                Descripcion = p.Descripcion,
                Disponible = p.Disponible
            };
            platosMostrar.Add(platoMostrar);
        }


        return Json(platosMostrar);
    }

    public JsonResult TraerPlatosAlModal(int? PlatoID)
    {
        var platosPorID = _context.Platos.ToList();
        if (PlatoID != null)
        {
            platosPorID = platosPorID.Where(e => e.PlatoID == PlatoID).ToList();
        }

        return Json(platosPorID.ToList());
    }

    public JsonResult GuardarPlato(
        int PlatoID,
        int MenuID,
        string NombrePlato,
        string Descripcion,
        decimal Precio,
        bool Disponible
        )
    {
        string resultado = "";

        NombrePlato = NombrePlato.ToUpper();

        if (PlatoID == 0)
        {
            if (MenuID > 0)
            {
                var Plato = new Plato
                {
                    MenuID = MenuID,
                    Nombre = NombrePlato,
                    Descripcion = Descripcion,
                    Precio = Precio,
                    Disponible = Disponible
                };
                _context.Add(Plato);
                _context.SaveChanges();
            }
        }
        else
        {
            var editarPlato = _context.Platos.Where(e => e.PlatoID == PlatoID).SingleOrDefault();
            if (editarPlato != null)
            {
                editarPlato.MenuID = MenuID;
                editarPlato.Nombre = NombrePlato;
                editarPlato.Descripcion = Descripcion;
                editarPlato.Precio = Precio;
                editarPlato.Disponible = Disponible;

                _context.SaveChanges();
                resultado = "Plato Editado";
            }
        }
        return Json(resultado);
    }

        public JsonResult EliminarPlato(int PlatoID)
    {
        var plato = _context.Platos.Find(PlatoID);
        _context.Remove(plato);
        _context.SaveChanges();

        return Json(true);
    }
}