using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passports.Models
{
    /// <summary>
    /// Фабрика для репозиториев с пасортами
    /// </summary>
    internal class PassportsRepositoryFactory : IPassportsRepositoryFactory
    {
        private readonly Dictionary<string, IPassportsRepository> _passprortRepositories = new Dictionary<string, IPassportsRepository>();
        private readonly List<string> _repositoriesNames = new List<string>();
        private readonly string _defaultRepositoryName;

        public PassportsRepositoryFactory(IEnumerable<IPassportsRepository> passprortRepositories, IConfiguration configuration)
        {
            _defaultRepositoryName = configuration["Database"];
            foreach (IPassportsRepository repository in passprortRepositories)
            {
                _repositoriesNames.Add(repository.Name);
                _passprortRepositories.Add(repository.Name, repository);
            }
        }

        /// <summary>
        /// Возвращает список имен репозитори
        /// </summary>
        /// <returns></returns>
        public List<string> GetRepositoriesNames()
        {
            return _repositoriesNames;
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
                throw new Exception($"Not database selected. Use option \"Database\" in appsettings.json. variants {Variants()}");
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
                
                throw new Exception($"PassportsRepository \"{name}\" does not exist. variants {Variants()}");
            }
        }

        private string Variants()
        {
            return String.Join(',', _repositoriesNames);
        }
    }
}
