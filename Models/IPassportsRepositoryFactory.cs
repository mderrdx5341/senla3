using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Интерфейс для фабрик паспортов
    /// </summary>
    internal interface IPassportsRepositoryFactory
    {
        /// <summary>
        /// Возвращает список имен репозиториев
        /// </summary>
        /// <returns></returns>
        public List<string> GetRepositoriesNames();

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
