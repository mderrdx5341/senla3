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
        /// Формирование списка паспортов для сохранения в репозиторий
        /// </summary>
        /// <param name="repositoryPassports"></param>
        /// <param name="newPassports"></param>
        /// <returns></returns>
        public Dictionary<Passport, OperationRepository> ChangeForRepository(List<Passport> repositoryPassports, List<Passport> newPassports);
    }
}
