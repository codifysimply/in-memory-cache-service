using CS.Services.CacheMemoryService.Extensions;
using CS.Services.CacheMemoryService.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.CacheMemoryService
{
    public class CSMemoryCacheService : ICSMemoryCacheService
    {
        private readonly IMemoryCache memoryCache;

        public CSMemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public object GetCache(object key)
        {
            return memoryCache.Get(key);
        }

        public T GetCache<T>(object key)
        {
            return memoryCache.Get<T>(key);
        }

        public void RemoveCache(object key)
        {
            memoryCache.Remove(key);
        }

        public T SetCache<T>(object key, T value, MemoryCacheEntryOptions options)
        {
            return memoryCache.Set<T>(key, value, options);
        }

        public T SetCache<T>(object key, T value, DateTimeOffset absoluteExpiration)
        {
            return memoryCache.Set<T>(key, value, absoluteExpiration);
        }

        public T SetCache<T>(object key, T value, TimeSpan slidingExpiration)
        {
            using ICacheEntry entry = memoryCache.CreateEntry(key);
            entry.SetSlidingExpiration(slidingExpiration);
            entry.Value = value;

            return value;
        }

        public Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, MemoryCacheEntryOptions options)
        {
            return memoryCache.GetOrCreateAsyncLazy(key, factory, options);
        }

        public Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, DateTimeOffset absoluteExpiration)
        {
            return memoryCache.GetOrCreateAsyncLazy(key, factory, absoluteExpiration);
        }

        public Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, TimeSpan slidingExpiration)
        {
            return memoryCache.GetOrCreateAsyncLazy(key, factory, slidingExpiration);
        }
    }
}