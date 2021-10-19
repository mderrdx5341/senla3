using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Объект для работы с базой данных
    /// </summary>
    internal class DataBaseContext : DbContext
    {
        /// <summary>
        /// Таблица с паспортами
        /// </summary>
        public DbSet<Passport> Passports { get; set; }
        /// <summary>
        /// Таблица с историей изменения паспартов
        /// </summary>
        public DbSet<PassportHistory> PassportsHistory { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
