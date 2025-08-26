using Modelos;

namespace Proyecto1.Services.Interfaces
{
    /// <summary>
    /// Define las operaciones de negocio para la gestión de lavados de vehículos.
    /// </summary>
    public interface IServicioLavado
    {
        /// <summary>Obtiene todos los lavados registrados, con filtro opcional.</summary>
        /// <param name="filtro">Texto para búsqueda flexible (opcional)</param>
        Task<List<Lavado>> ObtenerTodosAsync(string? filtro = null);

        /// <summary>Obtiene el detalle completo de un lavado por su ID.</summary>
        /// <param name="id">ID único del lavado</param>
        Task<Lavado?> ObtenerPorIdAsync(int id);

        /// <summary>Registra un nuevo lavado en el sistema.</summary>
        /// <param name="lavado">Objeto completo con datos del lavado</param>
        Task<(bool Exito, string Mensaje)> AgregarAsync(Lavado lavado);

        /// <summary>Actualiza los datos de un lavado existente.</summary>
        /// <param name="id">ID del lavado a actualizar</param>
        /// <param name="lavado">Datos nuevos del lavado</param>
        Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Lavado lavado);

        /// <summary>Elimina un lavado si no está vinculado a otros registros.</summary>
        /// <param name="id">ID del lavado a eliminar</param>
        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);
    }
}