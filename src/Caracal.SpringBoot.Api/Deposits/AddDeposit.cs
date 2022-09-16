using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Deposits;
using Caracal.Web.Core.Messaging;

namespace Caracal.SpringBoot.Api.Deposits; 

public sealed class AddDeposit {
  private readonly IWriteOnlyQueue _queue;
  public AddDeposit(IWriteOnlyQueue queue) => _queue = queue;
  
  public async Task<bool> ExecuteAsync(Deposit deposit, CancellationToken cancellationToken) => 
    await _queue.PublishAsync(
      nameof(Deposit), 
      new KeyValuePair<string, Deposit>($"Deposit {deposit.Id}", deposit), 
      cancellationToken);
}