using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Интерфейс для фабрик паспортов
    /// </summary>
    internal interface IPassportRepositoryFactory
    {
        /// <summary>
        /// Возвращает репозиторий по имени базы данных
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPassportsRepository GetByName(string name);

        /// <summary>
        /// Возвращает репозиторий выбранный по умолчанию в настройках
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPassportsRepository GetDefaultRepository();
    }
}
