using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApplicationProyecto.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Inyección de dependencias
builder.Services.AddMemoryCache();


builder.Services.AddDbContext<DBcontexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoBD")));

// 🔧 Configuración de CORS para Proyecto1
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🔧 Controladores con manejo de referencias cíclicas
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// 🔧 Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebApplicationProyecto API",
        Version = "v1",
        Description = "Servicios REST para Clientes, Empleados, Vehículos y Lavados"
    });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var controller = apiDesc.ActionDescriptor?.RouteValues["controller"];
        return !string.Equals(controller, "Error", StringComparison.OrdinalIgnoreCase);
    });
});

var app = builder.Build();

// 🔧 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationProyecto API V1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// 🔧 Personalización Swagger (opcional)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationProyecto API V1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Documentación Técnica | WebApplicationProyecto";
    c.InjectStylesheet("/swagger-ui/custom.css");
    c.InjectJavascript("/swagger-ui/custom.js");
});

// ⚠️ HTTPS opcional
// app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();