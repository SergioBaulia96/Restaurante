using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models
{
    public class Plato
    {
        [Key]
        public int PlatoID { get; set; }
        public int MenuID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; } = true;
        public virtual Menu Menus { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; }
    }
}
