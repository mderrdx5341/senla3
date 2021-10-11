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
        /// Добавление паспорта
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport);
        /// <summary>
        /// Получение списка паспартов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll();
    }
}