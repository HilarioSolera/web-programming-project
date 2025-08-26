using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos
{
    public class Empleado
    {
        // 📌 Identificación
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "La identificación no debe superar los 20 caracteres.")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no debe superar los 100 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        // 📱 Contacto
        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no debe superar los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        // 🧳 Información laboral
        [Required(ErrorMessage = "El puesto es obligatorio.")]
        [StringLength(50, ErrorMessage = "El puesto no debe superar los 50 caracteres.")]
        [Display(Name = "Puesto")]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime? FechaIngreso { get; set; }

        [Required(ErrorMessage = "El salario por día es obligatorio.")]
        [Range(0, 999999, ErrorMessage = "El salario debe estar entre ₡0 y ₡999,999.")]
        [Display(Name = "Salario por día")]
        public decimal? SalarioPorDia { get; set; }

        [Required(ErrorMessage = "Los días de vacaciones son obligatorios.")]
        [Range(0, 365, ErrorMessage = "Los días de vacaciones deben estar entre 0 y 365.")]
        [Display(Name = "Días de vacaciones")]
        public int? DiasVacaciones { get; set; }

        // Estos son opcionales
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de retiro")]
        public DateTime? FechaRetiro { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser positivo.")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Monto de liquidación")]
        public decimal? MontoLiquidacion { get; set; }

        // 🧼 Relación con lavados
        public List<Lavado> Lavados { get; set; } = new();

        [NotMapped]
        [Display(Name = "Lavados atendidos")]
        public int LavadosAtendidos { get; set; }

        // 📊 Campo calculado
        [NotMapped]
        [Display(Name = "Salario mensual estimado")]
        public decimal? SalarioMensualEstimado =>
            SalarioPorDia.HasValue ? SalarioPorDia * 30 : null;
    }
}
