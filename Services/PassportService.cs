using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Passports.Models;

namespace Passports.Services
{
    /// <summary>
    /// Сервис для предоставления данных о паспортах
    /// </summary>
    internal class PassportService : IPassportsService
    {
        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetPassports()
        {
            return PassportRepository.GetAll();
        }
    }
}
