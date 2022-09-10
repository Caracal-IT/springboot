using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Caracal.SpringBoot.Api.Caching; 

public static class CacheHelperWithApm {
  public static async Task SetRecordAsync<T>(this IConnectionMultiplexer multiplexer,
    string recordId,
    T data,
    TimeSpan? absoluteExpireTime = null,
    TimeSpan? slidingExpireTime = null)
  {
    var options = new DistributedCacheEntryOptions();

    options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
    options.SlidingExpiration = slidingExpireTime;

    var jsonData = JsonSerializer.Serialize(data);
    var db = multiplexer.GetDatabase();
    await db.StringSetAsync(recordId, jsonData, options.SlidingExpiration);
  }

  public static async Task<T?> GetRecordAsync<T>(this IConnectionMultiplexer multiplexer,
    string recordId)
  {
    var db = multiplexer.GetDatabase();
    var jsonData = await db.StringGetAsync(recordId);

    if (!jsonData.HasValue)
      return default(T);

    return JsonSerializer.Deserialize<T>(jsonData!);
  }
}