using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Shared.Models
{
    public class Acceso
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        public int PortonId { get; set; }
        [ForeignKey("PortonId")]
        public virtual Porton? Porton { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Metodo { get; set; } = "App Móvil"; // App Móvil, Intercomunicador, etc.

        public bool Exitoso { get; set; }
    }
}
