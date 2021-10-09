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
    public static class DataUpdaterAppBuilderExtensions
    {
        public static void StartDataUpdater(this IApplicationBuilder app)
        {
            IDataUpdaterService dataUpdater = app.ApplicationServices.GetRequiredService<IDataUpdaterService>();
            dataUpdater.Start();
        }
    }
}
