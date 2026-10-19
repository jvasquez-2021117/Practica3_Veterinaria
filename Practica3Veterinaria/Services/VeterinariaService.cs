using Practica3Veterinaria.Estructuras;
using Practica3Veterinaria.Models;
using Practica3Veterinaria.ViewModels;

namespace Practica3Veterinaria.Services;

/// <summary>
/// Adaptación del VeterinariaService de la Práctica 1.
/// El List&lt;Mascota&gt; del RAR original no está permitido por la Práctica 3,
/// así que se reemplazó con ListaMascotas, implementada manualmente.
/// </summary>
public sealed class VeterinariaService
{
    private readonly ListaMascotas _pacientes = new();
    private readonly object _candado = new();

    // Operaciones originales de Práctica 1 (con la lista propia como tipo de retorno).
    public ListaMascotas ObtenerTodos() { lock (_candado) return _pacientes.Copiar(); }

    public void Registrar(Mascota mascota)
    {
        ArgumentNullException.ThrowIfNull(mascota);
        lock (_candado)
        {
            if (_pacientes.Buscar(mascota.CodigoUnico) is not null)
                throw new InvalidOperationException("Ya existe un paciente con el mismo código.");
            _pacientes.Agregar(mascota);
        }
    }

    public Mascota? BuscarPorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return null;
        lock (_candado) return _pacientes.Buscar(codigo.Trim());
    }

    public bool Eliminar(string codigo)
    {
        lock (_candado) return _pacientes.Eliminar(codigo);
    }

    // Métodos adicionales necesarios para conectar la Práctica 1 con MVC.
    public ListaMascotas ObtenerTodas() => ObtenerTodos();
    public Mascota? Buscar(string codigo) => BuscarPorCodigo(codigo);

    public Mascota Registrar(FormMascotaViewModel formulario)
    {
        lock (_candado)
        {
            Mascota nueva;
            do { nueva = Construir(formulario); }
            while (_pacientes.Buscar(nueva.CodigoUnico) is not null);
            _pacientes.Agregar(nueva);
            return nueva;
        }
    }

    public bool Editar(string codigo, FormMascotaViewModel formulario)
    {
        lock (_candado)
        {
            Mascota? paciente = _pacientes.Buscar(codigo);
            if (paciente is null) return false;
            // La especie original se conserva: editar información no transforma un
            // Perro en Gato. Se preservan siempre CódigoUnico y Estado.
            int tipoActual = paciente switch
            {
                Perro => 1, Gato => 2, Ave => 3, Tortuga => 4, _ => 0
            };
            if (formulario.Tipo != tipoActual) return false;

            // Formularios validados antes de llegar a este servicio.
            paciente.Nombre = formulario.Nombre;
            paciente.PesoKg = formulario.Peso;
            paciente.Sexo = InterpretarSexo(formulario.Sexo);
            paciente.Edad = formulario.Edad;
            paciente.Propietario = formulario.Propietario;

            switch (paciente)
            {
                case Perro perro:
                    perro.Raza = formulario.Raza!;
                    perro.Tamano = formulario.Tamano!;
                    break;
                case Gato gato:
                    gato.Raza = formulario.Raza!;
                    gato.Esterilizado = formulario.Esterilizado;
                    break;
                case Ave ave:
                    ave.EnvergaduraAlasCm = formulario.EnvergaduraAlasCm!.Value;
                    ave.PuedeVolar = formulario.PuedeVolar;
                    break;
                case Tortuga tortuga:
                    tortuga.TipoCaparazon = formulario.TipoCaparazon!;
                    tortuga.EsAcuatica = formulario.EsAcuatica;
                    break;
            }
            return true;
        }
    }

    public bool CambiarEstado(string codigo)
    {
        lock (_candado)
        {
            Mascota? mascota = _pacientes.Buscar(codigo);
            if (mascota is null) return false;
            mascota.CambiarEstado(); // método original de Mascota (Práctica 1)
            return true;
        }
    }

    public double? CalcularDosis(string codigo, double dosisPorKg)
    {
        lock (_candado)
        {
            Mascota? mascota = _pacientes.Buscar(codigo);
            return mascota?.CalcularDosis(dosisPorKg); // polimorfismo original
        }
    }

    private static SexoMascota InterpretarSexo(string texto) => texto switch
    {
        "Macho" => SexoMascota.Macho,
        "Hembra" => SexoMascota.Hembra,
        _ => throw new ArgumentException("Sexo no válido")
    };

    private static EstadoPaciente InterpretarEstado(string texto) => texto switch
    {
        "Sano" => EstadoPaciente.Sano,
        "Enfermo" => EstadoPaciente.Enfermo,
        _ => throw new ArgumentException("Estado no válido")
    };

    private static Mascota Construir(FormMascotaViewModel datos)
    {
        var sexo = InterpretarSexo(datos.Sexo);
        var estado = InterpretarEstado(datos.Estado);
        return datos.Tipo switch
        {
            1 => new Perro(datos.Nombre, datos.Peso, sexo, datos.Edad,
                datos.Propietario, estado, datos.Raza!.Trim(), datos.Tamano!.Trim()),
            2 => new Gato(datos.Nombre, datos.Peso, sexo, datos.Edad,
                datos.Propietario, estado, datos.Raza!.Trim(), datos.Esterilizado),
            3 => new Ave(datos.Nombre, datos.Peso, sexo, datos.Edad,
                datos.Propietario, estado, datos.EnvergaduraAlasCm!.Value, datos.PuedeVolar),
            4 => new Tortuga(datos.Nombre, datos.Peso, sexo, datos.Edad,
                datos.Propietario, estado, datos.TipoCaparazon!.Trim(), datos.EsAcuatica),
            _ => throw new ArgumentException("La especie seleccionada no existe.")
        };
    }
}
