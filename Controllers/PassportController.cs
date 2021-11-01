using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Passports.Models;
using Passports.Services;
using System.Collections;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Passport>))]
        public async Task<IActionResult> GetPassportsAsync()
        {      
            var passports = await _passportsServie.GetPassportsAsync();
            return  Ok(passports);
        }

        /// <summary>
        /// Список всех записей истории
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PassportHistory>))]
        public async Task<IActionResult> GetHistory()
        {
            var passportsHistories = await _passportsServie.GetHistoryAsync();
            return Ok(passportsHistories);
        }
    }
}
