using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccessControl.Shared.Models
{
    public class Porton
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; // Ej: "Principal", "Peatonal"

        [Required]
        [MaxLength(50)]
        public string DireccionIP_ESP32 { get; set; } = string.Empty;

        [MaxLength(20)]
        public string EstadoActual { get; set; } = "Cerrado";

        [JsonIgnore]
        public virtual ICollection<Acceso> Accesos { get; set; } = new List<Acceso>();

        [JsonIgnore]
        public virtual ICollection<Camara> Camaras { get; set; } = new List<Camara>();
    }
}
