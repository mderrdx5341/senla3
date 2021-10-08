using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Passports.Models;
using Passports.Services;

namespace Passports.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController
    {
        private readonly PassportService _passportServie;
        public PassportController(PassportService passportService)
        {
            _passportServie = passportService;
        }
        [HttpGet]
        public ActionResult<List<Passport>> List()
        {
            return _passportServie.GetPassports();
        }
    }
}
