using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Passports.Models;

namespace Passports.DataBases
{
    /// <summary>
    /// Репозиторий паспортов работающий с PostgreSQL
    /// </summary>
    internal class PostgrePassportsRepository : IPassportsRepository
    {
        private readonly PostgreDataBase _ctx;
        private readonly ISaverPassports _saverPassports;

        public PostgrePassportsRepository(PostgreDataBase ctx, ISaverPassports sp)
        {
            _saverPassports = sp;
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
            foreach (KeyValuePair<Passport, OperationRepository> passportEntry in _saverPassports.ChangeForRepository(GetAll(), passports))
            {
                if (passportEntry.Value == OperationRepository.Add)
                {
                    _ctx.Add(passportEntry.Key);
                }
                if (passportEntry.Value == OperationRepository.Update)
                {
                    _ctx.Update(passportEntry.Key);
                }
            }
            _ctx.SaveChanges();
        }

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport)
        {
            _ctx.Passports.Add(passport);
        }

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        /// <param name="newStatus"></param>
        public void Update(Passport passport)
        {
            _ctx.Passports.Update((Passport)passport);
        }
    }
}
