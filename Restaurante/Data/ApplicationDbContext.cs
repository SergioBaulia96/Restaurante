using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mesero> Meseros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
}
