using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Passports.Models
{
    /// <summary>
    /// Класс для работы с Redis
    /// </summary>
    internal class RedisDataBase
    {
        private readonly IDatabase _db;

        public RedisDataBase(IConnectionMultiplexer redis, int databaseIndex = -1)
        {
            _db = redis.GetDatabase(databaseIndex);
        }

        /// <summary>
        /// Проверить существование ключа
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return _db.KeyExists(key);
        }

        /// <summary>
        /// Сохранить объект как Строку JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetObject<T>(string key, T obj)
        {
            _db.StringSet(key, JsonSerializer.Serialize<T>(obj));
        }

        /// <summary>
        /// Возратить объект из JSON строки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetObject<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(_db.StringGet(key));
        }
        public List<T> GetObjects<T>(string[] keys)
        {
            RedisKey[] convertedKeys = Array.ConvertAll(keys, key => (RedisKey)key);
            List<T> objs = new List<T>();
            foreach (string obJSON in _db.StringGet(convertedKeys)) {
                 objs.Add(JsonSerializer.Deserialize<T>(obJSON));
            }
            return objs;
        }
        ~RedisDataBase()
        {

        }
    }
}
