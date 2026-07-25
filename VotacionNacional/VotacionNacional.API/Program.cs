using Microsoft.EntityFrameworkCore;
using VotacionNacional.BLL.Interfaces;
using VotacionNacional.BLL.Services;
using VotacionNacional.DAL.Context;
using VotacionNacional.DAL.Interfaces;
using VotacionNacional.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión DefaultConnection.");
}

builder.Services.AddDbContext<VotacionDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IVotanteRepository, VotanteRepository>();
builder.Services.AddScoped<IVotanteService, VotanteService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirMVC", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

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
