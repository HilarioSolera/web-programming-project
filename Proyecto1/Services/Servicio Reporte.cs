using Proyecto1.Services.Interfaces;
using Modelos.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ServicioReporte : IServicioReporte
{
    private readonly HttpClient _http;
    private const string URL = "http://localhost:5188/api/reportes/proyeccion";

    public ServicioReporte(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ClienteVehiculoProyeccionDTO>> ObtenerProyeccionLavadosAsync()
    {
        var resultado = await _http.GetFromJsonAsync<List<ClienteVehiculoProyeccionDTO>>(URL);
        return resultado ?? new List<ClienteVehiculoProyeccionDTO>();
    }
}