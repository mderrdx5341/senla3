using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Services
{
    /// <summary>
    /// Интерфес для сервиса обновления данных
    /// </summary>
    internal interface IDataUpdaterService
    {
        /// <summary>
        /// Запуск сервиса
        /// </summary>
        public void Start();
    }
}
