using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Modelos;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Services.Implementations
{
    public class ServicioCliente : IServicioCliente
    {
        private readonly HttpClient _http;
        private const string BASE_URL = "http://localhost:5188/api/clientes";

        public ServicioCliente(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync(string filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? $"{BASE_URL}/buscar"
                : $"{BASE_URL}/buscar/{Uri.EscapeDataString(filtro)}";

            try
            {
                var clientes = await _http.GetFromJsonAsync<List<Cliente>>(url);
                return clientes ?? new List<Cliente>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }

        public async Task<List<Cliente>> ObtenerTodosAsync() =>
            await ObtenerTodosAsync(string.Empty);

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"{BASE_URL}/buscar-id/{id}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var cliente = await response.Content.ReadFromJsonAsync<Cliente>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return cliente;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al buscar cliente por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool Exito, string Mensaje)> AgregarAsync(Cliente cliente)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{BASE_URL}/agregar", cliente);
                var mensaje = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? (true, string.Empty)
                    : (false, mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al agregar cliente: {ex.Message}");
                return (false, $"Error técnico: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Cliente cliente)
        {
            try
            {
                cliente.Id = id;
                var json = JsonSerializer.Serialize(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync($"{BASE_URL}/actualizar/{id}", content);
                var mensaje = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? (true, string.Empty)
                    : (false, mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar cliente: {ex.Message}");
                return (false, $"Error técnico: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{BASE_URL}/eliminar/{id}");
                var contenido = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => (true, string.Empty),
                    HttpStatusCode.Conflict => (false, contenido), // ← error referencial
                    HttpStatusCode.NotFound => (false, contenido),
                    _ => (false, contenido)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar cliente: {ex.Message}");
                return (false, $"Error técnico: {ex.Message}");
            }
        }

        public async Task<bool> ExistePorIdAsync(int id) =>
            await ObtenerPorIdAsync(id) != null;

        public async Task<bool> PingAsync()
        {
            try
            {
                var response = await _http.GetAsync($"{BASE_URL}/ping");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error de conectividad: {ex.Message}");
                return false;
            }
        }
    }
}