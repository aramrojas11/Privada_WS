using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessControl.Shared.DTOs
{
    public class LoginResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public bool EsAdministrador { get; set; }

        //Propiedad para ocultar el boton de CAMARAS
        public bool MantenimientoPagado { get; set; }
    }
}
