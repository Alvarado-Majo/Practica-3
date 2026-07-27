var builder = WebApplication.CreateBuilder(args);

// Agrega soporte para controladores y vistas MVC
builder.Services.AddControllersWithViews();

// Configura el cliente HTTP que utilizará el MVC
// para comunicarse con el proyecto API
builder.Services.AddHttpClient("VotacionAPI", client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"];

    client.BaseAddress = new Uri(apiUrl!);
    client.Timeout = TimeSpan.FromSeconds(30);

})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

var app = builder.Build();

// Manejo de errores fuera del ambiente de desarrollo
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirige solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Permite utilizar los archivos de wwwroot
app.UseStaticFiles();

// Habilita el sistema de rutas
app.UseRouting();

// Habilita autorización
app.UseAuthorization();

// Define la ruta predeterminada del proyecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();