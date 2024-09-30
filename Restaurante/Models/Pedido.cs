using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

    public class Pedido
    {
        [Key]
        public int PedidoID { get; set; }
        public int MesaID { get; set; }
        public int MeseroID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public Estado Estado { get; set; }
        
        [NotMapped]
        public decimal Total
        {
            get { return DetallesPedidos?.Sum(d => d.Subtotal) ?? 0; }
        }
        public virtual Cliente Clientes { get; set; }
        public virtual Mesero Meseros { get; set; }
        public virtual Mesa Mesas { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; }
    }

    public enum Estado
    {
        EnPreparación = 1,
        Servido,
        Cancelado,
    }

    public class VistaPedido
    {
        public int PedidoID { get; set; }
        public int MesaID { get; set; }
        public int MeseroID { get; set; }
        public int ClienteID { get; set; }
        public string? NumeroMesa { get; set; }
        public string? NombreMesero {get ; set;}
        public string? NombreCliente {get; set; }
        public string? FechaPedido { get; set; }
        public string? Estado { get; set; }
        public string? Total {get; set;}

    }

