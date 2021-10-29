using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Passports.DataBases
{
    /// <summary>
    /// Класс для работы с Redis
    /// </summary>
    internal class RedisDataBase
    {
        private readonly IDatabase _db;

        public RedisDataBase(IConfiguration configuration)
        {
            var multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisDefaultConnection"));
            _db = multiplexer.GetDatabase();
        }

        /// <summary>
        /// Добавление значения во множество
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAddValue(string key, string value)
        {
            _db.SetAdd(key, value);
        }

        /// <summary>
        /// Возвращает множество как ключи
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisKey[] GetSetValuesAsKeys(string key)
        {
            return Array.ConvertAll(
                _db.SetScan(key).ToArray(),
                i => (RedisKey)(string)i
            );
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

        /// <summary>
        /// Возвращает список объектов из JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<T> GetObjects<T>(RedisKey[] keys)
        {
            List<T> objsFromJson = new List<T>();
            foreach (string obJSON in _db.StringGet(keys)) {
                objsFromJson.Add(JsonSerializer.Deserialize<T>(obJSON));
            }
            return objsFromJson;
        }
    }
}
