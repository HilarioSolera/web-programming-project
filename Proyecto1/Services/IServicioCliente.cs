using Modelos;

namespace Proyecto1.Services.Interfaces
{
    public interface IServicioCliente
    {
        // 🔍 Búsqueda con filtro opcional
        Task<List<Cliente>> ObtenerTodosAsync(string filtro);

        // 📋 Obtener todos sin filtro
        Task<List<Cliente>> ObtenerTodosAsync();

        // 🔎 Obtener detalle por ID
        Task<Cliente?> ObtenerPorIdAsync(int id);

        // ➕ Crear cliente
        Task<(bool Exito, string Mensaje)> AgregarAsync(Cliente cliente);

        // ✏️ Actualizar cliente
        Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, Cliente cliente);

        // 🗑️ Eliminar cliente
        Task<(bool Exito, string Mensaje)> EliminarAsync(int id);

        // 🔄 Validación opcional
        Task<bool> ExistePorIdAsync(int id);
    }
}