using Passports.Models;
using System.Collections.Generic;

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
        public List<IPassport> GetAll();

        /// <summary>
        /// Получение списка записей историй
        /// </summary>
        /// <returns></returns>
        public List<PassportHistory> GetHistory();

        /// <summary>
        /// Добавить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Add(IPassport passport);

        /// <summary>
        /// Обновить паспорт
        /// </summary>
        /// <param name="passport"></param>
        public void Update(IPassport passport);

        /// <summary>
        /// Обработать список паспортов
        /// </summary>
        /// <param name="passports"></param>
        void SaveRange(List<Passport> passports);
    }
}