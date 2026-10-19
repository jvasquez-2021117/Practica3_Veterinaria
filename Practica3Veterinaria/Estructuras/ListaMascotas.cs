using System.Collections;
using Practica3Veterinaria.Models;

namespace Practica3Veterinaria.Estructuras;

/// <summary>
/// Lista simplemente enlazada creada desde cero, específica para Mascota.
/// No usa colecciones del framework como almacenamiento ni nodos genéricos.
/// </summary>
public sealed class ListaMascotas : IEnumerable<Mascota>
{
    // Eslabón interno concreto, sin parámetros de tipo genéricos.
    private sealed class Eslabon
    {
        public Eslabon(Mascota mascota) => Valor = mascota;
        public Mascota Valor { get; set; }
        public Eslabon? Siguiente { get; set; }
    }

    private Eslabon? _primero;
    private Eslabon? _ultimo;
    private int _cantidad;

    public int Cantidad => _cantidad;

    public bool Agregar(Mascota mascota)
    {
        ArgumentNullException.ThrowIfNull(mascota);
        if (Buscar(mascota.CodigoUnico) is not null) return false;

        var nuevo = new Eslabon(mascota);
        if (_ultimo is null) _primero = nuevo;
        else _ultimo.Siguiente = nuevo;

        _ultimo = nuevo;
        _cantidad++;
        return true;
    }

    public Mascota? Buscar(string codigo)
    {
        var actual = _primero;
        while (actual is not null)
        {
            if (string.Equals(actual.Valor.CodigoUnico, codigo, StringComparison.OrdinalIgnoreCase))
                return actual.Valor;
            actual = actual.Siguiente;
        }
        return null;
    }

    public bool Actualizar(string codigo, Mascota actualizada)
    {
        ArgumentNullException.ThrowIfNull(actualizada);
        if (!string.Equals(codigo, actualizada.CodigoUnico, StringComparison.OrdinalIgnoreCase))
            return false;

        var actual = _primero;
        while (actual is not null)
        {
            if (string.Equals(actual.Valor.CodigoUnico, codigo, StringComparison.OrdinalIgnoreCase))
            {
                actual.Valor = actualizada;
                return true;
            }
            actual = actual.Siguiente;
        }
        return false;
    }

    public bool Eliminar(string codigo)
    {
        Eslabon? anterior = null;
        Eslabon? actual = _primero;
        while (actual is not null)
        {
            if (string.Equals(actual.Valor.CodigoUnico, codigo, StringComparison.OrdinalIgnoreCase))
            {
                if (anterior is null) _primero = actual.Siguiente;
                else anterior.Siguiente = actual.Siguiente;
                if (actual == _ultimo) _ultimo = anterior;
                _cantidad--;
                return true;
            }
            anterior = actual;
            actual = actual.Siguiente;
        }
        return false;
    }

    /// <summary>Copia los enlaces para recorrer sin interferir con cambios posteriores.</summary>
    public ListaMascotas Copiar()
    {
        var copia = new ListaMascotas();
        var actual = _primero;
        while (actual is not null)
        {
            copia.Agregar(actual.Valor);
            actual = actual.Siguiente;
        }
        return copia;
    }

    /// <summary>La interfaz facilita foreach sin sustituir el almacenamiento enlazado.</summary>
    public IEnumerator<Mascota> GetEnumerator()
    {
        var actual = _primero;
        while (actual is not null)
        {
            yield return actual.Valor;
            actual = actual.Siguiente;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
