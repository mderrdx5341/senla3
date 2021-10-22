using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Паспорт
    /// </summary>
    internal class PassportDTO : IPassport
    {
        /// <summary>
        /// Id паспорта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Активность паспорта
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Серия паспорта
        /// </summary>
        public int Series { get; set; }
        /// <summary>
        /// Номер паспорта
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// История изменения паспорта
        /// </summary>
        public List<PassportHistory> History { get; set; } = new List<PassportHistory>();
    }
}
