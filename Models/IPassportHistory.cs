using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    interface IPassportHistory
    {
        /// <summary>
        /// Id записи в истори
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id паспорта
        /// </summary>
        public int PassportId { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateTimeChange { get; set; }
        /// <summary>
        /// Тип изменения записи
        /// </summary>
        public PassportStatus ChangeType { get; set; }
    }
}
