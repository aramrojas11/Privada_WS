using AccessControl.Shared.DTOs;
using Privada_WS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Privada_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 1. OBTENER TODOS LOS USUARIOS (Gestión de inquilinos)
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Vivienda) // Unimos la tabla de Viviendas para ver el mantenimiento
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    NombreCompleto = u.NombreCompleto,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    EsAdministrador = u.EsAdministrador,
                    Activo = u.Activo,
                    NumeroCasa = u.Vivienda != null ? u.Vivienda.NumeroCasa : "N/A",
                    MantenimientoPagado = u.Vivienda != null && u.Vivienda.MantenimientoPagado
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // 2. CORTAR O DAR ACCESO A UN USUARIO (Bloquear desde la app)
        [HttpPut("usuarios/{id}/toggle-acceso")]
        public async Task<IActionResult> ToggleAccesoUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado" });

            // Invertimos su estado: si estaba activo, lo bloqueamos, y viceversa
            usuario.Activo = !usuario.Activo;
            await _context.SaveChangesAsync();

            string estado = usuario.Activo ? "activado" : "bloqueado";
            return Ok(new { exito = true, mensaje = $"Usuario {estado} exitosamente." });
        }

        // 3. VER EL HISTORIAL DE ACCESOS Y CÁMARAS
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial()
        {
            var historial = await _context.Accesos
                .Include(a => a.Usuario)
                .Include(a => a.Porton)
                .OrderByDescending(a => a.FechaHora) // Los más recientes primero
                .Take(100) // Limitamos a 100 para no consumir todos los datos del celular
                .Select(a => new HistorialDTO
                {
                    Id = a.Id,
                    NombreUsuario = a.Usuario != null ? a.Usuario.NombreCompleto : "Desconocido",
                    PuertaOCamara = a.Porton != null ? a.Porton.Nombre : "Cámara de vigilancia",
                    FechaHora = a.FechaHora,
                    Metodo = a.Metodo,
                    Exitoso = a.Exitoso
                })
                .ToListAsync();

            return Ok(historial);
        }
    }
}