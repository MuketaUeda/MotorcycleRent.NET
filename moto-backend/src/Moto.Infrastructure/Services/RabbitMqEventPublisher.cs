// RabbitMqEventPublisher - Publisher for motorcycle created events
// Publishes motorcycle created events to RabbitMQ
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moto.Application.DTOs.Events;
using Moto.Application.Interfaces;
using RabbitMQ.Client;

namespace Moto.Infrastructure.Services;

public class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "motorcycle_events";
    private const string QueueName = "motorcycle_created_queue";
    private const string RoutingKey = "motorcycle.created";

    public RabbitMqEventPublisher(IConfiguration configuration)
    {   
        // Create connection factory
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        // Create connection
        _connection = factory.CreateConnection();

        // Create channel
        _channel = _connection.CreateModel();

        // Declare exchange
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);

        // Declare queue
        _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);

        // Bind queue to exchange
        _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
    }

    public void PublishMotorcycleCreatedEvent(MotorcycleCreatedEventDto eventDto)
    {   
        // Serialize event to JSON --> RabbitMQ expects a byte array
        var message = JsonSerializer.Serialize(eventDto);
        var body = Encoding.UTF8.GetBytes(message);

        // Create properties --> RabbitMQ expects a basic properties object
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        // Publish message
        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: RoutingKey,
            basicProperties: properties,
            body: body);
    }

    public void Dispose()
    {
        // Dispose channel and connection
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

