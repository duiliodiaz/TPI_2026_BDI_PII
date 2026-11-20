using Microsoft.EntityFrameworkCore;
using CineApp.Back.Data;
using CineApp.Back.Repositories;
using CineApp.Back.Repositories.Interface;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Repositories.Implementations;
using CineApp.Back.Services;
using CineApp.Back.Services.Interface;
using CineApp.Back.Services.Interfaces;
using CineApp.Back.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Base de datos (una sola vez)
builder.Services.AddDbContext<CineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios y Servicios - Soporte y Comprobantes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ITamañoProductoRepository, TamañoProductoRepository>();
builder.Services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();
builder.Services.AddScoped<IFormaPagoRepository, FormaPagoRepository>();
builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();

builder.Services.AddScoped<ICatalogoProductoService, CatalogoProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IComprobanteService, ComprobanteService>();

// Repositorios y Servicios - Auth y Dashboard
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();