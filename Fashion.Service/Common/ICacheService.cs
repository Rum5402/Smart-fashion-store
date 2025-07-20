namespace Fashion.Service.Common
{
    /// <summary>
    /// Interface for caching service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get item from cache
        /// </summary>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Set item in cache
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// Remove items by pattern
        /// </summary>
        Task RemoveByPatternAsync(string pattern);

        /// <summary>
        /// Clear all cache
        /// </summary>
        Task ClearAsync();

        /// <summary>
        /// Get or set item with cache
        /// </summary>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);

        /// <summary>
        /// Check if item exists in cache
        /// </summary>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Get cache statistics
        /// </summary>
        Task<CacheStatsDto> GetStatsAsync();
    }

    /// <summary>
    /// Cache statistics
    /// </summary>
    public class CacheStatsDto
    {
        /// <summary>
        /// Total number of cache entries
        /// </summary>
        public int TotalEntries { get; set; }

        /// <summary>
        /// Total memory usage in bytes
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// Cache hit rate percentage
        /// </summary>
        public double HitRate { get; set; }

        /// <summary>
        /// Cache miss rate percentage
        /// </summary>
        public double MissRate { get; set; }
    }
} 