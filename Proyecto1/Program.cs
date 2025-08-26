using Proyecto1.Services.Interfaces;
using Proyecto1.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// ? Configuración necesaria para TempData
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider(); // TempData via Session

builder.Services.AddSession(); // Habilita Session
builder.Services.AddMemoryCache();

// ?? Servicios HTTP
builder.Services.AddHttpClient<IServicioCliente, ServicioCliente>();
builder.Services.AddHttpClient<IServicioEmpleado, ServicioEmpleado>();
builder.Services.AddHttpClient<IServicioVehiculo, ServicioVehiculo>();
builder.Services.AddHttpClient<IServicioLavado, ServicioLavado>();
builder.Services.AddHttpClient<IServicioReporte, ServicioReporte>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ?? ¡Esta línea es clave!
app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{cedula?}");

app.Run();