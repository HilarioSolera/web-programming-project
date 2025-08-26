using Modelos;
using Modelos.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto1.Services.Interfaces
{
    public interface IServicioEmpleado
    {
        Task<List<Empleado>> ObtenerTodosAsync(string? filtro = null);

        /// <summary>
        /// Obtiene un empleado por su ID único.
        /// </summary>
        /// <param name="id">ID del empleado</param>
        Task<Empleado?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Registra un nuevo empleado.
        /// </summary>
        /// <param name="empleado">Datos completos del empleado</param>
        Task<(bool Exito, string Mensaje)> AgregarAsync(Empleado empleado);

        /// <summary>
        /// Actualiza un empleado existente por su ID.
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <param name="empleado">Datos actualizados del empleado</param>
        Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Empleado empleado);

        /// <summary>
        /// Elimina un empleado por su ID.
        /// </summary>
        /// <param name="id">ID del empleado</param>
        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);

    }
}