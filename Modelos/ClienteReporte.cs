using System;

namespace Modelos
{
    public class ClienteReporte
    {
        public int Id { get; set; }                      // ID del cliente (de tabla Clientes)
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string PreferenciaLavado { get; set; } = string.Empty;
        public DateTime? UltimoLavado { get; set; }      // Puede ser null si no ha tenido
        public int DiasSinLavado { get; set; }           // Calculado desde la fecha actual
    }
}