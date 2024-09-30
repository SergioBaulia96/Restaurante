using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

    public class Mesa
    {
        [Key]
        public int MesaID { get; set; }
        public string? Numero { get; set; }
        public int Capacidad { get; set; }
        public bool Disponible { get; set; } = true;
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Reservacion> Reservaciones { get; set; }
    }

