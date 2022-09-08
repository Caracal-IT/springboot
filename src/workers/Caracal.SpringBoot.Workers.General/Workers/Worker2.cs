namespace Caracal.SpringBoot.Workers.General.Workers;

public class Worker2 : BackgroundService {
  private readonly ILogger<Worker2> _logger;

  public Worker2(ILogger<Worker2> logger) {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    while (!stoppingToken.IsCancellationRequested) {
      //_logger.LogInformation("Second Worker running at: {Time}", DateTimeOffset.Now);
      await Task.Delay(15000, stoppingToken);
    }
  }
}