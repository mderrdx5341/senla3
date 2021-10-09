using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Passports.Models;
using Passports.Services;

namespace Passports.Controllers
{
    /// <summary>
    /// Контроллер для работы с паспортами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly IPassportsService _passportsServie;
        public PassportController(IServiceProvider serviceProvider)
        {
            _passportsServie = serviceProvider.GetRequiredService<IPassportsService>();
        }
        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Passport>> List()
        {
            return _passportsServie.GetPassports();
        }
    }
}
