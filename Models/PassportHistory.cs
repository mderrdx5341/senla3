using Passports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Хранит изменене статуса паспорта
    /// </summary>
    internal class PassportHistory
    {
        /// <summary>
        /// Id записи истории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата изменения паспорта
        /// </summary>
        public DateTime DateTimeChange { get; set; }

        /// <summary>
        /// Тип изменения записи
        /// </summary>
        public PassportStatus ChangeType { get; set; }
    }
}
