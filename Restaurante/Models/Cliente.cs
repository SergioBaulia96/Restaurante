using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        [NotMapped]
        public string? NombreCompleto { get { return $"{Nombre} {Apellido}";} }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Reservacion> Reservaciones { get; set; }
    }

