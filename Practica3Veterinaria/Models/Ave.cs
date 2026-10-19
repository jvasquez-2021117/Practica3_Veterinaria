namespace Practica3Veterinaria.Models;

public sealed class Ave : Mascota
{
    private const double FactorAjuste = 0.50;
    private double _envergaduraAlasCm;
    private bool _puedeVolar;

    public Ave(
        string nombre,
        double pesoKg,
        SexoMascota sexo,
        int edad,
        string propietario,
        EstadoPaciente estado,
        double envergaduraAlasCm,
        bool puedeVolar)
        : base(nombre, pesoKg, sexo, edad, propietario, estado)
    {
        EnvergaduraAlasCm = envergaduraAlasCm;
        PuedeVolar = puedeVolar;
    }

    public double EnvergaduraAlasCm
    {
        get => _envergaduraAlasCm;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(EnvergaduraAlasCm), "La envergadura debe ser mayor que 0 cm.");
            }

            _envergaduraAlasCm = value;
        }
    }

    public bool PuedeVolar
    {
        get => _puedeVolar;
        set => _puedeVolar = value;
    }

    // El factor original queda visible para la calculadora Razor.
    public override double FactorDosis => 0.50;

    public override double CalcularDosis(double dosisPorKg)
    {
        ValidarDosisPorKg(dosisPorKg);
        return PesoKg * dosisPorKg * FactorAjuste;
    }

    public override string MostrarInformacion()
    {
        return base.MostrarInformacion() + Environment.NewLine +
               $"Envergadura:  {EnvergaduraAlasCm:0.##} cm{Environment.NewLine}" +
               $"Puede volar:  {(PuedeVolar ? "Sí" : "No")}";
    }
}
