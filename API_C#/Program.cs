using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CafeDatabase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configura la cadena de conexión CafeDatabase mediante user-secrets o ConnectionStrings__CafeDatabase.");
}

builder.Services.AddSingleton<IMarcaRepository>(_ => new MarcaRepository(connectionString));
builder.Services.AddSingleton<ICafeRepository>(_ => new CafeRepository(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngularLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();
