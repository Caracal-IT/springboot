using Caracal.Web.Core.Caching;
using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Withdrawals;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Caracal.SpringBoot.Api.Withdrawals; 

public sealed class GetWithdrawals {
  private readonly DataContext _dbContext;
  private readonly IConnectionMultiplexer _cache;
  
  public GetWithdrawals(DataContext dbContext, IConnectionMultiplexer cache) {
    _dbContext = dbContext;
    _cache = cache;
  }

  public async Task<List<Withdrawal>> ExecuteAsync(CancellationToken cancellationToken) {
    var recordKey = $"U{nameof(GetWithdrawals)}_{DateTime.Now:yyyyMMdd_hhmm}";
    var withdrawals = await _cache.GetRecordAsync<List<Withdrawal>>(recordKey);

    if (withdrawals != null) return withdrawals;
    
    withdrawals = await _dbContext.Withdrawals
                                  .ToListAsync(cancellationToken);
    
    await _cache.SetRecordAsync(recordKey, withdrawals);

    return withdrawals;
  }
}

