using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

    public class Mesa
    {
        [Key]
        public int MesaID { get; set; }
        public string? Numero { get; set; }
        public int Capacidad { get; set; }
        public bool Disponible { get; set; } = true;
        [NotMapped]
        public string? InfoCompleta { get { return $"{Numero} {Capacidad}";} }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Reservacion> Reservaciones { get; set; }
    }

