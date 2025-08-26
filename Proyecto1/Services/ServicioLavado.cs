using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Modelos;
using Modelos.DTOs;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Services.Implementations
{
    public class ServicioLavado : IServicioLavado
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://localhost:5188/api/lavado_vehiculos";
        private static readonly JsonSerializerOptions _opcionesJson = new(JsonSerializerDefaults.Web);

        public ServicioLavado(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Lavado>> ObtenerTodosAsync(string? filtro = null)
        {
            try
            {
                var endpoint = string.IsNullOrWhiteSpace(filtro)
                    ? $"{BaseUrl}/buscar"
                    : $"{BaseUrl}/buscar?filtro={Uri.EscapeDataString(filtro)}";

                var dtos = await _http.GetFromJsonAsync<List<LavadoDTO>>(endpoint, _opcionesJson);
                return dtos?.Select(MapearDesdeDTO).ToList() ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener lavados: {ex.Message}");
                return new();
            }
        }

        public async Task<Lavado?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"{BaseUrl}/buscar/{id}");
                if (!response.IsSuccessStatusCode) return null;

                var dto = await response.Content.ReadFromJsonAsync<LavadoDTO>(_opcionesJson);
                return dto is null ? null : MapearDesdeDTO(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener lavado por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool Exito, string Mensaje)> AgregarAsync(Lavado lavado)
        {
            return await EnviarJsonAsync(HttpMethod.Post, $"{BaseUrl}/agregar", lavado);
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Lavado lavado)
        {
            lavado.Id = id;
            return await EnviarJsonAsync(HttpMethod.Put, $"{BaseUrl}/actualizar/{id}", lavado);
        }

        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{BaseUrl}/eliminar/{id}");

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => (true, string.Empty),
                    HttpStatusCode.NotFound or HttpStatusCode.Conflict => (false, string.Empty),
                    _ => (false, $"Error {response.StatusCode}")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar lavado: {ex.Message}");
                return (false, string.Empty);
            }
        }

        // 🧠 Utilidad para enviar objetos como JSON
        private async Task<(bool, string)> EnviarJsonAsync(HttpMethod metodo, string url, Lavado lavado)
        {
            try
            {
                var json = JsonSerializer.Serialize(lavado, _opcionesJson);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(metodo, url) { Content = content };
                var response = await _http.SendAsync(request);

                return (response.IsSuccessStatusCode, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error HTTP ({metodo}) en {url}: {ex.Message}");
                return (false, string.Empty);
            }
        }

        private static Lavado MapearDesdeDTO(LavadoDTO dto) => new()
        {
            Id = dto.Id,
            Fecha = dto.Fecha,
            TipoLavado = dto.TipoLavado,
            Precio = dto.Precio,
            Estado = dto.Estado,
            ClienteId = dto.ClienteId,
            VehiculoId = dto.VehiculoId,
            EmpleadoId = dto.EmpleadoId
        };
    }
}
