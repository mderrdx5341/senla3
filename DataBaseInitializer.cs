using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passports.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public static void InitDataBase(this IServiceCollection services, IConfiguration config)
        {
            if (config["Database"] == "Redis")
            {
                var multiplexer = ConnectionMultiplexer.Connect("localhost");
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
                services.AddScoped<RedisDataBase, RedisDataBase>();
                services.AddScoped<IPassportsRepository, RedisPassportsRepository>();
            }
            else if (config["Database"] == "PostgreSQL")
            {
                string connection = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(connection));
                services.AddScoped<IPassportsRepository, PostgrePassportsRepository>();
            }
            else
            {
                throw new Exception($"Not database selected. Use option \"Database\": \"PostgreSQL\" or \"Database\": \"Redis\"");
            }
        }
    }
}
