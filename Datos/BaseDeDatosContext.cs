using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SugahriStore.Datos
{
    public class BaseDeDatosContext : DbContext
    {
        private static readonly string DatabasePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Database", "dbSqlite.db");

        // DbSets que representan las tablas de la base de datos
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<LineaPedido> LineasPedido { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configuración de SQLite como proveedor de la base de datos
            string connectionString = $"Data Source={DatabasePath}";
            optionsBuilder.UseSqlite(connectionString: connectionString,
                sqliteOptionsAction: op =>
                {
                    // Se especifica la ubicación de las migraciones
                    op.MigrationsAssembly(
                        Assembly.GetExecutingAssembly().FullName
                        );

                });

            base.OnConfiguring(optionsBuilder);

        }

        // Método para realizar el hash de una contraseña utilizando SHA256
        private static string HashContraseña(string password)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definición de las relaciones y configuración de las tablas

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

            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Auditoria>().ToTable("Auditorias");
            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Auditoria)
                .WithOne()
                .HasForeignKey<Pedido>(p => p.AuditoriaId)
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithOne()
                .HasForeignKey<Pedido>(p => p.ClienteId)
                .IsRequired(false);

            modelBuilder.Entity<Rol>().ToTable("Roles")
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            // Se agregan datos iniciales de usuarios y roles
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = -1,
                Nombre = "ADMIN",
                Contraseña = HashContraseña("ADMIN"),
                RolId = 1
            }
            );
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "ADMIN" },
                new Rol { Id = 2, Nombre = "USER" }
            );

            base.OnModelCreating(modelBuilder);
        }

        public void CrearBaseDeDatos()
        {
            Database.EnsureCreated();
        }
    }
}
