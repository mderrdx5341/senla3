using System;
using System.Collections.Generic;

namespace Passports.Models
{
    /// <summary>
    /// Паспорт
    /// </summary>
    internal class Passport : IPassport
    {
        public Passport()
        {
        }

        public Passport(IPassport p)
        {
            Series = p.Series;
            Number = p.Number;
            IsActive = p.IsActive;
            History = p.History;
        }

        /// <summary>
        /// Id для записи паспорта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Статус активности паспорта
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
