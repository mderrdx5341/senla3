using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Репозиторий паспортов работающий с Redis
    /// </summary>
    internal class RPassportsRepository : IPassportsRepository
    {
        private const string ListID = "passports";
        private readonly RedisDataBase _db;
        
        public RPassportsRepository(RedisDataBase db)
        {
            _db = db;
        }

        /// <summary>
        /// Получение списка паспортов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll()
        {
            List<Passport> passports = new List<Passport>();
            foreach (string passportID in GetKeys())
            {
                passports.Add(_db.GetObject<Passport>(passportID));
            }

            return passports;
        }

        /// <summary>
        /// Получение списка записей истории
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory()
        {
            throw new NotImplementedException();
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
            passport.History.Add(
                CreateHistoryRecord(passport, PassportStatus.Add)
            );
            string key = CreateKey(passport);
            AddKey(key);
            _db.SetObject<Passport>(CreateKey(passport), passport);
        }

        private void Update(Passport passport, bool newStatus)
        {
            passport.IsActive = newStatus;
            passport.History.Add(
                CreateHistoryRecord(
                    passport,
                    newStatus ? PassportStatus.Active : PassportStatus.NotActive
                )
            );

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
            HashSet<string> keys = new HashSet<string>();
            if (_db.KeyExists(ListID))
            {
                keys = _db.GetObject<HashSet<string>>(ListID);
            }
            return keys;
        }

        private void AddKey(string key)
        {
            HashSet<string> keys = GetKeys();
            keys.Add(key);
            _db.SetObject<HashSet<string>>(ListID, keys);
        }
    }
}
