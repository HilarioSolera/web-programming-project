using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationProyecto.Data;
using Modelos;
using Modelos.DTOs;

namespace WebApplicationProyecto.Controllers
{
    [Route("api/lavado_vehiculos")]
    [ApiController]
    public class LavadoVehiculosController : ControllerBase
    {
        private readonly DBcontexto _context;

        public LavadoVehiculosController(DBcontexto context)
        {
            _context = context;
        }

        // GET: api/lavado_vehiculos/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<LavadoDTO>>> Buscar([FromQuery] string? filtro)
        {
            var query = _context.Lavados.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLower();

                query = query.Where(l =>
                    l.TipoLavado.ToLower().Contains(f) ||
                    l.Estado.ToLower().Contains(f) ||
                    (l.Cliente != null && (
                        l.Cliente.NombreCompleto.ToLower().Contains(f) ||
                        l.Cliente.Identificacion.ToLower().Contains(f))) ||
                    (l.Vehiculo != null && l.Vehiculo.Placa.ToLower().Contains(f)) ||
                    (l.Empleado != null && l.Empleado.NombreCompleto.ToLower().Contains(f))
                );
            }

            var lavados = await query
                .Select(l => new LavadoDTO
                {
                    Id = l.Id,
                    Fecha = l.Fecha,
                    TipoLavado = l.TipoLavado,
                    Estado = l.Estado,
                    Precio = l.Precio,
                    ClienteId = l.ClienteId,
                    VehiculoId = l.VehiculoId,
                    EmpleadoId = l.EmpleadoId
                })
                .ToListAsync();

            return Ok(lavados);
        }

        // GET: api/lavado_vehiculos/buscar/{id}
        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<Lavado>> BuscarPorId(int id)
        {
            var lavado = await _context.Lavados
                .Include(l => l.Cliente)
                .Include(l => l.Vehiculo)
                .Include(l => l.Empleado)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lavado is null ? NotFound() : Ok(lavado);
        }

        // POST: api/lavado_vehiculos/agregar
        [HttpPost("agregar")]
        public async Task<ActionResult<Lavado>> AgregarLavado([FromBody] LavadoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId) ||
                !await _context.Vehiculos.AnyAsync(v => v.Id == dto.VehiculoId) ||
                !await _context.Empleados.AnyAsync(e => e.Id == dto.EmpleadoId))
            {
                return BadRequest("Alguna referencia (cliente, vehículo o empleado) no existe.");
            }

            var nuevo = new Lavado
            {
                Fecha = dto.Fecha,
                TipoLavado = dto.TipoLavado,
                Precio = dto.Precio,
                Estado = dto.Estado,
                ClienteId = dto.ClienteId,
                VehiculoId = dto.VehiculoId,
                EmpleadoId = dto.EmpleadoId
            };

            await _context.Lavados.AddAsync(nuevo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = nuevo.Id }, nuevo);
        }

        // PUT: api/lavado_vehiculos/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarLavado(int id, [FromBody] LavadoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID en la URL no coincide con el objeto enviado.");

            var existente = await _context.Lavados.FindAsync(id);
            if (existente == null)
                return NotFound();

            existente.Fecha = dto.Fecha;
            existente.TipoLavado = dto.TipoLavado;
            existente.Precio = dto.Precio;
            existente.Estado = dto.Estado;
            existente.ClienteId = dto.ClienteId;
            existente.VehiculoId = dto.VehiculoId;
            existente.EmpleadoId = dto.EmpleadoId;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el lavado: {ex.Message}");
            }
        }

        // DELETE: api/lavado_vehiculos/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarLavado(int id)
        {
            var lavado = await _context.Lavados.FindAsync(id);
            if (lavado is null) return NotFound();

            try
            {
                _context.Lavados.Remove(lavado);
                await _context.SaveChangesAsync();
                return Ok("Lavado eliminado correctamente.");
            }
            catch (DbUpdateException)
            {
                return Conflict("No se puede eliminar este lavado porque está relacionado con otros registros.");
            }
        }
    }
}
