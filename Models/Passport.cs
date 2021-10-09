using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Паспорт
    /// </summary>
    internal class Passport
    {
        /// <summary>
        /// Серия паспорта
        /// </summary>
        public int Series { get; set; }
        /// <summary>
        /// Номер паспорта
        /// </summary>
        public int Number { get; set; }
    }
}
