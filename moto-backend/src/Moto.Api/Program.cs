using Moto.Infrastructure;
using Moto.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Motorcycle Rent API",
        Version = "v1",
        Description = "API for motorcycle rental system"
    });
});

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application services
builder.Services.AddApplication();

// Add AutoMapper for both API and Application layers
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(Moto.Application.Mappings.MappingProfile).Assembly);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motorcycle Rent API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
