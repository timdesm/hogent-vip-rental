using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InterfaceAppPresentationLayer.Classes
{
    public class LogService
    {
        public static void WriteLog(List<String> lines)
        {
            string sDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RudysRentalManager/logs");

            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }

            using (FileStream stream = File.Create(Path.Combine(sDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".txt")))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(DateTime.Now.ToString()  + " -----------------------------------------");
                    foreach(string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.WriteLine(" ");
                }
            }
        }
    }
}
