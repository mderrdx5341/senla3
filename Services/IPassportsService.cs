using System.Collections;
using System.Collections.Generic;
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
        public List<IPassport> GetPassports();
        /// <summary>
        /// Cписок всех записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory();
    }
}