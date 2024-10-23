using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

    public class Reservacion
    {
        [Key]
        public int ReservacionID { get; set; }
        public int ClienteID { get; set; }
        public int MesaID { get; set; }
        public DateTime FechaReservacion { get; set; }
        public string? Notas { get; set; }
        public virtual Cliente Clientes { get; set; }
        public virtual Mesa Mesas { get; set; }
    }

public class VistaReservacion
{
    public IEnumerable<Mesa> Mesas { get; set; }
    public List<string> HorariosDisponibles { get; set; }
    public DateTime FechaSeleccionada { get; set; }
}

