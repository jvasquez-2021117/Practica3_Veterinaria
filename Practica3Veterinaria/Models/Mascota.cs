using System.Globalization;

namespace Practica3Veterinaria.Models;


public abstract class Mascota
{

    private string _nombre;
    private double _pesoKg;
    private SexoMascota _sexo;
    private int _edad;
    private string _propietario;
    private EstadoPaciente _estado;

    protected Mascota(
        string nombre,
        double pesoKg,
        SexoMascota sexo,
        int edad,
        string propietario,
        EstadoPaciente estado)
    {
        _nombre = string.Empty;
        _propietario = string.Empty;

        Nombre = nombre;
        PesoKg = pesoKg;
        Sexo = sexo;
        Edad = edad;
        Propietario = propietario;
        Estado = estado;
        CodigoUnico = GenerarCodigoUnico();
    }

    public string Nombre
    {
        get => _nombre;
        set => _nombre = ValidarTexto(value, nameof(Nombre));
    }

    public double PesoKg
    {
        get => _pesoKg;
        set
        {
            if (!double.IsFinite(value) || value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(PesoKg), "El peso debe ser mayor que 0 kg.");
            }

            _pesoKg = value;
        }
    }

    public SexoMascota Sexo
    {
        get => _sexo;
        set
        {
            if (!Enum.IsDefined(typeof(SexoMascota), value))
            {
                throw new ArgumentOutOfRangeException(nameof(Sexo), "El sexo indicado no es válido.");
            }

            _sexo = value;
        }
    }

    public int Edad
    {
        get => _edad;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Edad), "La edad no puede ser negativa.");
            }

            _edad = value;
        }
    }

    public string Propietario
    {
        get => _propietario;
        set => _propietario = ValidarTexto(value, nameof(Propietario));
    }


    public string CodigoUnico { get; }

    // Propiedades de solo lectura agregadas para presentar el modelo original en Razor.
    // Los atributos, propiedades originales y métodos de la Práctica 1 permanecen intactos.
    public string Codigo => CodigoUnico;
    public double Peso => PesoKg;
    public string Especie => GetType().Name;
    public string EstadoTexto => Estado.ToString();
    public bool EstaEnferma => Estado == EstadoPaciente.Enfermo;
    public abstract double FactorDosis { get; }

    public EstadoPaciente Estado
    {
        get => _estado;
        private set
        {
            if (!Enum.IsDefined(typeof(EstadoPaciente), value))
            {
                throw new ArgumentOutOfRangeException(nameof(Estado), "El estado indicado no es válido.");
            }

            _estado = value;
        }
    }

    public abstract double CalcularDosis(double dosisPorKg);


    public void CambiarEstado()
    {
        Estado = Estado == EstadoPaciente.Sano
            ? EstadoPaciente.Enfermo
            : EstadoPaciente.Sano;
    }


    public virtual string MostrarInformacion()
    {
        return string.Join(Environment.NewLine,
            $"Código:      {CodigoUnico}",
            $"Tipo:        {GetType().Name}",
            $"Nombre:      {Nombre}",
            $"Peso:        {PesoKg.ToString("0.##", CultureInfo.InvariantCulture)} kg",
            $"Sexo:        {Sexo}",
            $"Edad:        {Edad}",
            $"Propietario: {Propietario}",
            $"Estado:      {Estado}");
    }

    protected static void ValidarDosisPorKg(double dosisPorKg)
    {
        if (!double.IsFinite(dosisPorKg) || dosisPorKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dosisPorKg), "La dosis por kg debe ser mayor que 0 mg/kg.");
        }
    }

    protected static string ValidarTexto(string value, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{nombreCampo} no puede estar vacío.", nombreCampo);
        }

        return value.Trim();
    }

    private static string GenerarCodigoUnico()
    {
        // La validación de unicidad se realiza contra la lista propia del servicio.
        // No se usa HashSet, List ni otras colecciones como almacenamiento.
        return Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
    }
}
