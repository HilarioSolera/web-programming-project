using System.ComponentModel.DataAnnotations;

public class VehiculoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria"), MaxLength(15)]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es obligatoria"), MaxLength(30)]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "El modelo es obligatorio"), MaxLength(30)]
    public string Modelo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El color es obligatorio"), MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "El año debe ser válido")]
    [Required(ErrorMessage = "El año es obligatorio")]
     public int? Anio { get; set; }
    [MaxLength(20)]
    public string Traccion { get; set; } = string.Empty;

   public bool TratamientoNanoCeramico { get; set; }

    public DateTime? UltimaAtencion { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el cliente")]
    public int ClienteId { get; set; }
   

}