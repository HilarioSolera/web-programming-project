using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Lavado : IValidatableObject
    {
        public int Id { get; set; }

        // Información base
        [Required(ErrorMessage = "La fecha del lavado es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar un tipo de lavado.")]
        public string TipoLavado { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido.")]
        [RegularExpression(@"En proceso|Facturado|Agendado", ErrorMessage = "El estado debe ser 'En proceso', 'Facturado' o 'Agendado'.")]
        public string? Estado { get; set; }

        // Relación con entidades
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido.")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido.")]
        public int VehiculoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado válido.")]
        public int? EmpleadoId { get; set; }

        // Navegación
        public virtual Cliente? Cliente { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
        public virtual Empleado? Empleado { get; set; }

        // Validación personalizada
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (TipoLavado == "La Joya" && (!Precio.HasValue || Precio <= 0))
            {
                yield return new ValidationResult("Debe ingresar el precio para el tipo de lavado 'La Joya'.", new[] { nameof(Precio) });
            }
        }

        // Cálculo automático
        public decimal PrecioConIVA => Precio.HasValue ? Math.Round(Precio.Value * 1.13m, 2) : 0;
    }
}