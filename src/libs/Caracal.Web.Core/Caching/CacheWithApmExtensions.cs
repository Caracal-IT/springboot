using System.Text.Json;
using StackExchange.Redis;

namespace Caracal.Web.Core.Caching; 

public static class CacheWithApmExtensions {
  public static async Task SetRecordAsync<T>(this IConnectionMultiplexer multiplexer, string recordId, T data, TimeSpan? absoluteExpireTime = null) {
    var expiryTime = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
    var jsonData = JsonSerializer.Serialize(data);
    var db = multiplexer.GetDatabase();
    await db.StringSetAsync(recordId, jsonData, expiryTime, When.Always);
  }

  public static async Task<T?> GetRecordAsync<T>(this IConnectionMultiplexer multiplexer, string recordId) {
    var db = multiplexer.GetDatabase();
    var jsonData = await db.StringGetAsync(recordId);

    return !jsonData.HasValue ? default : JsonSerializer.Deserialize<T>(jsonData!);
  }
}