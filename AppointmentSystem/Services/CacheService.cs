using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace AppointmentSystem.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;
        private readonly IConfiguration _configuration;

        public CacheService(
            IMemoryCache memoryCache,
            IDistributedCache redisCache,
            IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _redisCache = redisCache;
            _configuration = configuration;
        }

        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            // Önce Redis'te ara
            var redisValue = await _redisCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                return JsonSerializer.Deserialize<T>(redisValue);
            }

            // Redis'te yoksa memory cache'te ara
            if (_memoryCache.TryGetValue(key, out T? cachedValue))
            {
                return cachedValue;
            }

            // Hiçbir cache'te yoksa factory'den al
            var value = await factory();

            // Redis'e kaydet
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);

            // Memory cache'e kaydet
            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            _memoryCache.Set(key, value, memoryOptions);

            return value;
        }

        public async Task RemoveAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
            _memoryCache.Remove(key);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            // Önce Redis'te ara
            var redisValue = await _redisCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                return JsonSerializer.Deserialize<T>(redisValue);
            }

            // Redis'te yoksa memory cache'te ara
            if (_memoryCache.TryGetValue(key, out T? cachedValue))
            {
                return cachedValue;
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };

            await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);

            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            _memoryCache.Set(key, value, memoryOptions);
        }
    }
} 