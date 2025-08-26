using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTOs
{
    public class ClienteVehiculoProyeccionDTO
    {
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string VehiculoPlaca { get; set; } = string.Empty;
        public string VehiculoDescripcion { get; set; } = string.Empty;

        public DateTime? UltimoLavado { get; set; }
        public DateTime FechaSugerida { get; set; }
    }
}
