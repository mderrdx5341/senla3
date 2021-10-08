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
    public class Download
    {
        const string NameZipFile = "passport.zip";
        static public void File(String Url)
        {
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            client.DownloadFileAsync(new Uri(Url), NameZipFile);
        }

        static private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            List<Passport> passports = new List<Passport>();
            ZipArchive zip = ZipFile.OpenRead(NameZipFile);

            foreach (var zipEntry in zip.Entries)
            {
                CSV.Read(zipEntry.Open());
            }
        }
    }
}
