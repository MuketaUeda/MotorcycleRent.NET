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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Motorcycle Event Worker");
        
        var maxRetries = 10;
        var retryDelay = TimeSpan.FromSeconds(5);
        var currentRetry = 0;
        
        while (currentRetry < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Attempting to start consumer (attempt {CurrentRetry}/{MaxRetries})", currentRetry + 1, maxRetries);
                _motorcycleCreatedHandler.StartConsuming();
                
                _logger.LogInformation("Successfully started consuming motorcycle events");
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
                break;
            }
            catch (Exception ex)
            {
                currentRetry++;
                _logger.LogWarning(ex, "Failed to start consumer (attempt {CurrentRetry}/{MaxRetries})", currentRetry, maxRetries);
                
                if (currentRetry < maxRetries)
                {
                    _logger.LogInformation("Retrying in {RetryDelay} seconds...", retryDelay.TotalSeconds);
                    await Task.Delay(retryDelay, stoppingToken);
                }
                else
                {
                    _logger.LogError(ex, "Failed to start consumer after {MaxRetries} attempts", maxRetries);
                    throw;
                }
            }
        }
        
        _motorcycleCreatedHandler.Dispose();
    }
}
