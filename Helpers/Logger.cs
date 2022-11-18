using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Helpers
{
    public static class Logger
    {
        public static void LogException(Exception ex)
        {
            try
            {
                var path = "~/" + AppConstants.ApplicationDataFolder + "/" + AppConstants.ExceptionLogsFolderName;

                var serverPath = HttpContext.Current.Server.MapPath(path);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                var fileServerPath = serverPath + "/" + DateTime.UtcNow.AddHours(5.5).ToString("dd-MMM-yyyy") + ".txt";

                using (StreamWriter writer = new StreamWriter(fileServerPath, true))
                {
                    writer.WriteLine();

                    writer.WriteLine("********************************************   " + DateTime.UtcNow.AddHours(5.5).ToString("dd-MMM-yyyy hh:mm tt") + "   *****************************************************");

                    writer.WriteLine();

                    writer.WriteLine("Exception Message : " + ex.Message);
                    writer.WriteLine("Exception Source  : " + ex.Source);
                    writer.WriteLine("Stacktrace        : " + ex.StackTrace);

                    writer.WriteLine();

                    writer.WriteLine("******************************************************************************************************************************");
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex1)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
        }

        public static void LogEvent(string Source, string Message)
        {
            try
            {
                var path = "~/" + AppConstants.ApplicationDataFolder + "/" + AppConstants.EventLogsFolderName;

                var serverPath = HttpContext.Current.Server.MapPath(path);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                var fileServerPath = serverPath + "/" + DateTime.UtcNow.AddHours(5.5).ToString("dd-MMM-yyyy") + ".txt";

                using (StreamWriter writer = new StreamWriter(fileServerPath, true))
                {
                    writer.WriteLine();

                    writer.WriteLine("********************************************   " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + "   *****************************************************");

                    writer.WriteLine();

                    writer.WriteLine("Source : " + Source);
                    writer.WriteLine("Message  : " + Message);

                    writer.WriteLine();

                    writer.WriteLine("******************************************************************************************************************************");
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex1)
#pragma warning restore CS0168 // Variable is declared but never used
            {
            }
        }

        public static bool DoFileWriteTest()
        {
            try
            {
                var path = "~/" + AppConstants.ApplicationDataFolder;

                var serverPath = HttpContext.Current.Server.MapPath(path);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                var fileServerPath = serverPath + "/" + "writetest.txt";

                using (StreamWriter writer = new StreamWriter(fileServerPath))
                {
                    writer.WriteLine("File write test");
                }

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}