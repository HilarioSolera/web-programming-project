using Microsoft.AspNetCore.Mvc;
using Proyecto1.Services.Interfaces;
using Modelos.DTOs;

public class ReporteController : Controller
{
    private readonly IServicioReporte _servicio;

    public ReporteController(IServicioReporte servicio)
    {
        _servicio = servicio;
    }

    public async Task<IActionResult> Proyeccion()
    {
        // Obtener proyección ya transformada desde el servicio
        var proyeccion = await _servicio.ObtenerProyeccionLavadosAsync();

        // Validar tipo y ordenar por FechaSugerida ascendente
        var ordenados = proyeccion
            .OrderBy(p => p.FechaSugerida)
            .ToList();

        // Confirmar tipo explícito en la vista
        return View("Proyeccion", ordenados); // Modelo: List<ClienteVehiculoProyeccionDTO>
    }
}