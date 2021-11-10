using Passports.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Интерфейс репозитория для паспортов
    /// </summary>
    internal interface IPassportsRepository
    {
        /// <summary>
        /// Возвращает имя репозитория
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Получение списка паспартов
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetAll();

        /// <summary>
        /// Асинхронное получение списка паспартов
        /// </summary>
        /// <returns></returns>
        public Task<List<Passport>> GetAllAsync();

        /// <summary>
        /// Получение списка записей историй
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory();

        /// <summary>
        /// Асинхронное получение списка записей историй
        /// </summary>
        /// <returns></returns>
        public Task<List<PassportHistory>> GetHistoryAsync();

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(Passport passport);

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Update(Passport passport);

        /// <summary>
        /// Обработать список паспортов
        /// </summary>
        /// <param name="passports"></param>
        void SaveRange(List<Passport> passports);

        /// <summary>
        /// Асинхронная обработать список паспортов
        /// </summary>
        /// <param name="passports"></param>
        void SaveRangeAsync(List<Passport> passports);
    }
}