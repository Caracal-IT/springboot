using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Deposits;
using Caracal.Web.Core.Messaging;

namespace Caracal.SpringBoot.Workers.General.Workers;

public sealed class Worker : BackgroundService {
  private readonly ILogger<Worker> _logger;
  private readonly IReadonlyQueue _queue;
  private readonly IServiceProvider _serviceProvider;

  public Worker(ILogger<Worker> logger, IReadonlyQueue queue, IServiceProvider serviceProvider) {
    _logger = logger;
    _queue = queue;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    while (!stoppingToken.IsCancellationRequested) {
      if (!await TryProcessDepositsAsync(stoppingToken))
        break;
    }
  }

  private async Task<bool> TryProcessDepositsAsync(CancellationToken stoppingToken) {
    try {
      await ProcessDepositsAsync(stoppingToken);
      return true;
    }
    catch (Exception exception) {
      _logger.Log(LogLevel.Critical, exception, "An exception occured while processing deposits");
      return false;
    }
  }

  private async Task ProcessDepositsAsync(CancellationToken stoppingToken) {
    foreach (var deposit in _queue.Subscribe<Deposit>(nameof(Deposit), stoppingToken)) {
      await ProcessDepositAsync(deposit.Value, stoppingToken);
    }
  }

  private async Task ProcessDepositAsync(Deposit deposit, CancellationToken stoppingToken) {
    using var scope = _serviceProvider.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

    await dataContext.Deposits.AddAsync(deposit, stoppingToken);
    await dataContext.SaveChangesAsync(stoppingToken);
  }
}