using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Infraestructura.Data;
using MarketingLaPazAPI.Infraestructura.Repositorios;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Core.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión de Railway
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
Console.WriteLine($"Cadena de conexión: {connectionString}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework con PostgreSQL para Railway
if (!string.IsNullOrEmpty(connectionString))
{
    // Parsear DATABASE_URL de Railway
    var parsedConnectionString = ParseDatabaseUrl(connectionString);
    builder.Services.AddDbContext<MarketingDbContext>(options =>
        options.UseNpgsql(parsedConnectionString));
}
else
{
    // Para desarrollo local
    builder.Services.AddDbContext<MarketingDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("MarketingDB")));
}

// 🔥 CONFIGURACIÓN CORS ACTUALIZADA 🔥
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite cualquier origen durante desarrollo
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("*"); // Expone todos los headers
    });
    
    // Política más específica para producción
    options.AddPolicy("ProductionCors", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4173",    // Vite Preview
                "http://localhost:3000",    // React Dev Server
                "http://127.0.0.1:4173",    // Vite Preview alternativo
                "http://127.0.0.1:3000",    // React alternativo
                "https://marketing-la-paz-frontend1-production.up.railway.app/", // Tu frontend en producción
                "https://marketinglapaz-production.up.railway.app" // Tu backend actual
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("*");
    });
});

// Repositorios
builder.Services.AddScoped<ICampañaRepositorio, CampañaRepositorio>();
builder.Services.AddScoped<ILeadRepositorio, LeadRepositorio>();

// Servicios
builder.Services.AddScoped<IServicioCampaña, ServicioCampaña>();

// Configurar el puerto para Railway
builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

// 🔥 USAR CORS AL INICIO DEL PIPELINE 🔥
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll"); // En desarrollo permite todo
    Console.WriteLine("✅ CORS configurado para desarrollo: AllowAll");
}
else
{
    app.UseCors("ProductionCors"); // En producción usa política específica
    Console.WriteLine("✅ CORS configurado para producción: ProductionCors");
}

// Ejecutar migraciones automáticamente al iniciar en producción
if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MarketingDbContext>();
        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas exitosamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error aplicando migraciones: {ex.Message}");
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // En producción, también mostrar Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketing La Paz API v1");
        c.RoutePrefix = string.Empty; // Servir Swagger en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Método para convertir DATABASE_URL de Railway a connection string
static string ParseDatabaseUrl(string databaseUrl)
{
    if (string.IsNullOrEmpty(databaseUrl))
        return "Host=localhost;Database=MarketingLaPazDB;Username=postgres;Password=password";

    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');

        return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parseando DATABASE_URL: {ex.Message}");
        return "Host=localhost;Database=MarketingLaPazDB;Username=postgres;Password=password";
    }
}
