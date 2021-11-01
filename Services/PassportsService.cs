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

        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public async Task<List<Passport>> GetPassportsAsync()
        {
            return await _passportsRepository.GetAllAsync();
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
        /// Список всех записей истории
        /// </summary>
        /// <returns></returns>
        public async Task<List<PassportHistory>> GetHistoryAsync()
        {
            return await _passportsRepository.GetHistoryAsync();
        }
    }
}
