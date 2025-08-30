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
        
        try
        {
            _motorcycleCreatedHandler.StartConsuming();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Motorcycle Event Worker");
        }
        finally
        {
            _motorcycleCreatedHandler.Dispose();
        }
    }
}
