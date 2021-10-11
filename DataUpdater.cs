using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using Passports.Models;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace Passports
{
    /// <summary>
    /// Класс для обнавления данных
    /// </summary>
    internal class DataUpdater: IDataUpdater
    {
        private const string NameZipFile = "passport.zip";
        private readonly string _url;

        private readonly IPassportsRepository _passportsRepository;

        public DataUpdater(IPassportsRepository passportsRepository, IConfiguration configuration)
        {
            _url = configuration["FileUrl"];
            _passportsRepository = passportsRepository;
        }

        /// <summary>
        /// Запуск обновления
        /// </summary>
        /// <param name="Url"></param>
        public void Run()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(_url), NameZipFile);
                ReadZipFile();
            }
        }

        private void ReadZipFile()
        {
            using (ZipArchive zip = ZipFile.OpenRead(NameZipFile))
            {
                foreach (var zipEntry in zip.Entries)
                {
                    UpdateDataFromZip(zipEntry.Open());
                }
            }
        }

        private void UpdateDataFromZip(Stream stream)
        {
            using (CSVStreamReader csv = new CSVStreamReader(stream))
            {
                try
                {
                    foreach (string[] record in csv)
                    {
                        _passportsRepository.Add(
                            new Passport() { Series = Convert.ToInt32(record[0]), Number = Convert.ToInt32(record[1]) }
                        );
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
