using System.ComponentModel.DataAnnotations;

namespace Practica3Veterinaria.ViewModels;

public sealed class DosisViewModel
{
    [Required]
    public string Codigo { get; set; } = "";

    [Range(0.000001, 1000000.0, ErrorMessage = "La dosis debe ser mayor que 0 y no superar 1,000,000 mg/kg.")]
    public double DosisPorKg { get; set; }

    public string Nombre { get; set; } = "";
    public string Especie { get; set; } = "";
    public double Peso { get; set; }
    public double Factor { get; set; }
    public double? ResultadoMg { get; set; }
}
