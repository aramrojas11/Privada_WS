using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AccessControl.Shared.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty; // Útil para la integración SIP

        public bool EsAdministrador { get; set; } = false;

        public bool Activo { get; set; } = true;

        // Llave foránea a Vivienda
        public int ViviendaId { get; set; }

        [ForeignKey("ViviendaId")]
        public virtual Vivienda? Vivienda { get; set; }

        [JsonIgnore]
        public virtual ICollection<Acceso> Accesos { get; set; } = new List<Acceso>();

        [JsonIgnore]
        public virtual ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
