using Microsoft.AspNetCore.Mvc;
using Practica3Veterinaria.Estructuras;
using Practica3Veterinaria.Models;
using Practica3Veterinaria.Services;
using Practica3Veterinaria.ViewModels;

namespace Practica3Veterinaria.Controllers;

public sealed class MascotasController : Controller
{
    private readonly VeterinariaService _veterinaria;

    public MascotasController(VeterinariaService veterinaria) => _veterinaria = veterinaria;

    [HttpGet]
    public IActionResult Index(string? buscar)
    {
        var todas = _veterinaria.ObtenerTodas();
        var filtradas = new ListaMascotas();
        var vista = new IndexMascotasViewModel { Mascotas = filtradas, Busqueda = buscar?.Trim() ?? "" };
        foreach (var mascota in todas)
        {
            vista.Total++;
            if (mascota.EstaEnferma) vista.Enfermas++;
            else vista.Sanas++;

            switch (mascota)
            {
                case Perro: vista.Perros++; break;
                case Gato: vista.Gatos++; break;
                case Ave: vista.Aves++; break;
                case Tortuga: vista.Tortugas++; break;
            }

            if (string.IsNullOrEmpty(vista.Busqueda) ||
                mascota.Nombre.Contains(vista.Busqueda, StringComparison.OrdinalIgnoreCase) ||
                mascota.Codigo.Contains(vista.Busqueda, StringComparison.OrdinalIgnoreCase) ||
                mascota.Propietario.Contains(vista.Busqueda, StringComparison.OrdinalIgnoreCase))
                filtradas.Agregar(mascota);
        }
        return View(vista);
    }

    [HttpGet]
    public IActionResult Crear() => View(new FormMascotaViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(FormMascotaViewModel formulario)
    {
        if (!ModelState.IsValid) return View(formulario);
        var nueva = _veterinaria.Registrar(formulario);
        TempData["Exito"] = $"{nueva.Nombre} fue registrado correctamente.";
        return RedirectToAction(nameof(Detalles), new { id = nueva.Codigo });
    }

    [HttpGet]
    public IActionResult Detalles(string id)
    {
        var mascota = _veterinaria.Buscar(id);
        if (mascota is null) return NotFound();
        return View(mascota);
    }

    [HttpGet]
    public IActionResult Editar(string id)
    {
        var mascota = _veterinaria.Buscar(id);
        if (mascota is null) return NotFound();
        return View(FormMascotaViewModel.DesdeMascota(mascota));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(string id, FormMascotaViewModel formulario)
    {
        if (!ModelState.IsValid) return View(formulario);
        if (!_veterinaria.Editar(id, formulario)) return NotFound();
        TempData["Exito"] = "Los datos de la mascota se actualizaron correctamente.";
        return RedirectToAction(nameof(Detalles), new { id });
    }

    [HttpGet]
    public IActionResult Eliminar(string id)
    {
        var mascota = _veterinaria.Buscar(id);
        if (mascota is null) return NotFound();
        return View(mascota);
    }

    [HttpPost]
    [ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmarEliminar(string id)
    {
        if (!_veterinaria.Eliminar(id)) return NotFound();
        TempData["Exito"] = "Mascota eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CambiarEstado(string id)
    {
        if (!_veterinaria.CambiarEstado(id)) return NotFound();
        TempData["Exito"] = "Estado del paciente actualizado.";
        return RedirectToAction(nameof(Detalles), new { id });
    }

    [HttpGet]
    public IActionResult Dosis(string id)
    {
        var mascota = _veterinaria.Buscar(id);
        if (mascota is null) return NotFound();
        return View(PrepararDosis(mascota, new DosisViewModel { Codigo = id }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Dosis(DosisViewModel formulario)
    {
        var mascota = _veterinaria.Buscar(formulario.Codigo);
        if (mascota is null) return NotFound();
        PrepararDosis(mascota, formulario);
        if (!ModelState.IsValid) return View(formulario);
        try
        {
            formulario.ResultadoMg = _veterinaria.CalcularDosis(mascota.Codigo, formulario.DosisPorKg);
        }
        catch (OverflowException)
        {
            ModelState.AddModelError(nameof(formulario.DosisPorKg), "La dosis produce un resultado demasiado grande.");
        }
        return View(formulario);
    }

    [HttpGet]
    public IActionResult Error() => View();

    private static DosisViewModel PrepararDosis(Mascota mascota, DosisViewModel formulario)
    {
        formulario.Nombre = mascota.Nombre;
        formulario.Especie = mascota.Especie;
        formulario.Peso = mascota.Peso;
        formulario.Factor = mascota.FactorDosis;
        return formulario;
    }
}
