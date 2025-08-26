using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Modelos
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(15, ErrorMessage = "La placa no debe exceder los 15 caracteres.")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "La placa solo puede contener letras, Mayúsculas y números sin espacios.")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(30, ErrorMessage = "La marca no debe exceder los 30 caracteres.")]
        [RegularExpression(@"\S+", ErrorMessage = "La marca no puede estar vacía ni contener solo espacios.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El modelo no debe exceder los 30 caracteres.")]
        [RegularExpression(@"\S+", ErrorMessage = "El modelo no puede estar vacío ni contener solo espacios.")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio.")]
        [StringLength(20, ErrorMessage = "El color no debe exceder los 20 caracteres.")]
        [RegularExpression(@"\S+", ErrorMessage = "El color no puede estar vacío ni contener solo espacios.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes seleccionar el tipo de tracción.")]
        [StringLength(20, ErrorMessage = "La tracción no debe exceder los 20 caracteres.")]
        [RegularExpression(@"^(4x2|4x4|AWD)$", ErrorMessage = "La tracción debe ser 4x2, 4x4 o AWD.")]
        public string Traccion { get; set; } = string.Empty;

        // ✅ No usar [Required] en bool. El valor false ya es válido.
        [Display(Name = "Tratamiento nano cerámico aplicado")]
        public bool TratamientoNanoCeramico { get; set; }


        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
        public int? Anio { get; set; }

        [Required(ErrorMessage = "La fecha de última atención es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Última atención")]
        public DateTime? UltimaAtencion { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un cliente.")]
        [Range(1, int.MaxValue, ErrorMessage = "El cliente seleccionado no es válido.")]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        [ValidateNever]
        public Cliente? Cliente { get; set; }

        [ValidateNever]
        public ICollection<Lavado> Lavados { get; set; } = new List<Lavado>();
    }
}
