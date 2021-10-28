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
        /// Сохранение паспортов
        /// </summary>
        public void Save(IPassportsRepository repositry, List<Passport> passports)
        {
            List<IPassport> dbPassports = repositry.GetAll();
            foreach (Passport passport in dbPassports)
            {
                Passport coincidentPassport = passports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport == null)
                {
                    if (passport.IsActive == false)
                    {
                        ChangeStatus(passport);
                        repositry.Update(
                            passport
                        );
                    }
                }
                else
                {
                    if (passport.IsActive == true)
                    {
                        ChangeStatus(passport);
                        repositry.Update(
                            passport
                        );
                    }
                    passports.Remove(coincidentPassport);
                }
            }

            foreach (Passport p in passports)
            {
                p.IsActive = false;
                AddHistoryRecordWhatsNew(p);
                repositry.Add(p);
            }
        }

        /// <summary>
        /// Добавляет статус что паспорт новый
        /// </summary>
        private void AddHistoryRecordWhatsNew(IPassport p)
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
        private void ChangeStatus(IPassport p)
        {
            p.IsActive = !p.IsActive;
            p.History.Add(
                CreateHistoryRecord(p.IsActive ? PassportStatus.Active : PassportStatus.NotActive)
            );
        }

    }
}
