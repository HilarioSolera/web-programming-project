using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationProyecto.Data;
using Modelos;

namespace WebApplicationProyecto.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly DBcontexto _context;

        public ClientesController(DBcontexto context)
        {
            _context = context;
        }

        // 🔍 GET: api/clientes/buscar/{filtro}
        [HttpGet("buscar/{filtro}")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> BuscarPorFiltro(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return BadRequest("El filtro no puede estar vacío.");

            try
            {
                var clientes = await _context.Clientes
                    .Where(c =>
                        c.Identificacion.Contains(filtro) ||
                        c.NombreCompleto.Contains(filtro) ||
                        c.Telefono.Contains(filtro) ||
                        c.Correo.Contains(filtro))
                    .Select(c => new ClienteDTO
                    {
                        Id = c.Id,
                        Identificacion = c.Identificacion,
                        NombreCompleto = c.NombreCompleto,
                        Provincia = c.Provincia,
                        Canton = c.Canton,
                        Distrito = c.Distrito,
                        DireccionExacta = c.DireccionExacta,
                        Telefono = c.Telefono,
                        Correo = c.Correo,
                        PreferenciaLavado = c.PreferenciaLavado
                    })
                    .ToListAsync();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al filtrar clientes: {ex.Message}");
            }
        }
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> BuscarTodos()
        {
            try
            {
                var clientes = await _context.Clientes
                    .AsNoTracking()
                    .OrderBy(c => c.Id) // Control predecible
                    .Select(c => new ClienteDTO
                    {
                        Id = c.Id,
                        Identificacion = c.Identificacion,
                        NombreCompleto = c.NombreCompleto,
                        Provincia = c.Provincia,
                        Canton = c.Canton,
                        Distrito = c.Distrito,
                        DireccionExacta = c.DireccionExacta,
                        Telefono = c.Telefono,
                        Correo = c.Correo,
                        PreferenciaLavado = c.PreferenciaLavado
                    })
                    .Take(100) // Límite explícito para evitar timeout
                    .ToListAsync();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al obtener clientes: {ex.Message}");
            }
        }

        // 🔍 GET: api/clientes/buscar-id/{id}
        [HttpGet("buscar-id/{id}")]
        public async Task<ActionResult<ClienteDTO>> BuscarPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente is null)
                return NotFound($"No se encontró el cliente con ID {id}.");

            var dto = new ClienteDTO
            {
                Id = cliente.Id,
                Identificacion = cliente.Identificacion,
                NombreCompleto = cliente.NombreCompleto,
                Provincia = cliente.Provincia,
                Canton = cliente.Canton,
                Distrito = cliente.Distrito,
                DireccionExacta = cliente.DireccionExacta,
                Telefono = cliente.Telefono,
                Correo = cliente.Correo,
                PreferenciaLavado = cliente.PreferenciaLavado
            };

            return Ok(dto);
        }

        // ➕ POST: api/clientes/agregar
        [HttpPost("agregar")]
        public async Task<ActionResult<ClienteDTO>> AgregarCliente([FromBody] ClienteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Clientes.AnyAsync(c => c.Identificacion == dto.Identificacion);
            if (existe)
                return Conflict("Ya existe un cliente con esa cédula.");

            var cliente = new Cliente
            {
                Identificacion = dto.Identificacion,
                NombreCompleto = dto.NombreCompleto,
                Provincia = dto.Provincia,
                Canton = dto.Canton,
                Distrito = dto.Distrito,
                DireccionExacta = dto.DireccionExacta,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                PreferenciaLavado = dto.PreferenciaLavado
            };

            try
            {
                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();

                dto.Id = cliente.Id;
                return CreatedAtAction(nameof(BuscarPorId), new { id = dto.Id }, dto);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar cliente: {ex.Message}");
            }
        }

        // ✏️ PUT: api/clientes/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID en la URL no coincide con el objeto enviado.");

            var existente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (existente is null)
                return NotFound($"No existe el cliente con ID {id}.");

            existente.Identificacion = dto.Identificacion;
            existente.NombreCompleto = dto.NombreCompleto;
            existente.Provincia = dto.Provincia;
            existente.Canton = dto.Canton;
            existente.Distrito = dto.Distrito;
            existente.DireccionExacta = dto.DireccionExacta;
            existente.Telefono = dto.Telefono;
            existente.Correo = dto.Correo;
            existente.PreferenciaLavado = dto.PreferenciaLavado;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el cliente: {ex.Message}");
            }
        }

        // 🗑️ DELETE: api/clientes/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente is null)
                return NotFound($"No se encontró el cliente con ID {id}.");

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return Ok("Cliente eliminado correctamente.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                return Conflict("No se puede eliminar este cliente porque está relacionado con vehículos o lavados.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error inesperado al eliminar el cliente.");
            }
        }
    }
}
