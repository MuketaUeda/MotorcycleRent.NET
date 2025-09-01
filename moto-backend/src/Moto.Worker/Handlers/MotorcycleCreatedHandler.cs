// MotorcycleCreatedHandler - Handler para eventos de moto criada
// Processa eventos quando uma moto é cadastrada no sistema
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moto.Application.DTOs.Events;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.Validators;
using FluentValidation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Moto.Worker.Handlers;

public class MotorcycleCreatedHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MotorcycleCreatedHandler> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;
    private const string ExchangeName = "motorcycle_events";
    private const string QueueName = "motorcycle_created_queue";
    private const string RoutingKey = "motorcycle.created";

    public MotorcycleCreatedHandler(IServiceProvider serviceProvider, ILogger<MotorcycleCreatedHandler> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    private void InitializeConnection()
    {
        if (_connection != null && _channel != null) return;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare exchange
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);

        // Declare queue
        _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);

        // Bind queue to exchange
        _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
    }

    public void StartConsuming()
    {
        try
        {
            _logger.LogInformation("Starting to consume from queue: {QueueName}", QueueName);
            
            InitializeConnection();
            
            var consumer = new EventingBasicConsumer(_channel!);
            
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    _logger.LogInformation("Received motorcycle created event: {Message}", message);
                    
                    var eventDto = JsonSerializer.Deserialize<MotorcycleCreatedEventDto>(message);
                    if (eventDto != null)
                    {
                        await ProcessMotorcycleCreatedEvent(eventDto);
                    }
                    
                    _channel!.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing motorcycle created event");
                    _channel!.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel!.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            _logger.LogInformation("Successfully started consuming motorcycle created events from queue: {QueueName}", QueueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting consumer for queue: {QueueName}", QueueName);
            throw;
        }
    }

    private async Task ProcessMotorcycleCreatedEvent(MotorcycleCreatedEventDto eventDto)
    {
        // Validate the event DTO
        var eventValidator = new MotorcycleCreatedEventDtoValidator();
        var validationResult = await eventValidator.ValidateAsync(eventDto);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid motorcycle created event: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            return;
        }

        // Only process motorcycles from year 2024
        if (eventDto.Year != 2024)
        {
            _logger.LogInformation("Skipping motorcycle {Plate} - year {Year} is not 2024", eventDto.Plate, eventDto.Year);
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var motorcycleRepository = scope.ServiceProvider.GetRequiredService<IMotorcycleRepository>();
        var motorcycleEventRepository = scope.ServiceProvider.GetRequiredService<IMotorcycleEventRepository>();

        // Check if the motorcycle exists in the database before creating the event
        var existingMotorcycle = await motorcycleRepository.GetByIdAsync(eventDto.Id);
        if (existingMotorcycle == null)
        {
            _logger.LogWarning("Motorcycle with ID {MotorcycleId} not found in database. Skipping event creation.", eventDto.Id);
            return;
        }

        var motorcycleEvent = new MotorcycleEvent
        {
            Id = Guid.NewGuid(),
            MotorcycleId = eventDto.Id,
            MotorcycleModel = eventDto.Model,
            MotorcyclePlate = eventDto.Plate,
            MotorcycleYear = eventDto.Year,
            EventDate = DateTime.UtcNow,
            EventType = "MotorcycleCreated"
        };

        // Validate the entity before saving
        var entityValidator = new MotorcycleEventValidator();
        var entityValidationResult = await entityValidator.ValidateAsync(motorcycleEvent);
        
        if (!entityValidationResult.IsValid)
        {
            _logger.LogWarning("Invalid motorcycle event entity: {Errors}", 
                string.Join(", ", entityValidationResult.Errors.Select(e => e.ErrorMessage)));
            return;
        }

        await motorcycleEventRepository.AddAsync(motorcycleEvent);
        _logger.LogInformation("Stored motorcycle event for 2024 motorcycle: {Plate}", eventDto.Plate);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
