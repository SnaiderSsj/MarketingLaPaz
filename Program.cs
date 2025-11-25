using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Infraestructura.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexion de Railway
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
Console.WriteLine($"La cadena de conexion es esta: {connectionString}");

// Add services to the container
builder.Services.AddControllers();

// Configurar PostgreSQL
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

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar el puerto para Railway
builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

// Ejecutar migraciones automaticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MarketingDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Metodo para convertir DATABASE_URL de Railway a connection string
static string ParseDatabaseUrl(string databaseUrl)
{
    if (string.IsNullOrEmpty(databaseUrl))
        return "Host=localhost;Database=MarketingLaPazDB;Username=postgres;Password=password";
    
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    
    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
}
