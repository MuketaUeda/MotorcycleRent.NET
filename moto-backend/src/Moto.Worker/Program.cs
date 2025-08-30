using Moto.Worker;
using Moto.Worker.Handlers;
using Moto.Infrastructure;
using Moto.Application;
using FluentValidation;
using Moto.Application.Validators;

var builder = Host.CreateApplicationBuilder(args);

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application services
builder.Services.AddApplication();

// Add FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateRentalDtoValidator>();

// Add Worker services
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<MotorcycleCreatedHandler>();

var host = builder.Build();
host.Run();
