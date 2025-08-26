using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos.DTOs
{
    public class clienteProyeccionDTO
    {
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Canton { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string DireccionExacta { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PreferenciaLavado { get; set; } = string.Empty;

        public string VehiculoPlaca { get; set; } = string.Empty;
        public string VehiculoDescripcion { get; set; } = string.Empty;
        public DateTime? UltimoLavado { get; set; } // ← solo para vista
        public DateTime FechaSugerida { get; set; } // ← calculado para vista
    }
}
