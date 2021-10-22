using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Объект для работы с базой данных
    /// </summary>
    internal class PostgreDataBase : DbContext
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Таблица с паспортами
        /// </summary>
        public DbSet<Passport> Passports { get; set; }
        /// <summary>
        /// Таблица с историей изменения паспартов
        /// </summary>
        public DbSet<PassportHistory> PassportsHistory { get; set; }

        public PostgreDataBase(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        /// <summary>
        /// Конфигурация БД postgreSQL
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = _configuration.GetConnectionString("PostgreDefaultConnection");
            optionsBuilder.UseNpgsql(connection);
        }
    }
}
