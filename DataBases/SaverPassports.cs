using Passports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.DataBases
{     
    /// <summary>
    /// Реализация алгоритма сохранения паспортов
    /// </summary>
    internal class SaverPassports : ISaverPassports
    {
        /// <summary>
        /// Формирование списка паспортов для сохранения в репозиторий
        /// </summary>
        public Dictionary<Passport, OperationRepository> ChangeForDataBase(List<Passport> repositoryPassports, List<Passport> newPassports)
        {
            Dictionary<Passport, OperationRepository> toRepository = new Dictionary<Passport, OperationRepository>();

            foreach (Passport passport in repositoryPassports)
            {
                Passport coincidentPassport = newPassports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport != null)
                {
                    newPassports.Remove(coincidentPassport);
                }

                if (
                    (coincidentPassport == null && passport.IsActive == false)
                    || (coincidentPassport != null && passport.IsActive == true)
                ) {
                    ChangeStatus(passport);
                    toRepository.Add(passport, OperationRepository.Update);
                }
            }

            foreach (Passport p in newPassports)
            {
                p.IsActive = false;
                AddHistoryRecordWhatsNew(p);
                toRepository.Add(p, OperationRepository.Add);
            }

            return toRepository;
        }

        /// <summary>
        /// Добавляет статус что паспорт новый
        /// </summary>
        private void AddHistoryRecordWhatsNew(Passport p)
        {
            p.History.Add(
                CreateHistoryRecord(PassportStatus.Add)
            );
        }

        private PassportHistory CreateHistoryRecord(PassportStatus status)
        {
            return new PassportHistory()
            {
                DateTimeChange = DateTime.Today,
                ChangeType = status
            };
        }

        /// <summary>
        /// Меняет статус паспорта
        /// </summary>
        private void ChangeStatus(Passport p)
        {
            p.IsActive = !p.IsActive;
            p.History.Add(
                CreateHistoryRecord(p.IsActive ? PassportStatus.Active : PassportStatus.NotActive)
            );
        }

    }
}
