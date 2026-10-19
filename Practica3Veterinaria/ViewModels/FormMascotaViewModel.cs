using System.ComponentModel.DataAnnotations;
using Practica3Veterinaria.Models;

namespace Practica3Veterinaria.ViewModels;

/// <summary>Datos del formulario, separados del dominio y de los datos almacenados.</summary>
public sealed class FormMascotaViewModel : IValidatableObject
{
    public string Codigo { get; set; } = "";

    [Required(ErrorMessage = "Indica el estado del paciente.")]
    [RegularExpression("^(Sano|Enfermo)$", ErrorMessage = "Estado no válido.")]
    public string Estado { get; set; } = "Sano";

    [Range(1, 4, ErrorMessage = "Selecciona una especie.")]
    public int Tipo { get; set; }

    [Required(ErrorMessage = "Ingresa el nombre.")]
    [StringLength(80, ErrorMessage = "Máximo 80 caracteres.")]
    public string Nombre { get; set; } = "";

    [Range(0.01, 5000.0, ErrorMessage = "El peso debe estar entre 0.01 y 5000 kg.")]
    public double Peso { get; set; }

    [Required(ErrorMessage = "Selecciona el sexo.")]
    [RegularExpression("^(Macho|Hembra)$", ErrorMessage = "Sexo inválido.")]
    public string Sexo { get; set; } = "";

    [Range(0, 250, ErrorMessage = "La edad debe estar entre 0 y 250 años.")]
    public int Edad { get; set; }

    [Required(ErrorMessage = "Ingresa el propietario.")]
    [StringLength(80, ErrorMessage = "Máximo 80 caracteres.")]
    public string Propietario { get; set; } = "";

    [StringLength(60, ErrorMessage = "Máximo 60 caracteres.")]
    public string? Raza { get; set; }

    [StringLength(30, ErrorMessage = "Máximo 30 caracteres.")]
    public string? Tamano { get; set; }

    public bool Esterilizado { get; set; }

    public double? EnvergaduraAlasCm { get; set; }

    public bool PuedeVolar { get; set; }

    [StringLength(60, ErrorMessage = "Máximo 60 caracteres.")]
    public string? TipoCaparazon { get; set; }

    public bool EsAcuatica { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ((Tipo == 1 || Tipo == 2) && string.IsNullOrWhiteSpace(Raza))
            yield return new ValidationResult("Ingresa la raza.", new[] { nameof(Raza) });

        if (Tipo == 1 && string.IsNullOrWhiteSpace(Tamano))
            yield return new ValidationResult("Ingresa el tamaño.", new[] { nameof(Tamano) });

        if (Tipo == 3 && (EnvergaduraAlasCm is null || !double.IsFinite(EnvergaduraAlasCm.Value)
                          || EnvergaduraAlasCm.Value <= 0 || EnvergaduraAlasCm.Value > 10000))
            yield return new ValidationResult("La envergadura debe ser mayor que 0 y menor o igual a 10000 cm.",
                new[] { nameof(EnvergaduraAlasCm) });

        if (Tipo == 4 && string.IsNullOrWhiteSpace(TipoCaparazon))
            yield return new ValidationResult("Ingresa el tipo de caparazón.", new[] { nameof(TipoCaparazon) });

        if (!double.IsFinite(Peso))
            yield return new ValidationResult("El peso debe ser un número válido.", new[] { nameof(Peso) });
    }

    public static FormMascotaViewModel DesdeMascota(Mascota mascota)
    {
        var datos = new FormMascotaViewModel
        {
            Codigo = mascota.CodigoUnico,
            Nombre = mascota.Nombre,
            Peso = mascota.PesoKg,
            Sexo = mascota.Sexo.ToString(),
            Estado = mascota.Estado.ToString(),
            Edad = mascota.Edad,
            Propietario = mascota.Propietario
        };

        switch (mascota)
        {
            case Perro perro:
                datos.Tipo = 1;
                datos.Raza = perro.Raza;
                datos.Tamano = perro.Tamano;
                break;
            case Gato gato:
                datos.Tipo = 2;
                datos.Raza = gato.Raza;
                datos.Esterilizado = gato.Esterilizado;
                break;
            case Ave ave:
                datos.Tipo = 3;
                datos.EnvergaduraAlasCm = ave.EnvergaduraAlasCm;
                datos.PuedeVolar = ave.PuedeVolar;
                break;
            case Tortuga tortuga:
                datos.Tipo = 4;
                datos.TipoCaparazon = tortuga.TipoCaparazon;
                datos.EsAcuatica = tortuga.EsAcuatica;
                break;
        }
        return datos;
    }
}
