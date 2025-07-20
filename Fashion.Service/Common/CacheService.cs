using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Fashion.Service.Common
{
    /// <summary>
    /// Memory cache service implementation
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly Dictionary<string, DateTime> _cacheTimestamps = new();
        private readonly Dictionary<string, int> _cacheHits = new();
        private readonly Dictionary<string, int> _cacheMisses = new();

        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(1);
                
                if (_memoryCache.TryGetValue(key, out T? value))
                {
                    IncrementCacheHits(key);
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                    return value;
                }

                IncrementCacheMisses(key);
                _logger.LogDebug("Cache miss for key: {Key}", key);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item from cache with key: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(1);
                
                var options = new MemoryCacheEntryOptions();
                
                if (expiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = expiration;
                }
                else
                {
                    // Default expiration: 30 minutes
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                }

                _memoryCache.Set(key, value, options);
                _cacheTimestamps[key] = DateTime.UtcNow;
                
                _logger.LogDebug("Item cached with key: {Key}, expiration: {Expiration}", key, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting item in cache with key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(1);
                
                _memoryCache.Remove(key);
                _cacheTimestamps.Remove(key);
                _cacheHits.Remove(key);
                _cacheMisses.Remove(key);
                
                _logger.LogDebug("Item removed from cache with key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cache with key: {Key}", key);
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                var regex = new Regex(pattern);
                var keysToRemove = new List<string>();

                // Get all keys that match the pattern
                foreach (var key in _cacheTimestamps.Keys)
                {
                    if (regex.IsMatch(key))
                    {
                        keysToRemove.Add(key);
                    }
                }

                // Remove matching keys
                foreach (var key in keysToRemove)
                {
                    await RemoveAsync(key);
                }

                _logger.LogDebug("Removed {Count} items from cache matching pattern: {Pattern}", keysToRemove.Count, pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing items from cache by pattern: {Pattern}", pattern);
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                // Simulate async operation
                await Task.Delay(1);
                
                if (_memoryCache is MemoryCache memoryCache)
                {
                    memoryCache.Compact(1.0);
                }
                
                _cacheTimestamps.Clear();
                _cacheHits.Clear();
                _cacheMisses.Clear();
                
                _logger.LogInformation("Cache cleared");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cache");
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            var cachedValue = await GetAsync<T>(key);
            if (cachedValue != null)
            {
                return cachedValue;
            }

            var value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            // Simulate async operation
            await Task.Delay(1);
            return _memoryCache.TryGetValue(key, out _);
        }

        public async Task<CacheStatsDto> GetStatsAsync()
        {
            // Simulate async operation
            await Task.Delay(1);
            
            var totalHits = _cacheHits.Values.Sum();
            var totalMisses = _cacheMisses.Values.Sum();
            var totalRequests = totalHits + totalMisses;

            var hitRate = totalRequests > 0 ? (double)totalHits / totalRequests * 100 : 0;
            var missRate = totalRequests > 0 ? (double)totalMisses / totalRequests * 100 : 0;

            return new CacheStatsDto
            {
                TotalEntries = _cacheTimestamps.Count,
                MemoryUsage = GC.GetTotalMemory(false),
                HitRate = Math.Round(hitRate, 2),
                MissRate = Math.Round(missRate, 2)
            };
        }

        private void IncrementCacheHits(string key)
        {
            if (!_cacheHits.ContainsKey(key))
                _cacheHits[key] = 0;
            _cacheHits[key]++;
        }

        private void IncrementCacheMisses(string key)
        {
            if (!_cacheMisses.ContainsKey(key))
                _cacheMisses[key] = 0;
            _cacheMisses[key]++;
        }
    }
} 