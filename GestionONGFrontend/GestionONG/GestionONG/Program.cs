using GestionONG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Si deseas mantener el nombre de las propiedades como en tu modelo
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


// Add DbContext
builder.Services.AddDbContext<GestionOngContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionConnection"));
});

// Add Swagger with annotations support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionOngBackend API", Version = "v1" });
    c.EnableAnnotations(); 
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS with the specified policy
app.UseCors("AllowFrontend");

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
