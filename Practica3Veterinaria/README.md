# Práctica 3 — Sistema de Veterinaria con ASP.NET Core MVC

Proyecto educativo de C# / ASP.NET Core MVC (.NET 8), sin persistencia ni base de datos.

## Inicio rápido — Visual Studio 2022

1. Instala Visual Studio 2022 con **Desarrollo de ASP.NET y web** y el SDK .NET 8.
2. Abre `Practica3Veterinaria.sln` (no un archivo `.cs` por separado).
3. Presiona **F5** con el perfil **Veterinaria (HTTPS)**.
4. En el navegador registra, consulta, edita y elimina mascotas, cambia su estado y calcula dosis.

**Guía completa:** [Documentacion/GUIA_VISUAL_STUDIO.md](Documentacion/GUIA_VISUAL_STUDIO.md)

## Restricciones respetadas

- C#, **ASP.NET Core MVC** y **Razor**.
- Clases `Mascota` (abstracta), `Perro`, `Gato`, `Ave` y `Tortuga`.
- Lista enlazada **hecha manualmente**, con eslabón interno específico. **No usa `List<T>`, `Node<T>`, `LinkedList<T>` ni otra colección del framework como almacenamiento.**
- Ninguna base de datos, Entity Framework ni almacenamiento permanente.
- Sin dependencias externas de NuGet, CDN o archivos DLL descargables.
- Modelos de dominio separados de controladores, servicio, formularios y vistas.
- Formularios HTTP POST protegidos con tokens antifalsificación, lectura por HTTP GET.

## Cálculo académico de dosis

`dosis_mg = peso_kg × dosis_base_mg_por_kg × factor_especie`

Perro `1.00`; Gato `0.90`; Ave `0.50`; Tortuga `0.80`. **No son dosis veterinarias reales.**

## Verificaciones

Ejecuta `dotnet run --project Pruebas/Pruebas.csproj` dentro de la carpeta del proyecto para probar registro, búsqueda, modificación, eliminación, polimorfismo, estado y dosis.

## Nota de compatibilidad con Práctica 1

Esta versión **sí utiliza las clases `.cs` extraídas del archivo `Practica1_Veterinaria_POO.rar`**. Se conservaron las propiedades, constructores y métodos de POO de la Práctica 1, realizando únicamente las adaptaciones necesarias para MVC y correcciones documentadas. **Única excepción obligatoria:** el servicio original almacenaba las mascotas en `List<Mascota>`, lo cual contradice expresamente la Práctica 3; se reemplazó por `ListaMascotas` propia. Consulta `Documentacion/ORIGEN_Y_ADAPTACIONES.md` para conocer exactamente las diferencias.


> **Adaptación del RAR original:** consulta [ORIGEN_Y_ADAPTACIONES.md](Documentacion/ORIGEN_Y_ADAPTACIONES.md). Se conservaron las clases del RAR y se sustituyó `List<Mascota>` por una lista propia porque la Práctica 3 lo prohíbe.
