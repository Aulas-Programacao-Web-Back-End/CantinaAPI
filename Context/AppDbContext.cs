using Microsoft.EntityFrameworkCore;
using CantinaAPI.Models;

namespace CantinaAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PKs
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);

            // 🔥 RELACIONAMENTO Pedido -> Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany() // um cliente pode ter vários pedidos
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 RELACIONAMENTO Pedido -> Produto
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Produto)
                .WithMany() // um produto pode estar em vários pedidos
                .HasForeignKey(p => p.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}