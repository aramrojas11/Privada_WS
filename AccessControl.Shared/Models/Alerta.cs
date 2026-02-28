using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Shared.Models
{
    public class Alerta
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Estado { get; set; } = "Activa"; // Activa, Atendida, Falsa Alarma

        [MaxLength(255)]
        public string? ResolucionAdmin { get; set; } // Notas del administrador que atendió la alerta
    }
}
