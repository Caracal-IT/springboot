using Caracal.SpringBoot.Api.Caching;
using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Withdrawals;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Caracal.SpringBoot.Api.Withdrawals; 

public class GetWithdrawals {
  private readonly DataContext _dbContext;
  //private readonly IDistributedCache _cache;
  private readonly IConnectionMultiplexer _multiplexer;
  
  public GetWithdrawals(DataContext dbContext
   // , IDistributedCache cache
   , IConnectionMultiplexer multiplexer
    ) {
    _dbContext = dbContext;
    //_cache = cache;
    _multiplexer = multiplexer;
  }

  public async Task<List<Withdrawal>> ExecuteAsync() {
    var recordKey = $"Users_{DateTime.Now:yyyyMMdd_hhmm}";
    
    var withdrawals = await _multiplexer.GetRecordAsync<List<Withdrawal>>(recordKey);

    if (withdrawals == null) {
      withdrawals = _dbContext.Withdrawals.ToList();
      await Task.Delay(100);
      await _multiplexer.SetRecordAsync(recordKey, withdrawals);
    }

    return withdrawals;
  }
}