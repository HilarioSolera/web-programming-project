using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Modelos;
using Modelos.DTOs;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Services.Implementations
{
    public class ServicioEmpleado : IServicioEmpleado
    {
        private readonly HttpClient _http;
        private const string BASE_URL = "http://localhost:5188/api/empleados";

        public ServicioEmpleado(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Empleado>> ObtenerTodosAsync(string? filtro = null)
        {
            try
            {
                var url = string.IsNullOrWhiteSpace(filtro)
                    ? $"{BASE_URL}/buscar"
                    : $"{BASE_URL}/buscar?query={Uri.EscapeDataString(filtro)}";

                var dtos = await _http.GetFromJsonAsync<List<EmpleadoDTO>>(url);
                return dtos?.Select(MapearDesdeDTO).ToList() ?? new List<Empleado>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener empleados: {ex.Message}");
                return new List<Empleado>();
            }
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"{BASE_URL}/buscar/{id}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var dto = await response.Content.ReadFromJsonAsync<EmpleadoDTO>();
                return dto is null ? null : MapearDesdeDTO(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener empleado por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool Exito, string Mensaje)> AgregarAsync(Empleado empleado)
        {
            try
            {
                var dto = MapearADTO(empleado);
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync($"{BASE_URL}/agregar", content);
                string mensaje = "";

                if (response.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    var cuerpo = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    if (cuerpo != null && cuerpo.TryGetValue("mensaje", out var m))
                    {
                        mensaje = m?.ToString() ?? "";
                    }
                }

                if (string.IsNullOrWhiteSpace(mensaje))
                {
                    mensaje = await response.Content.ReadAsStringAsync(); // respaldo
                }

                return (response.IsSuccessStatusCode, mensaje);
            }
            catch (Exception ex)
            {
                return (false, $"❌ Error técnico al agregar empleado: {ex.Message}");
            }
        }



        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Empleado empleado)
        {
            try
            {
                empleado.Id = id;
                var dto = MapearADTO(empleado);
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync($"{BASE_URL}/actualizar/{id}", content);
                var cuerpo = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var mensaje = cuerpo?["mensaje"] ?? "";

                return (response.IsSuccessStatusCode, mensaje);
            }
            catch (Exception ex)
            {
                return (false, $"❌ Error técnico al actualizar empleado: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{BASE_URL}/eliminar/{id}");
                var cuerpo = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var mensaje = cuerpo?["mensaje"] ?? "";

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => (true, mensaje),
                    HttpStatusCode.Conflict => (false, mensaje),
                    HttpStatusCode.NotFound => (false, mensaje),
                    _ => (false, $"❌ Error inesperado: {mensaje}")
                };
            }
            catch (Exception ex)
            {
                return (false, $"❌ Error técnico al eliminar empleado: {ex.Message}");
            }
        }

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

        private static EmpleadoDTO MapearADTO(Empleado modelo) => new EmpleadoDTO
        {
            Id = modelo.Id,
            NombreCompleto = modelo.NombreCompleto,
            Identificacion = modelo.Identificacion,
            Puesto = modelo.Puesto,
            Telefono = modelo.Telefono,
            Correo = modelo.Correo,
            FechaNacimiento = modelo.FechaNacimiento,
            FechaIngreso = modelo.FechaIngreso,
            SalarioPorDia = modelo.SalarioPorDia,
            DiasVacaciones = modelo.DiasVacaciones,
            FechaRetiro = modelo.FechaRetiro,
            MontoLiquidacion = modelo.MontoLiquidacion
        };
    }
}