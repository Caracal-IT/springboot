namespace Caracal.SpringBoot.Workers.General.Workers;

public sealed class Worker : BackgroundService {
  private readonly ILogger<Worker> _logger;

  public Worker(ILogger<Worker> logger) {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    while (!stoppingToken.IsCancellationRequested) {
      //_logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
      await Task.Delay(10000, stoppingToken);
    }
  }
}