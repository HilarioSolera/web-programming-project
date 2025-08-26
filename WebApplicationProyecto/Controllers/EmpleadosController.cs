using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationProyecto.Data;
using Modelos;
using Modelos.DTOs;

namespace WebApplicationProyecto.Controllers
{
    [Route("api/empleados")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly DBcontexto _context;

        public EmpleadosController(DBcontexto context)
        {
            _context = context;
        }

        // 🔍 GET: api/empleados/buscar?query=valor
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<EmpleadoDTO>>> Buscar([FromQuery(Name = "query")] string? filtro)
        {
            try
            {
                var queryable = _context.Empleados.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    filtro = filtro.Trim();

                    queryable = queryable
                        .Where(e =>
                            EF.Functions.Like(e.Identificacion, $"%{filtro}%") ||
                            EF.Functions.Like(e.NombreCompleto, $"%{filtro}%"))
                        .Take(100); // Limita aunque haya filtro
                }
                else
                {
                    queryable = queryable
                        .OrderByDescending(e => e.FechaIngreso)
                        .Take(100);
                }

                var empleados = await queryable.ToListAsync();
                var dtos = empleados.Select(MapearADTO).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"❌ Error al filtrar empleados: {ex.Message}" });
            }
        }



        // 🔎 GET: api/empleados/buscar/{id}
        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<EmpleadoDTO>> BuscarPorId(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado is null)
                return NotFound(new { mensaje = $"❌ No se encontró el empleado con ID" });

            var dto = MapearADTO(empleado);
            return Ok(dto);
        }

        // ➕ POST: api/empleados/agregar

        // ➕ POST: api/empleados/agregar
        [HttpPost("agregar")]
        public async Task<ActionResult<object>> AgregarEmpleado([FromBody] EmpleadoDTO nuevo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => new
                    {
                        Campo = m.Key,
                        Mensajes = m.Value.Errors.Select(e => e.ErrorMessage)
                    });

                return BadRequest(new
                {
                    mensaje = "❌ Validación fallida.",
                    errores
                });
            }

            if (string.IsNullOrWhiteSpace(nuevo.Identificacion))
                return BadRequest(new { mensaje = "❌ La identificación no puede estar vacía." });

            bool yaExiste = await _context.Empleados
                .AnyAsync(e => e.Identificacion == nuevo.Identificacion);

            if (yaExiste)
                return Conflict(new { mensaje = "❌ Ya existe un empleado con esa identificación." });

            var entidad = MapearDesdeDTO(nuevo);

            await _context.Empleados.AddAsync(entidad);
            await _context.SaveChangesAsync();

            nuevo.Id = entidad.Id;

            return Ok(new
            {
                mensaje = "✅ El empleado fue creado correctamente.",
                id = nuevo.Id
            });
        }

        // 🔄 PUT: api/empleados/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] EmpleadoDTO actualizado)
        {
            if (id != actualizado.Id)
                return BadRequest(new { mensaje = $"❌ El ID ({id}) no coincide con el modelo recibido." });

            var existe = await _context.Empleados.AnyAsync(e => e.Id == id);
            if (!existe)
                return NotFound(new { mensaje = $"❌ No existe un empleado con ID {id}." });

            var entidad = MapearDesdeDTO(actualizado);
            _context.Entry(entidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "✅ El empleado fue actualizado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = $"❌ Error técnico al actualizar: {ex.Message}" });
            }
        }

        // 🗑️ DELETE: api/empleados/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado is null)
                return NotFound(new { mensaje = $"❌ No se encontró el empleado con ID {id}." });

            var tieneLavados = await _context.Lavados.AnyAsync(l => l.EmpleadoId == id);
            if (tieneLavados)
                return Conflict(new { mensaje = "❌ No se puede eliminar el empleado porque tiene lavados asignados." });

            _context.Empleados.Remove(empleado);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = $"✅ El empleado fue eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"❌ Error técnico al eliminar: {ex.Message}" });
            }
        }

        // 🔄 Mapeo: Empleado → DTO
        private static EmpleadoDTO MapearADTO(Empleado e) => new EmpleadoDTO
        {
            Id = e.Id,
            NombreCompleto = e.NombreCompleto ?? "",
            Identificacion = e.Identificacion ?? "",
            Puesto = e.Puesto ?? "",
            Telefono = e.Telefono ?? "",
            Correo = e.Correo ?? "",
            FechaNacimiento = e.FechaNacimiento,
            FechaIngreso = e.FechaIngreso,
            SalarioPorDia = e.SalarioPorDia,
            DiasVacaciones = e.DiasVacaciones,
            FechaRetiro = e.FechaRetiro,
            MontoLiquidacion = e.MontoLiquidacion
        };

        // 🔄 Mapeo: DTO → Empleado
        private static Empleado MapearDesdeDTO(EmpleadoDTO dto) => new Empleado
        {
            Id = dto.Id,
            NombreCompleto = dto.NombreCompleto,
            Identificacion = dto.Identificacion,
            Puesto = dto.Puesto,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            FechaNacimiento = dto.FechaNacimiento,
            FechaIngreso = dto.FechaIngreso,
            SalarioPorDia = dto.SalarioPorDia,
            DiasVacaciones = dto.DiasVacaciones,
            FechaRetiro = dto.FechaRetiro,
            MontoLiquidacion = dto.MontoLiquidacion
        };
    }
}