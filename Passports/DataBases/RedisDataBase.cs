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

        public RedisDataBase(IConnectionMultiplexer multiplexer)
        {
            _db = multiplexer.GetDatabase();
        }

        /// <summary>
        /// Добавление значения во множество
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetAddValue(string key, string value)
        {
            _db.SetAddAsync(key, value).ConfigureAwait(false);
        }

        /// <summary>
        /// Возвращает множество как ключи
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual RedisKey[] SetGetValuesAsKeys(string key)
        {
            return Array.ConvertAll(
                _db.SetScan(key).ToArray(),
                redisValue => (RedisKey)((string)redisValue)
            );
        }

        /// <summary>
        /// Сохранить объект как Строку JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public virtual void StringAddObject<T>(string key, T obj)
        {
            _db.StringSetAsync(key, JsonSerializer.Serialize<T>(obj)).ConfigureAwait(false);
        }

        /// <summary>
        /// Возратить объект из JSON строки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGetObject<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(_db.StringGet(key));
        }

        /// <summary>
        /// Возвращает список объектов из JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual List<T> StringGetObjects<T>(RedisKey[] keys)
        {
            List<T> objsFromJson = new List<T>();
            foreach (string obJSON in _db.StringGet(keys))
            {
                objsFromJson.Add(JsonSerializer.Deserialize<T>(obJSON));
            }
            return objsFromJson;
        }

        /// <summary>
        /// Возвращает список объектов из JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<List<T>> StringGetObjectsAsync<T>(RedisKey[] keys)
        {
            List<T> objsFromJson = new List<T>();
            RedisValue[] objsJson = await _db.StringGetAsync(keys).ConfigureAwait(false);
            foreach (string obJSON in objsJson) {
                objsFromJson.Add(JsonSerializer.Deserialize<T>(obJSON));
            }
            return objsFromJson;
        }
    }
}
