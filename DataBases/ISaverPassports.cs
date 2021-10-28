using Passports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.DataBases
{
    /// <summary>
    /// Интерфейс для реализации сохранения паспортов
    /// </summary>
    internal interface ISaverPassports
    {
        /// <summary>
        /// Сохранение паспортов
        /// </summary>
        /// <param name="repositry"></param>
        /// <param name="passports"></param>
        public void Save(IPassportsRepository repositry, List<IPassport> passports);
    }
}
