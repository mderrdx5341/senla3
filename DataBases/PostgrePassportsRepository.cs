using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        public PostgrePassportsRepository(IConfiguration configuration, ISaverPassports sp)
        {
            _saverPassports = sp;
            _ctx = new PostgreDataBase(configuration);
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
        /// Асинхронное получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public async Task<List<Passport>> GetAllAsync()
        {
            return await _ctx.Passports.Include(p => p.History).ToListAsync().ConfigureAwait(false);
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
        /// Получение списка записей истории
        /// </summary>
        /// <returns></returns>
        public async Task<List<PassportHistory>> GetHistoryAsync()
        {
            return await _ctx.PassportsHistory.ToListAsync().ConfigureAwait(false);
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
                    Add(passportEntry.Key);
                }
                if (passportEntry.Value == OperationRepository.Update)
                {
                    Update(passportEntry.Key);
                }
            }
        }

        /// <summary>
        /// Обработать список паспортов
        /// </summary>
        /// <param name="passports"></param>
        public async void SaveRangeAsync(List<Passport> passports)
        {
            await _mutex.WaitAsync();
            try
            {
                await Task.Run(() => SaveRange(passports));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _mutex.Release();
        }

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport)
        {
            passport.Id = 0;
            _ctx.Passports.Add(passport);
            _ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Update(Passport passport)
        {
            _ctx.Passports.Update((Passport)passport);
            _ctx.SaveChangesAsync();
        }
    }
}
