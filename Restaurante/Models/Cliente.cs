using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Reservacion> Reservaciones { get; set; }
    }

