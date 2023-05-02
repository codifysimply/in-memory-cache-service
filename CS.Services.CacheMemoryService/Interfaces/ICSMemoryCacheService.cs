using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.CacheMemoryService.Interfaces
{
    public interface ICSMemoryCacheService
    {
        object GetCache(object key);
        T GetCache<T>(object key);
        void RemoveCache(object key);

        T SetCache<T>(object key, T value, MemoryCacheEntryOptions options);
        T SetCache<T>(object key, T value, DateTimeOffset absoluteExpiration);
        T SetCache<T>(object key, T value, TimeSpan slidingExpiration);

        Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, MemoryCacheEntryOptions options);
        Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, DateTimeOffset absoluteExpiration);
        Task<T> GetOrCreateAsyncLazy<T>(object key, Func<Task<T>> factory, TimeSpan slidingExpiration);
    }
}
