using CS.MemoryCache.WebApi.Entities;
using CS.MemoryCache.WebApi.Services.Interfaces;
using CS.Services.CacheMemoryService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CS.MemoryCache.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IMemoryCache memoryCache;
        private readonly ICSMemoryCacheService cSMemoryCacheService;
        public EmployeeController(IMemoryCache memoryCache, 
            IEmployeeService employeeService, 
            ICSMemoryCacheService cSMemoryCacheService)
        {
            this.memoryCache = memoryCache;
            this.employeeService = employeeService;
            this.cSMemoryCacheService = cSMemoryCacheService; 
        }

        [HttpGet("employees")]
        public async Task<IEnumerable<Employee>> GelAllEmployees()
        {
            IEnumerable<Employee> cacheEmployees;

            if (!memoryCache.TryGetValue("cacheEmployees", out cacheEmployees))
            {
                cacheEmployees = await employeeService.GetEmployees();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(100))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(200))
                    .SetPriority(CacheItemPriority.NeverRemove)
                    .SetSize(1024);

                memoryCache.Set("cacheEmployees", cacheEmployees, cacheEntryOptions);
            }

            memoryCache.TryGetValue("cacheEmployees", out cacheEmployees);
            return cacheEmployees;
        }

        [HttpGet("cache")]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var items = await cSMemoryCacheService.GetOrCreateAsyncLazy<IEnumerable<Employee>>("employees", async () =>
            {
                var employees = await employeeService.GetEmployees();
                return employees;
            }, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) });
            return items;
        }
    }
}
