using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "La identificación no debe superar los 20 caracteres.")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no debe superar los 100 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        [StringLength(50, ErrorMessage = "La provincia no debe superar los 50 caracteres.")]
        [Display(Name = "Provincia")]
        public string Provincia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cantón es obligatorio.")]
        [StringLength(50, ErrorMessage = "El cantón no debe superar los 50 caracteres.")]
        [Display(Name = "Cantón")]
        public string Canton { get; set; } = string.Empty;

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        [StringLength(50, ErrorMessage = "El distrito no debe superar los 50 caracteres.")]
        [Display(Name = "Distrito")]
        public string Distrito { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección exacta es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección exacta no debe superar los 200 caracteres.")]
        [Display(Name = "Dirección exacta")]
        public string DireccionExacta { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no debe superar los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La preferencia de lavado es obligatoria.")]
        [StringLength(50, ErrorMessage = "La preferencia de lavado no debe superar los 50 caracteres.")]
        [Display(Name = "Preferencia de lavado")]
        public string PreferenciaLavado { get; set; } = string.Empty;

        // Relaciones de navegación
        public List<Vehiculo> Vehiculos { get; set; } = new();
        public List<Lavado> Lavados { get; set; } = new();
    }
}
