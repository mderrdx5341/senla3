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
        public void Save(IPassportsRepository repositry, List<Models.Passport> passports)
        {
            List<IPassport> dbPassports = repositry.GetAll();
            foreach (Passport passport in dbPassports)
            {
                Models.Passport coincidentPassport = passports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport == null)
                {
                    if (passport.IsActive == false)
                    {
                        repositry.Update(
                            new Models.Passport(passport).changeStatus().ReturnData()
                        );
                    }
                }
                else
                {
                    if (passport.IsActive == true)
                    {                        
                        repositry.Update(
                            new Models.Passport(passport).changeStatus().ReturnData()
                        );
                    }
                    passports.Remove(coincidentPassport);
                }
            }

            foreach (Models.Passport p in passports)
            {
                p.Id = 0;
                p.IsActive = false;
                p.AddHistoryRecordWhatsNew();
                repositry.Add(p.ReturnData());
            }
        }
    }
}
