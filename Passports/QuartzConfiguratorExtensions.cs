using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports
{
    /// <summary>
    /// Возможность считывать время задания из конфига
    /// </summary>
    internal static class QuartzConfiguratorExtensions
    {
        /// <summary>
        /// Создание задачи и выполнение ее по времени из конфигурации
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="quartz"></param>
        /// <param name="config"></param>
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
        where T : IJob
        {
            string jobName = typeof(T).Name;

            var cronSchedule = config["TimeDownload"];

            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at TimeDownload");
            }

            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}
