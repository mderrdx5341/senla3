using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Passports.Models;

namespace Passports
{
    /// <summary>
    /// Репозиторий с паспортами
    /// </summary>
    internal class PassportsRepository : IPassportsRepository
    {
        private readonly DataBaseContext _ctx;
        public PassportsRepository(IServiceProvider serviceProvider)
        {
            _ctx = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataBaseContext>(); ;
        }
        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll()
        {
            return _ctx.Passports.Include(p => p.History).ToList();
        }
        /// <summary>
        /// Получение списка записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory()
        {
            return _ctx.PassportsHistory.ToList();
        }
        /// <summary>
        /// Обработать список паспортов
        /// </summary>
        /// <param name="passports"></param>
        public void SaveRange(List<Passport> passports)
        {
            List<Passport> dbPassports = _ctx.Passports.ToList();
            foreach (Passport passport in dbPassports)
            {
                Passport inPassport = passports.Where(
                    inP => inP.Series == passport.Series && inP.Number == passport.Number
                ).FirstOrDefault();

                if (inPassport == null)
                {
                    if (passport.IsActive == false)
                    {
                        Update(passport, true);
                    }
                } 
                else
                {
                    if (passport.IsActive == true)
                    {
                        Update(passport, false);                        
                    }
                    passports.Remove(inPassport);
                }
            }

            foreach (Passport p in passports)
            {
                Add(p);
            }
            _ctx.SaveChanges();
        }
        private void Add(Passport passport)
        {
            passport.Id = 0;
            passport.IsActive = false;
            passport.History.Add(
                new PassportHistory()
                {
                    Id = 0,
                    DateTimeChange = DateTime.Today,
                    ChangeType = PassportHistory.ChangeTypes.Add,
                    PassportId = passport.Id
                }
            );

            _ctx.Passports.Add(passport);
        }
        private void Update(Passport passport, bool newStatus)
        {
            if (newStatus == true)
            {
                passport.IsActive = true;
                passport.History.Add(
                    new PassportHistory()
                    {
                        Id = 0,
                        PassportId = passport.Id,
                        DateTimeChange = DateTime.Today,
                        ChangeType = PassportHistory.ChangeTypes.Active
                    }
                );
            }
            else
            {
                passport.IsActive = false;
                passport.History.Add(
                    new PassportHistory()
                    {
                        Id = 0,
                        PassportId = passport.Id,
                        DateTimeChange = DateTime.Today,
                        ChangeType = PassportHistory.ChangeTypes.NotActive
                    }
                );
            }
            _ctx.Passports.Update(passport);
        }
    }
}
