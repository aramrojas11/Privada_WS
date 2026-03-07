using AccessControl.Shared.DTOs;
using AccessControl.Shared.Models;
using Privada_WS.Data;
using Microsoft.AspNetCore.Mvc;

namespace Privada_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccionesController(AppDbContext context)
        {
            _context = context;
        }

        #region ACEESO PEATONAL Y VEHICULAR 
        [HttpPost("vehicular")]
        public async Task<IActionResult> AbrirVehicular([FromBody] AccionRequest request)
        {
            //ENVIAMOS USUARIO ID Y PORTON ID PARA REGISTRAR EL ACCESO
            return await RegistrarAccesoY_Abrir(request.UsuarioId, portonId: 1);
        }

        [HttpPost("peatonal")]
        public async Task<IActionResult> AbrirPeatonal([FromBody] AccionRequest request)
        {
            //ENVIAMOS USUARIO ID Y PORTON ID PARA REGISTRAR EL ACCESO
            return await RegistrarAccesoY_Abrir(request.UsuarioId, portonId: 2);
        }
        #endregion

        [HttpPost("alarma")]
        public async Task<IActionResult> ActivarAlarma([FromBody] AccionRequest request)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);
                if (usuario == null) return BadRequest(new { exito = false, mensaje = "Usuario no válido." });

                var nuevaAlerta = new Alerta
                {
                    UsuarioId = request.UsuarioId,
                    FechaHora = DateTime.UtcNow,
                    Estado = "Activa",
                    ResolucionAdmin = "Pendiente"
                };

                _context.Alertas.Add(nuevaAlerta);
                await _context.SaveChangesAsync();

                // TODO: Mandar HTTP Request al ESP32 para encender la sirena física

                return Ok(new { exito = true, mensaje = "Alarma vecinal activada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, mensaje = $"Error: {ex.Message}" });
            }
        }

        private async Task<IActionResult> RegistrarAccesoY_Abrir(int usuarioId, int portonId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return BadRequest(new { exito = false, mensaje = "Usuario no válido." });

                // Lógica de negocio: Validar si puede entrar
                bool tienePermiso = usuario.Activo; // Asumiendo que validas si está activo

                // Creamos el registro con tu modelo exacto
                var nuevoAcceso = new Acceso
                {
                    UsuarioId = usuarioId,
                    PortonId = portonId,
                    FechaHora = DateTime.UtcNow,
                    Metodo = "App Móvil",
                    Exitoso = tienePermiso // Aquí usamos tu booleano
                };

                _context.Accesos.Add(nuevoAcceso);
                await _context.SaveChangesAsync();

                if (!tienePermiso)
                {
                    return BadRequest(new { exito = false, mensaje = "Acceso denegado. Contacte a la administración." });
                }

                // TODO: Aquí la API hace una petición HTTP a la IP del ESP32 para abrir el relé

                return Ok(new { exito = true, mensaje = "Acceso concedido. Abriendo puerta..." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, mensaje = $"Error: {ex.Message}" });
            }
        }
    }
}