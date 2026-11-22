using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CineApp.Back.Data;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Repositories.Implementations;
using CineApp.Back.Security;
using CineApp.Back.Services.Interfaces;
using CineApp.Back.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// JWT: Issuer, Audience y duración salen de appsettings.json; la clave, de User Secrets.
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

if (Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32)
{
    if (!builder.Environment.IsDevelopment())
        throw new InvalidOperationException("Falta la clave JWT (Jwt:Key) o tiene menos de 32 caracteres.");

    // En desarrollo, sin clave configurada se usa una aleatoria: los tokens dejan de valer al reiniciar.
    jwtSettings.Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
    Console.WriteLine("ADVERTENCIA: no hay Jwt:Key configurada; se usa una clave temporal. " +
        "Para fijarla: dotnet user-secrets set \"Jwt:Key\" \"<clave de 32+ caracteres>\" --project CineApp.WebAPI");
}

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

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
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger con botón "Authorize" para probar endpoints protegidos pegando el token.
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegá el token que devuelve POST /api/auth/login"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();