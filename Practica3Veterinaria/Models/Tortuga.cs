namespace Practica3Veterinaria.Models;

public sealed class Tortuga : Mascota
{
    private const double FactorAjuste = 0.80;
    private string _tipoCaparazon;
    private bool _esAcuatica;

    public Tortuga(
        string nombre,
        double pesoKg,
        SexoMascota sexo,
        int edad,
        string propietario,
        EstadoPaciente estado,
        string tipoCaparazon,
        bool esAcuatica)
        : base(nombre, pesoKg, sexo, edad, propietario, estado)
    {
        _tipoCaparazon = string.Empty;
        TipoCaparazon = tipoCaparazon;
        EsAcuatica = esAcuatica;
    }

    public string TipoCaparazon
    {
        get => _tipoCaparazon;
        set => _tipoCaparazon = ValidarTexto(value, nameof(TipoCaparazon));
    }

    public bool EsAcuatica
    {
        get => _esAcuatica;
        set => _esAcuatica = value;
    }

    // El factor original queda visible para la calculadora Razor.
    public override double FactorDosis => 0.80;

    public override double CalcularDosis(double dosisPorKg)
    {
        ValidarDosisPorKg(dosisPorKg);
        return PesoKg * dosisPorKg * FactorAjuste;
    }

    public override string MostrarInformacion()
    {
        return base.MostrarInformacion() + Environment.NewLine +
               $"Caparazón:    {TipoCaparazon}{Environment.NewLine}" +
               $"Es acuática:  {(EsAcuatica ? "Sí" : "No")}";
    }
}
