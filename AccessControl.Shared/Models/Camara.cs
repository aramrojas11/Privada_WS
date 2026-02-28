using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Shared.Models
{
    public class Camara
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string RtspUrl { get; set; } = string.Empty;

        [ForeignKey("PortonId")]
        public virtual Porton? Porton { get; set; }
    }
}
