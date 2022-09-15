using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Deposits;
using Caracal.Web.Core.Messaging;

namespace Caracal.SpringBoot.Workers.General.Workers;

public sealed class Worker : BackgroundService {
  private readonly ILogger<Worker> _logger;
  private readonly IReadonlyQueue _queue;
  private readonly DataContext _dbContext;

  public Worker(ILogger<Worker> logger, IReadonlyQueue queue, DataContext dbContext) {
    _logger = logger;
    _queue = queue;
    _dbContext = dbContext;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken) {
    while (!stoppingToken.IsCancellationRequested) {
      if (!TryProcessDeposits(stoppingToken))
        break;
    }

    return Task.CompletedTask;
  }

  private bool TryProcessDeposits(CancellationToken stoppingToken) {
    try {
      ProcessDeposits(stoppingToken);
      return true;
    }
    catch (Exception exception) {
      _logger.Log(LogLevel.Critical, exception, "An exception occured while processing deposits");
      return false;
    }
  }

  private void ProcessDeposits(CancellationToken stoppingToken) {
    foreach (var deposit in _queue.Subscribe<Deposit>(nameof(Deposit), stoppingToken)) {
      ProcessDeposit(deposit, stoppingToken);
    }
  }

  private void ProcessDeposit(Deposit deposit, CancellationToken stoppingToken) {
    _dbContext.Deposits.AddAsync(deposit, stoppingToken);
    _dbContext.SaveChangesAsync(stoppingToken);
  }
}