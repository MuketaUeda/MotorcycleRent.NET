// Executes RabbitMQ continuous worker
using Moto.Worker.Handlers;

namespace Moto.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly MotorcycleCreatedHandler _motorcycleCreatedHandler;

    public Worker(ILogger<Worker> logger, MotorcycleCreatedHandler motorcycleCreatedHandler)
    {
        _logger = logger;
        _motorcycleCreatedHandler = motorcycleCreatedHandler;
    }
    
    // Executes RabbitMQ continuous worker
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Motorcycle Event Worker");
        
        // Set max retries and retry delay
        var maxRetries = 10;
        var retryDelay = TimeSpan.FromSeconds(5);
        var currentRetry = 0;
        
        // Execute worker
        while (currentRetry < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Attempting to start consumer (attempt {CurrentRetry}/{MaxRetries})", currentRetry + 1, maxRetries);
                _motorcycleCreatedHandler.StartConsuming(); // Start consuming motorcycle events
                
                _logger.LogInformation("Successfully started consuming motorcycle events");
                
                // Infinite loop until cancellation token is requested
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken); // Wait for 1 second
                }
                break;
            }
            catch (Exception ex)
            {
                currentRetry++; // Increment retry count
                _logger.LogWarning(ex, "Failed to start consumer (attempt {CurrentRetry}/{MaxRetries})", currentRetry, maxRetries);
                
                if (currentRetry < maxRetries)
                {
                    _logger.LogInformation("Retrying in {RetryDelay} seconds...", retryDelay.TotalSeconds);
                    await Task.Delay(retryDelay, stoppingToken); // Wait for retry delay
                }
                else
                {
                    _logger.LogError(ex, "Failed to start consumer after {MaxRetries} attempts", maxRetries);
                    throw;
                }
            }
        }
        // Dispose motorcycle created handler
        _motorcycleCreatedHandler.Dispose();
    }
}
