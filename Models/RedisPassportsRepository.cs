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
        
        public RedisPassportsRepository(RedisDataBase db)
        {
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
            return _db.GetObjects<Passport>(GetKeys().ToArray());
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
            List<Passport> dbPassports = GetAll();
            foreach (Passport passport in dbPassports)
            {
                Passport coincidentPassport = passports.Where(
                    p => p.Series == passport.Series && p.Number == passport.Number
                ).FirstOrDefault();

                if (coincidentPassport == null)
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
                    passports.Remove(coincidentPassport);
                }
            }

            foreach (Passport p in passports)
            {
                Add(p);
            }
        }

        private void Add(Passport passport)
        {
            passport.Id = 0;
            passport.IsActive = false;
            PassportHistory record = CreateHistoryRecord(passport, PassportStatus.Add);
            passport.History.Add(
                record
            );
            string key = CreateKey(passport);
            AddKey(key);
            AddHistoryRecord(passport, record);
            _db.SetObject<Passport>(key, passport);
        }

        private void Update(Passport passport, bool newStatus)
        {
            passport.IsActive = newStatus;
            PassportHistory record = CreateHistoryRecord(
                    passport,
                    newStatus ? PassportStatus.Active : PassportStatus.NotActive
            );
            passport.History.Add(
               record
            );
            AddHistoryRecord(passport, record);
            _db.SetObject<Passport>(CreateKey(passport), passport);
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

        private string CreateKey(Passport passport)
        {
            return passport.Series + "-" + passport.Number;
        }

        private HashSet<string> GetKeys()
        {
            return GetSet(PassportKeys);
        }

        private void AddKey(string key)
        {
            AddValueToSet(PassportKeys, key);
        }

        private HashSet<string> GetHistoryKeys()
        {
            return GetSet(DateKeys);
        }

        private void AddHistoryRecord(Passport passport, PassportHistory record)
        {
            string key = CreateHistoryKey(passport, record);
            AddHistoryKey(key);
            _db.SetObject<PassportHistory>(key, record);
        }

        private void AddHistoryKey(string key)
        {
            AddValueToSet(DateKeys, key);
        }

        private string CreateHistoryKey(Passport passport, PassportHistory passportHistory)
        {
            return CreateKey(passport) + " - " + passportHistory.DateTimeChange.ToString();
        }

        private HashSet<string> GetSet(string name)
        {
            HashSet<string> keys = new HashSet<string>();
            if (_db.KeyExists(name))
            {
                keys = _db.GetObject<HashSet<string>>(name);
            }
            return keys;
        }

        private void AddValueToSet(string name, string value)
        {
            HashSet<string> keys = GetSet(name);
            keys.Add(value);
            _db.SetObject<HashSet<string>>(name, keys);
        }
    }
}
