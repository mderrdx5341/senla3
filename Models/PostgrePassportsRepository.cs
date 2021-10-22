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
        private readonly PostgreDataBase _ctx;
        private readonly ISaverPassports _saverPassports;
        private bool isEnabledSave = true;

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
        public List<IPassport> GetAll()
        {
            return _ctx.Passports.Include(p => p.History).ToList<IPassport>();
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
            _saverPassports.Save(this, passports);
            _ctx.SaveChanges();
            isEnabledSave = true;
        }

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(IPassport passport)
        {
            _ctx.Passports.Add((PassportDTO)passport);
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
        public void Update(IPassport passport)
        {
            _ctx.Passports.Update((PassportDTO)passport);
            if (isEnabledSave)
            {
                _ctx.SaveChanges();
            }
        }
    }
}
