using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.Net;
//
using System.IO;
using System.IO.Compression;
//
using Passports.Models;
using System.ComponentModel;

namespace Passports
{
    /// <summary>
    /// Класс для скачивания файла с данными
    /// </summary>
    public class Download
    {
        const string NameZipFile = "passport.zip";

        /// <summary>
        /// Получение файла с данными
        /// </summary>
        /// <param name="Url"></param>
        static public void GetFile(String Url)
        {
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            client.DownloadFileAsync(new Uri(Url), NameZipFile);
        }

        static private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            ZipArchive zip = ZipFile.OpenRead(NameZipFile);

            foreach (var zipEntry in zip.Entries)
            {
                CSV.Read(zipEntry.Open());
            }
        }
    }
}
