using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ParkingManagement.Logger
{
    public class LoggerClass
    {
        public static void AddLog(Exception inputData)
        {
            String fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            try
            {
                string folderpath = ConfigurationManager.AppSettings["LogFileFolderPath"];

                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                folderpath = folderpath + "\\" + fileName;
                using (StreamWriter writer = new StreamWriter(folderpath, true))
                {
                    writer.WriteLine(inputData.ToString());
                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}
