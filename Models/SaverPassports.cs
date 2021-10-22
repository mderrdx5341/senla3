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
            List<IPassport> dbPassports = repositry.GetAll();
            foreach (PassportDTO passport in dbPassports)
            {
                Passport coincidentPassport = passports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport == null)
                {
                    if (passport.IsActive == false)
                    {
                        repositry.Update(
                            new Passport(passport).changeStatus().createDTO()
                        );
                    }
                }
                else
                {
                    if (passport.IsActive == true)
                    {                        
                        repositry.Update(
                            new Passport(passport).changeStatus().createDTO()
                        );
                    }
                    passports.Remove(coincidentPassport);
                }
            }

            foreach (Passport p in passports)
            {
                p.Id = 0;
                p.IsActive = false;
                p.AddHistoryRecordWhatsNew();
                repositry.Add(p.createDTO());
            }
        }
    }
}
