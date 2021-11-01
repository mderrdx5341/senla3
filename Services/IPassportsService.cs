using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Passports.Models;

namespace Passports.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с паспортами
    /// </summary>
    internal interface IPassportsService
    {
        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetPassports();

        /// <summary>
        /// Список всех паспортов
        /// </summary>
        /// <returns></returns>
        public Task<List<Passport>> GetPassportsAsync();

        /// <summary>
        /// Cписок всех записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory();

        /// <summary>
        /// Cписок всех записей истории
        /// </summary>
        /// <returns></returns>
        public Task<List<PassportHistory>> GetHistoryAsync();
    }
}