using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passports.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports
{
    /// <summary>
    /// Расширение класса для сборки приложения
    /// </summary>
    public static class DataUpdaterAppBuilderExtensions
    {
        /// <summary>
        /// Старт обновления данных
        /// </summary>
        /// <param name="app"></param>
        public static void StartDataUpdater(this IApplicationBuilder app)
        {
            IDataUpdaterService dataUpdater = app.ApplicationServices.GetRequiredService<IDataUpdaterService>();
            dataUpdater.Start();
        }
    }
}
