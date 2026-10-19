using Practica3Veterinaria.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// Singleton: la lista propia permanece viva mientras el servidor siga ejecutándose.
builder.Services.AddSingleton<VeterinariaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Mascotas/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mascotas}/{action=Index}/{id?}");

app.Run();
