using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;
    public class Mesero
    {
        [Key]
        public int MeseroID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }

