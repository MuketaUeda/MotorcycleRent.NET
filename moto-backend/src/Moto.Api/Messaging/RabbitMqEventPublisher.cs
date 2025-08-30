using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moto.Application.DTOs.Events;
using Moto.Application.Interfaces;
using RabbitMQ.Client;

namespace Moto.Api.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "motorcycle_events";
    private const string QueueName = "motorcycle_created_queue";
    private const string RoutingKey = "motorcycle.created";

    public RabbitMqEventPublisher(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
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

    public void PublishMotorcycleCreatedEvent(MotorcycleCreatedEventDto eventDto)
    {
        var message = JsonSerializer.Serialize(eventDto);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: RoutingKey,
            basicProperties: properties,
            body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

