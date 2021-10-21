using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Passports.Models
{
    /// <summary>
    /// Репозиторий паспортов работающий с PostgreSQL
    /// </summary>
    internal class PostgrePassportsRepository : IPassportsRepository
    {
        private readonly DataBaseContext _ctx;
        private readonly ISaverPassports _saverPassport;
        private bool isEnabledSave = true;

        public PostgrePassportsRepository(DataBaseContext ctx, ISaverPassports sp)
        {
            _saverPassport = sp;
            _ctx = ctx;
        }

        /// <summary>
        /// Возвращает имя репозитория
        /// </summary>
        public string Name => "PostgreSQL";
        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll()
        {
            return _ctx.Passports.ToList();
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
            isEnabledSave = false;
            _saverPassport.Save(this, passports);
            _ctx.SaveChanges();
            isEnabledSave = true;
        }

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport)
        {
            passport.Id = 0;
            passport.IsActive = false;
            passport.History.Add(
                CreateHistoryRecord(passport, PassportStatus.Add)
            );

            _ctx.Passports.Add(passport);
            if (isEnabledSave)
            {
                _ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        /// <param name="newStatus"></param>
        public void Update(Passport passport, bool newStatus)
        {
            passport.IsActive = newStatus;
            passport.History.Add(
                CreateHistoryRecord(
                    passport,
                    newStatus ? PassportStatus.Active : PassportStatus.NotActive
                )
            );
            _ctx.Passports.Update(passport);
            if (isEnabledSave)
            {
                _ctx.SaveChanges();
            }
        }

        private PassportHistory CreateHistoryRecord(Passport passport, PassportStatus status)
        {
            return new PassportHistory()
            {
                Id = 0,
                PassportId = passport.Id,
                DateTimeChange = DateTime.Today,
                ChangeType = status
            };
        }
    }
}
