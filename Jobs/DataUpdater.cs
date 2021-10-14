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

namespace Passports.Jobs
{
    internal class DataUpdater : IJob
    {
        private const string NameZipFile = "passport.zip";
        private readonly string _url;
        private readonly IPassportsRepository _passportsRepository;

        public DataUpdater(IPassportsRepository passportsRepository , IConfiguration configuration)
        {
            _passportsRepository = passportsRepository;
            _url = configuration["FileUrl"];
        }

        /// <summary>
        /// Запуск обновления
        /// </summary>
        /// <param name="Url"></param>
        public Task Execute(IJobExecutionContext context)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(_url), NameZipFile);
                UpdateDataFromZip();
            }
            return Task.CompletedTask;
        }

        private void UpdateDataFromZip()
        {
            using (ZipArchive zip = ZipFile.OpenRead(NameZipFile))
            {
                foreach (var zipEntry in zip.Entries)
                {
                    UpdateData(zipEntry.Open());
                }
            }
        }

        private void UpdateData(Stream stream)
        {
            using (StreamReader csv = new StreamReader(stream))
            {
                string line = csv.ReadLine();
                string[] record = line.Split(";");
                _passportsRepository.Add(
                    new Passport() { Series = Convert.ToInt32(record[0]), Number = Convert.ToInt32(record[1]) }
                );
            }
        }
    }
}
