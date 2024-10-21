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
    public decimal Subtotal
    {
        get { return Cantidad * PrecioUnitario; }
    }

    public virtual Pedido Pedidos { get; set; }
    public virtual Plato Platos { get; set; }
}

public class VistaDetallePedido
{
    public int DetallePedidoID { get; set; }
    public int PedidoID { get; set; }
    public int PlatoID { get; set; }
    public int Cantidad { get; set; }
    public decimal? TotalPedido {get; set;}
    public string? NombrePlato {get; set;}
    public decimal PrecioPlato { get; set; }
    public decimal Subtotal { get; set; }
}
