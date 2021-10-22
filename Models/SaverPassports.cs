using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
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
            List<Passport> dbPassports = repositry.GetAll();
            foreach (Passport passport in dbPassports)
            {
                Passport coincidentPassport = passports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport == null)
                {
                    if (passport.IsActive == false)
                    {
                        passport.changeStatus();
                        repositry.Update(passport);
                    }
                }
                else
                {
                    if (passport.IsActive == true)
                    {
                        passport.changeStatus();
                        repositry.Update(passport);
                    }
                    passports.Remove(coincidentPassport);
                }
            }

            foreach (Passport p in passports)
            {
                p.Id = 0;
                p.IsActive = false;
                p.AddHistoryRecordWhatsNew();
                repositry.Add(p);
            }
        }
    }
}
