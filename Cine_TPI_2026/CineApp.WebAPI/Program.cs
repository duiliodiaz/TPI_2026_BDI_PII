using Microsoft.EntityFrameworkCore;
using CineApp.Back.Data;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Repositories.Implementations;
using CineApp.Back.Services.Interfaces;
using CineApp.Back.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ITamañoProductoRepository, TamañoProductoRepository>();
builder.Services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();

builder.Services.AddScoped<ICatalogoProductoService, CatalogoProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
