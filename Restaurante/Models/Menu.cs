using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

    public class Menu
    {
        [Key]
        public int MenuID { get; set; }
        public string? Nombre { get; set; }
        public virtual ICollection<Plato> Platos { get; set; }
    }


    public class VistaMenu
    {
        public int MenuID { get; set; }
        public string? Nombre { get; set; }
        public List<VistaPlato> ListadoPlatos { get; set; }
    }

