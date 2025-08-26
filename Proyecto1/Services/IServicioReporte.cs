using Modelos;
using Modelos.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto1.Services.Interfaces
{
    public interface IServicioReporte
    {
        /// <summary>
        /// Obtiene la lista de clientes que deben ser contactados para agendar lavados.
        /// </summary>
        /// <returns>Lista de proyecciones con información de contacto y estado del último lavado.</returns>
        Task<List<ClienteVehiculoProyeccionDTO>> ObtenerProyeccionLavadosAsync();
    }
}