using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Data.Context;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
var connectionString = builder.Configuration.GetConnectionString("ConexionLoggin");
builder.Services.AddDbContext<AplicacionDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ISesionUsuarioRepositorio, SesionUsuarioRepositorio>();
builder.Services.AddScoped<IPeriodoInactividadRepositorio, PeriodoInactividadRepositorio>();
builder.Services.AddScoped<IPeriodoActividadRepositorio, PeriodoActividadRepositorio>();
builder.Services.AddHostedService<VerificarInactividadService>(); // Agregar el servicio de fondo

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
