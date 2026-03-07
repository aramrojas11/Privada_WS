using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessControl.Shared.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool EsAdministrador { get; set; }
        public bool Activo { get; set; }
        // Traemos datos de su vivienda para saber si debe dinero
        public string NumeroCasa { get; set; } = string.Empty;
        public bool MantenimientoPagado { get; set; }
    }
}
