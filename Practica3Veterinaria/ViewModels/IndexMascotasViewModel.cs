using Practica3Veterinaria.Estructuras;

namespace Practica3Veterinaria.ViewModels;

public sealed class IndexMascotasViewModel
{
    public required ListaMascotas Mascotas { get; init; }
    public int Total { get; set; }
    public int Sanas { get; set; }
    public int Enfermas { get; set; }
    public int Perros { get; set; }
    public int Gatos { get; set; }
    public int Aves { get; set; }
    public int Tortugas { get; set; }
    public string Busqueda { get; set; } = "";
}
