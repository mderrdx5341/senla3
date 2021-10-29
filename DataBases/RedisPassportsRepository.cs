using Passports.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.DataBases
{
    /// <summary>
    /// Репозиторий паспортов работающий с Redis
    /// </summary>
    internal class RedisPassportsRepository : IPassportsRepository
    {
        private const string PassportKeys = "passports";
        private const string DateKeys = "dates";
        private readonly RedisDataBase _db;
        private readonly ISaverPassports _saverPassports;

        public RedisPassportsRepository(RedisDataBase db, ISaverPassports sp)
        {
            _saverPassports = sp;
            _db = db;
        }

        /// <summary>
        /// Возвращает имя репозитория
        /// </summary>
        public string Name => "Redis";

        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll()
        {
            return _db.GetObjects<Passport>(GetAllPassportsKeys().ToArray());
        }

        /// <summary>
        /// Получение списка записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory()
        {
            return _db.GetObjects<PassportHistory>(GetHistoryKeys().ToArray());
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
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport)
        {
            string key = CreatePassportKey(passport);
            AddPassportKey(key);
            AddHistoryRecord(passport);
            _db.SetObject<Passport>(key, passport);
        }

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Update(Passport passport)
        {
            AddHistoryRecord(passport);
            _db.SetObject<Passport>(CreatePassportKey(passport), passport);
        }

        private RedisKey[] GetAllPassportsKeys()
        {
            return _db.GetSetValuesAsKeys(PassportKeys);
        }

        private void AddPassportKey(string key)
        {
            _db.SetAddValue(PassportKeys, key);
        }

        private RedisKey[] GetHistoryKeys()
        {
            return _db.GetSetValuesAsKeys(DateKeys);
        }

        private void AddHistoryRecord(Passport passport)
        {
            string key = CreateHistoryKey(passport, passport.History.Last());
            AddHistoryKey(key);
            _db.SetObject<PassportHistory>(key, passport.History.Last());
        }

        private void AddHistoryKey(string key)
        {
            _db.SetAddValue(DateKeys, key);
        }
        private string CreatePassportKey(Passport passport)
        {
            return passport.Series + "-" + passport.Number;
        }

        private string CreateHistoryKey(Passport passport, PassportHistory passportHistory)
        {
            return CreatePassportKey(passport) + " - " + passportHistory.DateTimeChange.ToString();
        }
    }
}
