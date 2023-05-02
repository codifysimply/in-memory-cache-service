
using CS.MemoryCache.WebApi.Services;
using CS.MemoryCache.WebApi.Services.Interfaces;
using CS.Services.CacheMemoryService.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CS.MemoryCache.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddCSMemoryCacheService();

            var configuration = builder.Configuration;
            builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EmployeeConnection")));

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}