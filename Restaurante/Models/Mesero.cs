using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;
    public class Mesero
    {
        [Key]
        public int MeseroID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        [NotMapped]
        public string? NombreCompleto { get { return $"{Nombre} {Apellido}";} }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }

