using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Passports.Models;

namespace Passports
{
    public class PassportRepository
    {
        private static readonly List<Passport> _passports = new List<Passport>();

        public static void Add(Passport passport)
        {
            _passports.Add(passport);
        }

        public static List<Passport> GetAll()
        {
            return _passports;
        }
    }
}
