using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Passports.Models;

namespace Passports
{
    /// <summary>
    /// Класс для парсинга CSV
    /// </summary>
    public class CSV
    {
        /// <summary>
        /// Парсит строки CSV и добавляет данные в репозиторий
        /// </summary>
        /// <param name="fileStream"></param>
        static public void Read(Stream fileStream)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(";");
                        Passport passport = new Passport()
                        {
                            Series = Convert.ToInt32(data[0]),
                            Number = Convert.ToInt32(data[1])
                        };
                        PassportRepository.Add(passport);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
