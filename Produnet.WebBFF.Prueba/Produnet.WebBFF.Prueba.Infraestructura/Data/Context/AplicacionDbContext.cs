using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Data.Context
{
    public class AplicacionDbContext : DbContext
    {
        public DbSet<SesionUsuario> SesionesUsuario { get; set; }
        public DbSet<PeriodoInactividad> PeriodosInactividad { get; set; }

        public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SesionUsuario>()
                .HasMany(us => us.PeriodosInactividad)
                .WithOne(ip => ip.SesionUsuario)
                .HasForeignKey(ip => ip.SesionUsuarioId);
        }
    }
}
