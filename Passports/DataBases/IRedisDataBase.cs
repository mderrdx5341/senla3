using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Passports.DataBases
{
    /// <summary>
    /// Интерфейс для работы с Redis
    /// </summary>
    internal interface IRedisDataBase
    {
        /// <summary>
        /// Добавление значения во множество
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAddValue(string key, string value);

        /// <summary>
        /// Возвращает множество как ключи
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisKey[] SetGetValuesAsKeys(string key);

        /// <summary>
        /// Сохранить объект как Строку JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void StringAddObject<T>(string key, T obj);

        /// <summary>
        /// Возвращает список объектов из JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<T> StringGetObjects<T>(RedisKey[] keys);

        /// <summary>
        /// Возвращает список объектов из JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Task<List<T>> StringGetObjectsAsync<T>(RedisKey[] keys);
    }
}