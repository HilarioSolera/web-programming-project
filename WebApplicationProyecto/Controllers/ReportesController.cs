using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Modelos.DTOs;
using WebApplicationProyecto.Data;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly DBcontexto _db;

    public ReportesController(DBcontexto db)
    {
        _db = db;
    }

    [HttpGet("proyeccion")]
    public ActionResult<List<ClienteVehiculoProyeccionDTO>> Proyeccion()
    {
        var hoy = DateTime.Today;

        var resultado = _db.Clientes
            .SelectMany(cliente => cliente.Vehiculos.Select(vehiculo =>
                new ClienteVehiculoProyeccionDTO
                {
                    Identificacion = cliente.Identificacion,
                    NombreCompleto = cliente.NombreCompleto,
                    Correo = cliente.Correo,
                    VehiculoPlaca = vehiculo.Placa,
                    VehiculoDescripcion = $"{vehiculo.Marca} {vehiculo.Modelo}".Trim(),
                    UltimoLavado = vehiculo.UltimaAtencion,
                    FechaSugerida = hoy.AddDays(vehiculo.UltimaAtencion.HasValue ? 30 : 7)
                }))
            .ToList();

        return Ok(resultado);
    }
}