using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
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
        public List<IPassport> GetAll()
        {
            return _db.GetObjects<PassportDTO>(GetAllPassportsKeys().ToArray()).ToList<IPassport>();
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
            _saverPassports.Save(this, passports);
        }

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(IPassport passport)
        {
            string key = CreateKey(passport);
            AddPassportKey(key);
            AddHistoryRecord(passport, passport.History.Last());
            _db.SetObject<IPassport>(key, passport);
        }

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        /// <param name="newStatus"></param>
        public void Update(IPassport passport)
        {
            AddHistoryRecord(passport, passport.History.Last());
            _db.SetObject<IPassport>(CreateKey(passport), passport);
        }

        private string CreateKey(IPassport passport)
        {
            return passport.Series + "-" + passport.Number;
        }

        private HashSet<string> GetAllPassportsKeys()
        {
            return GetDataset(PassportKeys);
        }

        private void AddPassportKey(string key)
        {
            AddValueToDataset(PassportKeys, key);
        }

        private HashSet<string> GetHistoryKeys()
        {
            return GetDataset(DateKeys);
        }

        private void AddHistoryRecord(IPassport passport, PassportHistory record)
        {
            string key = CreateHistoryKey(passport, record);
            AddHistoryKey(key);
            _db.SetObject<PassportHistory>(key, record);
        }

        private void AddHistoryKey(string key)
        {
            AddValueToDataset(DateKeys, key);
        }

        private string CreateHistoryKey(IPassport passport, PassportHistory passportHistory)
        {
            return CreateKey(passport) + " - " + passportHistory.DateTimeChange.ToString();
        }

        private HashSet<string> GetDataset(string name)
        {
            HashSet<string> keys = new HashSet<string>();
            if (_db.KeyExists(name))
            {
                keys = _db.GetObject<HashSet<string>>(name);
            }
            return keys;
        }

        private void AddValueToDataset(string name, string value)
        {
            HashSet<string> keys = GetDataset(name);
            keys.Add(value);
            _db.SetObject<HashSet<string>>(name, keys);
        }
    }
}
