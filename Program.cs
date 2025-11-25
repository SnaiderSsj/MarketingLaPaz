using Microsoft.EntityFrameworkCore;
using MarketingLaPazAPI.Infraestructura.Data;
using MarketingLaPazAPI.Infraestructura.Repositorios;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Core.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework con PostgreSQL
builder.Services.AddDbContext<MarketingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MarketingDB")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MarketingPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Repositorios
builder.Services.AddScoped<ICampañaRepositorio, CampañaRepositorio>();
builder.Services.AddScoped<ILeadRepositorio, LeadRepositorio>();

// Servicios
builder.Services.AddScoped<IServicioCampaña, ServicioCampaña>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MarketingPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();