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
        public DateTime FechaPedido { get; set; }
        public Estado Estado { get; set; }
        public decimal Total { get; set; }
        public virtual Cliente Clientes { get; set; }
        public virtual Mesero Meseros { get; set; }
        public virtual Mesa Mesas { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; }
    }

    public enum Estado
    {
        Preparando = 1,
        Listo,
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
        public string? ApellidoMesero { get; set; }
        public string? NombreCliente {get; set; }
        public string? ApellidoCliente { get; set; }
        public string? FechaPedido { get; set; }
        public string? Estado { get; set; }
        public string? Total {get; set;}

    }

