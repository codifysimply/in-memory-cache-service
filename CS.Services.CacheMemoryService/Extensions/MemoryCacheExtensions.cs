using CS.Services.CacheMemoryService;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.CacheMemoryService.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static Task<T> GetOrCreateAsyncLazy<T>(this IMemoryCache cache, object key,
            Func<Task<T>> LazyFactory, MemoryCacheEntryOptions options)
        {
            if (!cache.TryGetValue(key, out AsyncLazy<T> asyncLazy))
            {
                var entry = cache.CreateEntry(key);
                if (options != null) entry.SetOptions(options);
                var newAsyncLazy = new AsyncLazy<T>(LazyFactory);
                entry.Value = newAsyncLazy;
                entry.Dispose(); // Dispose inserts the entry in the cache

                if (!cache.TryGetValue(key, out asyncLazy)) asyncLazy = newAsyncLazy;
            }

            if (asyncLazy.Value.IsCompleted) return asyncLazy.Value;

            return asyncLazy.Value.ContinueWith(t => t,
                default, TaskContinuationOptions.RunContinuationsAsynchronously,
                TaskScheduler.Default).Unwrap();
        }

        public static Task<T> GetOrCreateAsyncLazy<T>(this IMemoryCache cache, object key,
            Func<Task<T>> LazyFactory, DateTimeOffset absoluteExpiration)
        {
            return cache.GetOrCreateAsyncLazy(key, LazyFactory,
                new MemoryCacheEntryOptions() { AbsoluteExpiration = absoluteExpiration });
        }

        public static Task<T> GetOrCreateAsyncLazy<T>(this IMemoryCache cache, object key,
            Func<Task<T>> LazyFactory, TimeSpan slidingExpiration)
        {
            return cache.GetOrCreateAsyncLazy(key, LazyFactory,
                new MemoryCacheEntryOptions() { SlidingExpiration = slidingExpiration });
        }
    }
}

