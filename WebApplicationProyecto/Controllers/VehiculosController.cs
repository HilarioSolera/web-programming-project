using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationProyecto.Data;
using Modelos;

namespace WebApplicationProyecto.Controllers
{
    [Route("api/vehiculos")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly DBcontexto _context;

        public VehiculosController(DBcontexto context)
        {
            _context = context;
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> Buscar()
        {
            try
            {
                var vehiculos = await _context.Vehiculos
                    .Include(v => v.Cliente)
                    .Select(v => new VehiculoDTO
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Marca = v.Marca,
                        Modelo = v.Modelo,
                        Color = v.Color,
                        Traccion = v.Traccion,
                        ClienteId = v.ClienteId,
                        Anio = v.Anio ?? 0,
                        UltimaAtencion = v.UltimaAtencion,
                        TratamientoNanoCeramico = v.TratamientoNanoCeramico,
                       
                    })
                    .ToListAsync();

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar vehículos: {ex.Message}");
                return StatusCode(500, "Error al obtener los vehículos.");
            }
        }

        [HttpGet("buscarId/{id}")]
        public async Task<ActionResult<VehiculoDTO>> BuscarPorId(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehiculo is null)
                return NotFound(new { mensaje = "Vehículo no encontrado por Id." });

            var dto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Color = vehiculo.Color,
                Traccion = vehiculo.Traccion,
                ClienteId = vehiculo.ClienteId,
                Anio = vehiculo.Anio ?? 0,
                UltimaAtencion = vehiculo.UltimaAtencion,
                TratamientoNanoCeramico = vehiculo.TratamientoNanoCeramico,
               
            };

            return Ok(dto);
        }

        [HttpGet("buscar/{placa}")]
        public async Task<ActionResult<VehiculoDTO>> BuscarPorPlaca(string placa)
        {
            var placaNormalizada = placa?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(placaNormalizada))
                return BadRequest(new { mensaje = "La placa proporcionada es inválida." });

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Placa == placaNormalizada);

            if (vehiculo == null)
                return NotFound(new { mensaje = "Vehículo no encontrado." });

            var dto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Color = vehiculo.Color,
                Traccion = vehiculo.Traccion,
                ClienteId = vehiculo.ClienteId,
                Anio = vehiculo.Anio ?? 0,
                UltimaAtencion = vehiculo.UltimaAtencion,
                TratamientoNanoCeramico = vehiculo.TratamientoNanoCeramico,
                
            };

            return Ok(dto);
        }

        [HttpPost("agregar")]
        public async Task<ActionResult<VehiculoDTO>> AgregarVehiculo(VehiculoDTO nuevoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var placaNormalizada = nuevoDto.Placa?.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(placaNormalizada))
                return BadRequest(new { mensaje = "La placa proporcionada es inválida." });

            bool placaExistente = await _context.Vehiculos.AnyAsync(v => v.Placa == placaNormalizada);
            if (placaExistente)
                return Conflict(new { mensaje = "Ya existe un vehículo con esa placa." });

            bool clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == nuevoDto.ClienteId);
            if (!clienteExiste)
                return NotFound(new { mensaje = "El cliente asociado no existe." });

            var nuevo = new Vehiculo
            {
                Placa = placaNormalizada,
                Marca = nuevoDto.Marca?.Trim(),
                Modelo = nuevoDto.Modelo?.Trim(),
                Color = nuevoDto.Color?.Trim(),
                Traccion = nuevoDto.Traccion?.Trim(),
                ClienteId = nuevoDto.ClienteId,
                Anio = nuevoDto.Anio,
                UltimaAtencion = nuevoDto.UltimaAtencion,
                TratamientoNanoCeramico = nuevoDto.TratamientoNanoCeramico
            };

            await _context.Vehiculos.AddAsync(nuevo);
            await _context.SaveChangesAsync();

            nuevoDto.Id = nuevo.Id;
            nuevoDto.Placa = nuevo.Placa;

            return CreatedAtAction(nameof(BuscarPorPlaca), new { placa = nuevo.Placa }, nuevoDto);
        }

        [HttpPut("editar/{placa}")]
        public async Task<IActionResult> ActualizarVehiculo(string placa, VehiculoDTO actualizadoDto)
        {
            var placaNormalizada = placa?.Trim().ToUpper();
            if (placaNormalizada != actualizadoDto.Placa?.Trim().ToUpper())
                return BadRequest(new { mensaje = "La placa en la URL no coincide con el DTO." });

            var existente = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placaNormalizada);
            if (existente == null)
                return NotFound(new { mensaje = "Vehículo no encontrado." });

            bool clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == actualizadoDto.ClienteId);
            if (!clienteExiste)
                return NotFound(new { mensaje = "Cliente asociado no válido." });

            existente.Marca = actualizadoDto.Marca?.Trim();
            existente.Modelo = actualizadoDto.Modelo?.Trim();
            existente.Color = actualizadoDto.Color?.Trim();
            existente.Traccion = actualizadoDto.Traccion?.Trim();
            existente.ClienteId = actualizadoDto.ClienteId;
            existente.Anio = actualizadoDto.Anio;
            existente.UltimaAtencion = actualizadoDto.UltimaAtencion;
            existente.TratamientoNanoCeramico = actualizadoDto.TratamientoNanoCeramico;


            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Vehículo actualizado correctamente." });
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarPorId(int id, VehiculoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El Id de la URL no coincide con el DTO." });

            var existente = await _context.Vehiculos.FindAsync(id);
            if (existente is null)
                return NotFound(new { mensaje = "Vehículo no encontrado." });

            bool clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
                return NotFound(new { mensaje = "Cliente asociado no válido." });

            existente.Placa = dto.Placa?.Trim().ToUpper();
            existente.Marca = dto.Marca?.Trim();
            existente.Modelo = dto.Modelo?.Trim();
            existente.Color = dto.Color?.Trim();
            existente.Traccion = dto.Traccion?.Trim();
            existente.ClienteId = dto.ClienteId;
            existente.Anio = dto.Anio;
            existente.UltimaAtencion = dto.UltimaAtencion;
            existente.TratamientoNanoCeramico = dto.TratamientoNanoCeramico;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Vehículo actualizado correctamente por Id." });
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return NotFound(new { mensaje = "Vehículo no encontrado." });

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Vehículo eliminado correctamente." });
        }
    }
}