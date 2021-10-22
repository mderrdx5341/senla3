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

        /// <summary>
        /// Добавляет статус что паспорт новый
        /// </summary>
        public void AddHistoryRecordWhatsNew()
        {
            History.Add(
                CreateHistoryRecord(PassportStatus.Add)
            );
        }

        /// <summary>
        /// Меняет статус паспорта
        /// </summary>
        public void changeStatus()
        {
            IsActive = !IsActive;
            History.Add(
                CreateHistoryRecord(IsActive ? PassportStatus.Active : PassportStatus.NotActive)
            );
        }

        private PassportHistory CreateHistoryRecord(PassportStatus status)
        {
            return new PassportHistory()
            {
                Id = 0,
                PassportId = Id,
                DateTimeChange = DateTime.Today,
                ChangeType = status
            };
        }
    }
}
