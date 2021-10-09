using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using Passports.Models;
using System.ComponentModel;

namespace Passports
{
    /// <summary>
    /// Класс для скачивания файла с данными
    /// </summary>
    internal class Download
    {
        private const string NameZipFile = "passport.zip";

        private IPassportsRepository _passportsRepository;

        public Download(IPassportsRepository passportsRepository)
        {
            _passportsRepository = passportsRepository;
        }

        /// <summary>
        /// Получение файла с данными
        /// </summary>
        /// <param name="Url"></param>
        public void GetFile(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                client.DownloadFileAsync(new Uri(url), NameZipFile);
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
