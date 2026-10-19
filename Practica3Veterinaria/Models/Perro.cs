namespace Practica3Veterinaria.Models;

public sealed class Perro : Mascota
{
    private string _raza;
    private string _tamano;

    public Perro(
        string nombre,
        double pesoKg,
        SexoMascota sexo,
        int edad,
        string propietario,
        EstadoPaciente estado,
        string raza,
        string tamano)
        : base(nombre, pesoKg, sexo, edad, propietario, estado)
    {
        _raza = string.Empty;
        _tamano = string.Empty;
        Raza = raza;
        Tamano = tamano;
    }

    public string Raza
    {
        get => _raza;
        set => _raza = ValidarTexto(value, nameof(Raza));
    }

    public string Tamano
    {
        get => _tamano;
        set => _tamano = ValidarTexto(value, nameof(Tamano));
    }

    // El factor original queda visible para la calculadora Razor.
    public override double FactorDosis => 1.00;

    public override double CalcularDosis(double dosisPorKg)
    {
        ValidarDosisPorKg(dosisPorKg);
        return PesoKg * dosisPorKg;
    }

    public override string MostrarInformacion()
    {
        return base.MostrarInformacion() + Environment.NewLine +
               $"Raza:        {Raza}{Environment.NewLine}" +
               $"Tamaño:      {Tamano}";
    }
}
