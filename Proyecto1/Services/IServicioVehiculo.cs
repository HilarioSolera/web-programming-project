using Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto1.Services.Interfaces
{
    public interface IServicioVehiculo
    {
        /// <summary>
        /// Retorna todos los vehículos registrados en el sistema.
        /// </summary>
        Task<List<Vehiculo>> ObtenerTodosAsync();

        /// <summary>
        /// Busca un vehículo por su placa única.
        /// </summary>
        Task<Vehiculo?> ObtenerPorPlacaAsync(string placa);

        /// <summary>
        /// Verifica si existe un vehículo con la placa especificada.
        /// </summary>
        Task<bool> ExistePlacaAsync(string placa);

        /// <summary>
        /// Agrega un nuevo vehículo al sistema.
        /// </summary>
        Task<(bool Exito, string Mensaje)> AgregarAsync(Vehiculo vehiculo);

        /// <summary>
        /// Actualiza los datos de un vehículo por su placa.
        /// </summary>
        Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Vehiculo vehiculo);

        /// <summary>
        /// Elimina un vehículo por su placa.
        /// </summary>
        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);

        /// <summary>
        /// Busca un vehículo por su Id único.
        /// </summary>
        Task<Vehiculo?> ObtenerPorIdAsync(int id);

       
    }
}