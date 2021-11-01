using Passports.DataBases;
using Passports.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace Passports
{
    /// <summary>
    /// Класс для инициализации базы данных
    /// </summary>
    internal static class DataBaseInitializerExtensions
    {
        /// <summary>
        /// Инициализируюет выбраную базу данных в настройках
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void InitDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["Database"] == "Redis")
            {
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("RedisDefaultConnection")
                );
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
                services.AddSingleton<RedisDataBase>();
                services.AddSingleton<IPassportsRepository, RedisPassportsRepository>();
            }
            else if (configuration["Database"] == "PostgreSQL")
            {
                services.AddDbContext<PostgreDataBase>(options => options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"))
                );
                services.AddScoped<IPassportsRepository, PostgrePassportsRepository>();
            }
            else
            {
                throw new Exception($"Not database selected. Use option \"Database\" in appsettings.json.");
            }
        }
    }
}
