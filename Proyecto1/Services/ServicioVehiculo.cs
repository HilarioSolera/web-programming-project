using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Modelos;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Services.Implementations
{
    public class ServicioVehiculo : IServicioVehiculo
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "http://localhost:5188/api/vehiculos";
        private readonly IServicioCliente servicioCliente;

        public ServicioVehiculo(HttpClient http, IServicioCliente servicioCliente)
        {
            _http = http;
            this.servicioCliente = servicioCliente;
        }

        public async Task<List<Vehiculo>> ObtenerTodosAsync()
        {
            try
            {
                var listaDto = await _http.GetFromJsonAsync<List<VehiculoDTO>>($"{_baseUrl}/buscar");
                if (listaDto is null) return new List<Vehiculo>();

                var tareas = listaDto.Select(MapearVehiculoDesdeDTO);
                return (await Task.WhenAll(tareas)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener vehículos: {ex.Message}");
                return new List<Vehiculo>();
            }
        }

        public async Task<Vehiculo?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var dto = await _http.GetFromJsonAsync<VehiculoDTO>($"{_baseUrl}/buscarId/{id}");
                return dto is null ? null : await MapearVehiculoDesdeDTO(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al buscar vehículo por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<Vehiculo?> ObtenerPorPlacaAsync(string placa)
        {
            try
            {
                var placaNormalizada = placa.Trim().ToUpper();
                var dto = await _http.GetFromJsonAsync<VehiculoDTO>($"{_baseUrl}/buscar/{Uri.EscapeDataString(placaNormalizada)}");
                return dto is null ? null : await MapearVehiculoDesdeDTO(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al buscar vehículo por placa: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ExistePlacaAsync(string placa)
        {
            try
            {
                var placaNormalizada = placa.Trim().ToUpper();
                var response = await _http.GetAsync($"{_baseUrl}/buscar/{Uri.EscapeDataString(placaNormalizada)}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al verificar existencia de placa: {ex.Message}");
                return false;
            }
        }

        public async Task<(bool Exito, string Mensaje)> AgregarAsync(Vehiculo vehiculo)
        {
            try
            {
                var dto = MapearDTODesdeVehiculo(vehiculo);
                var response = await _http.PostAsJsonAsync($"{_baseUrl}/agregar", dto);

                return (response.IsSuccessStatusCode, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al insertar vehículo: {ex.Message}");
                return (false, string.Empty);
            }
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Vehiculo modelo)
        {
            try
            {
                var dto = MapearDTODesdeVehiculo(modelo);
                var response = await _http.PutAsJsonAsync($"{_baseUrl}/actualizar/{id}", dto);

                return (response.IsSuccessStatusCode, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al editar vehículo: {ex.Message}");
                return (false, string.Empty);
            }
        }

        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_baseUrl}/eliminar/{id}");

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => (true, string.Empty),
                    HttpStatusCode.NotFound => (false, string.Empty),
                    HttpStatusCode.Conflict => (false, string.Empty),
                    _ => (false, string.Empty)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar vehículo: {ex.Message}");
                return (false, string.Empty);
            }
        }

        private async Task<Vehiculo> MapearVehiculoDesdeDTO(VehiculoDTO dto)
        {
            var cliente = (await servicioCliente.ObtenerTodosAsync())
                .FirstOrDefault(c => c.Id == dto.ClienteId);

            return new Vehiculo
            {
                Id = dto.Id,
                Placa = dto.Placa,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Color = dto.Color,
                Traccion = dto.Traccion,
                TratamientoNanoCeramico = dto.TratamientoNanoCeramico,
                Anio = dto.Anio,
                UltimaAtencion = dto.UltimaAtencion,
                ClienteId = dto.ClienteId,
                Cliente = cliente is not null
                    ? new Cliente
                    {
                        Id = cliente.Id,
                        Identificacion = cliente.Identificacion,
                        NombreCompleto = cliente.NombreCompleto
                    }
                    : null
            };
        }

        private VehiculoDTO MapearDTODesdeVehiculo(Vehiculo v) => new VehiculoDTO
        {
            Id = v.Id,
            Placa = v.Placa,
            Marca = v.Marca,
            Modelo = v.Modelo,
            Color = v.Color,
            Traccion = v.Traccion,
            TratamientoNanoCeramico = v.TratamientoNanoCeramico,
            Anio = v.Anio,
            UltimaAtencion = v.UltimaAtencion,
            ClienteId = v.ClienteId
        };
    }
}