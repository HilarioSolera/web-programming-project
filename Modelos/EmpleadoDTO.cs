using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos.DTOs
{
    public class EmpleadoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El puesto es obligatorio.")]
        public string Puesto { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime? FechaIngreso { get; set; }

        [Range(0, 999999, ErrorMessage = "El salario debe estar entre ₡0 y ₡999,999.")]
        [Display(Name = "Salario por día")]
        public decimal? SalarioPorDia { get; set; }

        [Range(0, 365, ErrorMessage = "Los días de vacaciones deben estar entre 0 y 365.")]
        public int? DiasVacaciones { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de retiro")]
        public DateTime? FechaRetiro { get; set; }

        [Range(0, 9999999, ErrorMessage = "El monto debe estar entre ₡0 y ₡9,999,999.")]
        [Display(Name = "Monto de liquidación")]
        public decimal? MontoLiquidacion { get; set; }
    }
}