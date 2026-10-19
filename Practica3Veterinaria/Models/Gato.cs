namespace Practica3Veterinaria.Models;

public sealed class Gato : Mascota
{
    private const double FactorAjuste = 0.90;
    private string _raza;
    private bool _esterilizado;

    public Gato(
        string nombre,
        double pesoKg,
        SexoMascota sexo,
        int edad,
        string propietario,
        EstadoPaciente estado,
        string raza,
        bool esterilizado)
        : base(nombre, pesoKg, sexo, edad, propietario, estado)
    {
        _raza = string.Empty;
        Raza = raza;
        Esterilizado = esterilizado;
    }

    public string Raza
    {
        get => _raza;
        set => _raza = ValidarTexto(value, nameof(Raza));
    }

    public bool Esterilizado
    {
        get => _esterilizado;
        set => _esterilizado = value;
    }

    // El factor original queda visible para la calculadora Razor.
    public override double FactorDosis => 0.90;

    public override double CalcularDosis(double dosisPorKg)
    {
        ValidarDosisPorKg(dosisPorKg);
        return PesoKg * dosisPorKg * FactorAjuste;
    }

    public override string MostrarInformacion()
    {
        return base.MostrarInformacion() + Environment.NewLine +
               $"Raza:        {Raza}{Environment.NewLine}" +
               $"Esterilizado:{(Esterilizado ? " Sí" : " No")}";
    }
}
