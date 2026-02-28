using Microsoft.AspNetCore.Mvc;
using AccessControl.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Privada_WS.Data;

namespace Privada_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Inyectamos la base de datos
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            // 1. Buscar al usuario por correo e INCLUIR su Vivienda (para saber si pagó)
            var usuario = await _context.Usuarios
                .Include(u => u.Vivienda)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // 2. Si el correo no existe en la base de datos
            if (usuario == null)
            {
                return BadRequest(new LoginResponse { Exito = false, Mensaje = "Correo o contraseña incorrectos." });
            }

            // 3. Validar si está activo (por si lo dieron de baja de la privada)
            if (!usuario.Activo)
            {
                return Unauthorized(new LoginResponse { Exito = false, Mensaje = "Usuario inactivo. Contacte a la administración." });
            }

            // 4. Validar contraseña 
            // NOTA: Por ahora comparamos texto plano para la prueba de concepto. 
            // Más adelante usaremos un Hash (BCrypt) por seguridad profesional.
            if (usuario.PasswordHash != request.Password)
            {
                return BadRequest(new LoginResponse { Exito = false, Mensaje = "Correo o contraseña incorrectos." });
            }

            // 5. Si pasa todos los filtros, preparamos el paquete de bienvenida
            var response = new LoginResponse
            {
                Exito = true,
                Mensaje = "Inicio de sesión exitoso.",
                UsuarioId = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                EsAdministrador = usuario.EsAdministrador,
                MantenimientoPagado = usuario.Vivienda != null && usuario.Vivienda.MantenimientoPagado
            };

            return Ok(response);
        }
    }
}
