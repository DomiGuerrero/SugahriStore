using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;
using System.Reflection;

namespace SugahriStore.ManejoDatos
{
    public class BaseDeDatosContext : DbContext
    {
        static readonly string Database = "dbSqlite.db";
        string ejemplo;
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<LineaPedido> LineasPedido { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Filename=" + Database,
                sqliteOptionsAction: op =>
                {
                    op.MigrationsAssembly(
                        Assembly.GetExecutingAssembly().FullName
                        );
                });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().ToTable("Pedidos")
                .HasMany(p => p.LineasPedido)
                .WithOne(lp => lp.Pedido)
                .HasForeignKey(lp => lp.PedidoId);

            modelBuilder.Entity<LineaPedido>().ToTable("LineasPedido");

            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Auditoria>().ToTable("Auditorias");

            modelBuilder.Entity<Rol>().ToTable("Roles")
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
