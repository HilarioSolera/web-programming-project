using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplicationProyecto.Data
{
    public class DBcontexto : DbContext
    {
        public DBcontexto(DbContextOptions<DBcontexto> options) : base(options)
        {
            // Puedes probar conexión aquí si deseas
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Lavado> Lavados { get; set; }

        // Vista de reporte
        public DbSet<ClienteReporte> ClientesSinLavadoReciente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vista sin clave
            modelBuilder.Entity<ClienteReporte>()
                .HasNoKey()
                .ToView("ClientesSinLavadoReciente");

            // Precisión para decimales
            modelBuilder.Entity<Empleado>()
                .Property(e => e.SalarioPorDia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.MontoLiquidacion)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Lavado>()
                .Property(l => l.Precio)
                .HasPrecision(10, 2);

            // Relaciones con DeleteBehavior.Restrict
            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lavado>()
                .HasOne(l => l.Cliente)
                .WithMany(c => c.Lavados)
                .HasForeignKey(l => l.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lavado>()
                .HasOne(l => l.Vehiculo)
                .WithMany(v => v.Lavados)
                .HasForeignKey(l => l.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lavado>()
                .HasOne(l => l.Empleado)
                .WithMany(e => e.Lavados)
                .HasForeignKey(l => l.EmpleadoId)
                .OnDelete(DeleteBehavior.SetNull); // opcional, puede quedar null
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configura el timeout si es necesario
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=Proyecto;Trusted_Connection=True;TrustServerCertificate=True;",
                    opt => opt.CommandTimeout(60)); // 60 segundos de timeout
            }
        }
    }
}
