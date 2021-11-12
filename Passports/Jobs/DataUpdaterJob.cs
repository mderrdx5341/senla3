using Microsoft.Extensions.Configuration;
using Passports.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Net.Http;
using System.Threading;

namespace Passports.Jobs
{
    /// <summary>
    /// Класс обновляет данные паспортов
    /// </summary>
    internal class DataUpdaterJob : IJob
    {
        private const string NameZipFile = "passport.zip";
        private readonly string _url;
        private readonly IPassportsRepository _passportsRepository;

        public DataUpdaterJob(IPassportsRepository passportsRepository, IConfiguration configuration)
        {
            _passportsRepository = passportsRepository;
            _url = configuration["FileUrl"];
        }

        /// <summary>
        /// Запуск обновления
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(_url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    using (Stream streamToWriteTo = File.Open(NameZipFile, FileMode.Create))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo).ConfigureAwait(false);                       
                    }                   
                }
            }
            UpdateDataFromZip();
        }

        private void UpdateDataFromZip()
        {
            using (ZipArchive zip = ZipFile.OpenRead(NameZipFile))
            {
                foreach (var zipEntry in zip.Entries)
                {
                    List<Passport> passports = ReadDataFromCSV(zipEntry.Open());
                    _passportsRepository.SaveRangeAsync(
                        passports
                    );
                }
            }
            //File.Delete(NameZipFile);
        }

        public List<Passport> ReadDataFromCSV(Stream stream)
        {
            List<Passport> passports = new List<Passport>();
            using (StreamReader csv = new StreamReader(stream))
            {
                
                string line;
                while ((line = csv.ReadLine()) != null)
                {
                    string[] record = line.Split(";");
                    passports.Add(
                        new Passport() { Series = Convert.ToInt32(record[0]), Number = Convert.ToInt32(record[1]) }
                    );
                }

            }

            return passports;
        }
    }
}
