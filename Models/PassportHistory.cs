using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    internal class PassportHistory
    {
        /// <summary>
        /// Возможные виды изменения записи
        /// </summary>
        public enum ChangeTypes
        {
            Add,
            Active,
            NotActive
        }
        /// <summary>
        /// Id записи в истори
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id паспорта
        /// </summary>
        public int PassportId { get; set; }
        /*public Passport Passport { get; set; }*/
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateTimeChange { get; set; }
        /// <summary>
        /// Тип изменения записи
        /// </summary>
        public ChangeTypes ChangeType { get; set; }
    }
}
