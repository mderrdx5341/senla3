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
    /// Класс для скачивания файла с данными
    /// </summary>
    internal class DataUpdater: IDataUpdater
    {
        private const string NameZipFile = "passport.zip";
        private readonly string _url;

        private IPassportsRepository _passportsRepository;

        public DataUpdater(IPassportsRepository passportsRepository, IConfiguration configuration)
        {
            _url = configuration["FileUrl"];
            _passportsRepository = passportsRepository;
        }

        /// <summary>
        /// Получение файла с данными
        /// </summary>
        /// <param name="Url"></param>
        public void Run()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                client.DownloadFileAsync(new Uri(_url), NameZipFile);
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            using (ZipArchive zip = ZipFile.OpenRead(NameZipFile))
            {

                foreach (var zipEntry in zip.Entries)
                {
                    using(CSVStreamReader csv = new CSVStreamReader(zipEntry.Open()))
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
    }
}
