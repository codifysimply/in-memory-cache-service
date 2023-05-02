using CS.Services.CacheMemoryService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.CacheMemoryService.Extensions
{
    public static class CSMemoryCacheServiceExtensions
    {
        public static IServiceCollection AddCSMemoryCacheService(this IServiceCollection services)
        {
            return services.AddSingleton<ICSMemoryCacheService, CSMemoryCacheService>();
        }
    }
}
