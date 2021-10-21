using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    internal class PassportRepositoryFactory : IPassportRepositoryFactory
    {
        private readonly Dictionary<string, IPassportsRepository> _passprortRepositories = new Dictionary<string, IPassportsRepository>();
        private readonly string _defaultRepositoryName;
        public PassportRepositoryFactory(IEnumerable<IPassportsRepository> passprortRepositories, IConfiguration configuration)
        {
            _defaultRepositoryName = configuration["Database"];
            foreach (IPassportsRepository repository in passprortRepositories)
            {
                _passprortRepositories.Add(repository.Name, repository);
            }
        }

        /// <summary>
        /// Возвращает репозиторий по умолчанию
        /// </summary>
        /// <returns></returns>
        public IPassportsRepository GetDefaultRepository()
        {
            try
            {
                return GetByName(_defaultRepositoryName);
            }
            catch(Exception)
            { 
                throw new Exception($"Not database selected. Use option \"Database\": \"PostgreSQL\" or \"Database\": \"Redis\" in appsettings.json");
            }
        }

        /// <summary>
        /// Возвращает репозиторий по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPassportsRepository GetByName(string name)
        {
            try
            {
                return _passprortRepositories[name];
            }
            catch(Exception)
            {
                throw new Exception($"PassportsRepository \"{name}\" does not exist");
            }
        }
    }
}
