using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Passports.Models;

namespace Passports
{
    /// <summary>
    /// Репозиторий с паспортами
    /// </summary>
    internal class PassportsRepository : IPassportsRepository
    {
        private readonly List<Passport> _passports = new List<Passport>();
        /// <summary>
        /// Добавление паспорта в список
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport)
        {
            _passports.Add(passport);
        }
        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll()
        {
            return _passports;
        }
    }
}
