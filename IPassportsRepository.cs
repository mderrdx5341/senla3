using Passports.Models;
using System.Collections.Generic;

namespace Passports
{
    internal interface IPassportsRepository
    {
        public void Add(Passport passport);
        public List<Passport> GetAll();
    }
}