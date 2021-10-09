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
    internal class PassportsService : IPassportsService
    {
        private IPassportsRepository _passportsRepository;
        public PassportsService(IPassportsRepository passportsRepository)
        {
            _passportsRepository = passportsRepository;
        }
        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetPassports()
        {
            return _passportsRepository.GetAll();
        }
    }
}
