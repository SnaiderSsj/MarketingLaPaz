using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Infraestructura.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== DIAGNÓSTICO DETALLADO =====
Console.WriteLine("=== INICIANDO DIAGNÓSTICO DATABASE ===");

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE");
Console.WriteLine($"1. DATABASE presente: {!string.IsNullOrEmpty(database)}");

if (!string.IsNullOrEmpty(databaseUrl))
{
    Console.WriteLine($"2. DATABASE valor: {database}");
    
    try
    {
        var parsedConnection = ParseDatabaseUrl(database);
        Console.WriteLine($"3. Connection string parseada: {parsedConnection}");
        
        // Probemos la conexión inmediatamente
        Console.WriteLine("4. Probando conexión...");
        using var dbContext = new MarketingDbContext(
            new DbContextOptionsBuilder<MarketingDbContext>()
                .UseNpgsql(parsedConnection)
                .Options);
                
        var canConnect = dbContext.Database.CanConnect();
        Console.WriteLine($"5. ¿Puede conectar?: {canConnect}");
        
        if (!canConnect)
        {
            Console.WriteLine("6. ERROR: No se puede conectar a la BD");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"7. ERROR en parseo/conexión: {ex.Message}");
        Console.WriteLine($"8. StackTrace: {ex.StackTrace}");
    }
}
else
{
    Console.WriteLine("2. DATABASE está VACÍA o NULL");
    
    // Mostrar todas las variables de entorno para debug
    Console.WriteLine("=== VARIABLES DE ENTORNO ===");
    foreach (var envVar in Environment.GetEnvironmentVariables().Cast<System.Collections.DictionaryEntry>())
    {
        if (envVar.Key.ToString().Contains("DATABASE") || envVar.Key.ToString().Contains("POSTGRES"))
        {
            Console.WriteLine($"{envVar.Key} = {envVar.Value}");
        }
    }
}

// ===== CONFIGURACIÓN NORMAL =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
if (!string.IsNullOrEmpty(databaseUrl))
{
    try
    {
        var parsedConnection = ParseDatabaseUrl(databaseUrl);
        builder.Services.AddDbContext<MarketingDbContext>(options =>
            options.UseNpgsql(parsedConnection));
        Console.WriteLine("✅ DbContext configurado con Railway PostgreSQL");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error configurando DbContext: {ex.Message}");
        throw;
    }
}
else
{
    Console.WriteLine("🔄 Usando base de datos local");
    builder.Services.AddDbContext<MarketingDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("MarketingDB")));
}

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

// ===== MIGRACIÓN AUTOMÁTICA =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Console.WriteLine("🔄 Aplicando migraciones...");
        var context = services.GetRequiredService<MarketingDbContext>();
        context.Database.Migrate();
        Console.WriteLine("✅ Migraciones aplicadas exitosamente");
        
        // Verificar datos
        var campañasCount = context.Campañas.Count();
        Console.WriteLine($"📊 Campañas en BD: {campañasCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error en migraciones: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}

// Resto de la configuración...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static string ParseDatabaseUrl(string databaseUrl)
{
    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        
        return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
    }
    catch (Exception ex)
    {
        throw new Exception($"Error parseando DATABASE_URL: {ex.Message}");
    }
}
