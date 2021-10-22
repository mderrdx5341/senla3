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
        private readonly IPassportsRepository _passportsRepository;

        public PassportsService(IPassportsRepositoryFactory passportsRepositoryFactory)
        {
            _passportsRepository = passportsRepositoryFactory.GetDefaultRepository();
        }

        /// <summary>
        /// Список всех записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory()
        {
            return _passportsRepository.GetHistory();
        }

        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public List<IPassport> GetPassports()
        {
            return _passportsRepository.GetAll();
        }
    }
}
