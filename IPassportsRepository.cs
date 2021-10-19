using Passports.Models;
using System.Collections.Generic;

namespace Passports
{
    /// <summary>
    /// Интерфейс репозитория для паспортов
    /// </summary>
    internal interface IPassportsRepository
    {
        /// <summary>
        /// Получение списка паспартов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll();
        /// <summary>
        /// Получение списка записей историй
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory();
        /// <summary>
        /// Обработать список паспартов
        /// </summary>
        /// <param name="passports"></param>
        void SaveRange(List<Passport> passports);
    }
}