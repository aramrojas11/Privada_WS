using AccessControl.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Privada_WS.Data
{
    public class AppDbContext : DbContext
    {
        // El constructor que recibe las opciones de conexión (la cadena de conexión)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aquí le decimos a Entity Framework cuáles clases se van a convertir en Tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vivienda> Viviendas { get; set; }
        public DbSet<Porton> Portones { get; set; }
        public DbSet<Camara> Camaras { get; set; }
        public DbSet<Acceso> Accesos { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
    }
}