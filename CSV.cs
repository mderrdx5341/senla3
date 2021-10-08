using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Passports.Models;

namespace Passports
{
    public class CSV
    {
        static public void Read(Stream fileStream)
        {
            try
            {                
                StreamReader sr = new StreamReader(fileStream);
                var line = sr.ReadLine();
                while (line != null)
                {
                    line = sr.ReadLine();
                    var data = line.Split(";");
                    foreach(String s in data)
                    {
                        Passport passport = new Passport() { 
                            Series = Convert.ToInt32(data[0]), 
                            Number = Convert.ToInt32(data[1])
                        };
                        PassportRepository.Add(passport);
                    }
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
