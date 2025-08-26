using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelos.DTOs
{
    public class LavadoDTO : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha del lavado es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar un tipo de lavado.")]
        public string TipoLavado { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado del lavado.")]
        [RegularExpression(@"En proceso|Facturado|Agendado", ErrorMessage = "El estado debe ser válido.")]
        public string Estado { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido.")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido.")]
        public int VehiculoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado válido.")]
        public int? EmpleadoId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (TipoLavado == "La Joya" && (!Precio.HasValue || Precio <= 0))
            {
                yield return new ValidationResult("Debe ingresar el precio para 'La Joya'.", new[] { nameof(Precio) });
            }
        }
    }
}
