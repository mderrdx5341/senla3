using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Passports.Models;

namespace Passports.Services
{
    public class PassportService
    {
        public List<Passport> GetPassports()
        {
            return PassportRepository.GetAll();
        }
    }
}
