using Microsoft.EntityFrameworkCore;
using VotacionNacional.BLL.Interfaces;
using VotacionNacional.BLL.Services;
using VotacionNacional.DAL.Context;
using VotacionNacional.DAL.Interfaces;
using VotacionNacional.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controladores de la API
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cadena de conexión
string? connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión DefaultConnection."
    );
}

// Entity Framework
builder.Services.AddDbContext<VotacionDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Repositorios
builder.Services.AddScoped<IVotanteRepository, VotanteRepository>();
builder.Services.AddScoped<IPartidoRepository, PartidoRepository>();
builder.Services.AddScoped<IVotoRepository, VotoRepository>();

// Servicios
builder.Services.AddScoped<IVotanteService, VotanteService>();
builder.Services.AddScoped<IPartidoService, PartidoService>();
builder.Services.AddScoped<IVotacionService,VotacionService>();

// Permite que el MVC consuma la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirMVC", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger solo durante desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirMVC");

app.UseAuthorization();

app.MapControllers();

app.Run();