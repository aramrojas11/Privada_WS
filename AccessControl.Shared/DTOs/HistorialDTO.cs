using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessControl.Shared.DTOs
{
    public class HistorialDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string PuertaOCamara { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public bool Exitoso { get; set; }
    }
}
