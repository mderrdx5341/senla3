using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    interface IPassportHistory
    {
        /// <summary>
        /// id для записи истории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Время изменения записи паспорта
        /// </summary>
        public DateTime DateTimeChange { get; set; }

        /// <summary>
        /// Тип изменения записи
        /// </summary>
        public PassportStatus ChangeType { get; set; }
    }
}
