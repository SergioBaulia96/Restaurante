using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

public class DetallePedido
{
    [Key]
    public int DetallePedidoID { get; set; }
    public int PedidoID { get; set; }
    public int PlatoID { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    
    [NotMapped] // No se almacenará en la base de datos
    public decimal Subtotal 
    {
        get { return Cantidad * PrecioUnitario; }
    }
    public virtual Pedido Pedidos { get; set; }
    public virtual Plato Platos { get; set; }


}
