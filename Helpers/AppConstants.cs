using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Helpers
{
    public static class AppConstants
    {
        //public static string BaseURL { get; } = "https://kotabaghlive.com/";
        public static string BaseURL { get; } = "https://localhost:44341/";
        public static string CompanyName { get; } = "Disha Guidance Consultancy";
        public static string CompanyWebsite { get; } = "disha.com";
        public static string ApplicationDataFolder { get; } = "ApplicationData";
        public static string ExceptionLogsFolderName { get; } = "ExceptionLogs";
        public static string EventLogsFolderName { get; } = "EventLogs";
        public static string DateTimeFormatForViews { get; } = "{0:dd-MMM-yyyy}";

        public static DateTime TruncateDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        public static class UserRoles
        { 
            public static class SuperAdministrator
            {
                public static string RoleName { get; } = "SuperAdministrator";
                public static int RoleId
                {
                    get
                    {
                        return Convert.ToInt32(ConfigurationManager.AppSettings["RoleId_SuperAdministrator"]);
                    }
                }
            }

            public static class Administrator
            {

                public static string RoleName { get; } = "Administrator";
                public static int RoleId
                {
                    get
                    {
                        return Convert.ToInt32(ConfigurationManager.AppSettings["RoleId_Administrator"]);
                    }
                }
            }

            public static class User
            {
                public static string RoleName { get; } = "User";
                public static int RoleId
                {
                    get
                    {
                        return Convert.ToInt32(ConfigurationManager.AppSettings["RoleId_Editor"]);
                    }
                }
            }

 

        }


        


        #region Properties from WebConfig

        public static int PageSize
        {
            get
            {
                try
                {
                    var pageSizeFromConfig = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

                    if (pageSizeFromConfig > 0)
                    {
                        return pageSizeFromConfig;
                    }
                    else
                    {
                        return 10;
                    }
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static int PasswordLinkExpirationTimeInMinutes
        {
            get
            {
                try
                {
                    var timeInMinutes = Convert.ToString(ConfigurationManager.AppSettings["PasswordLinkExpirationTimeInMinutes"]);

                    if (!string.IsNullOrEmpty(timeInMinutes))
                    {
                        return Convert.ToInt32(timeInMinutes);
                    }
                    else
                    {
                        //default is 30
                        return 30;
                    }
                }
                catch
                {
                    //default is 30
                    return 30;
                }
            }
        }

        public static string StringEncryptionKey
        {
            get
            {
                try
                {
                    var key = Convert.ToString(ConfigurationManager.AppSettings["StringEncryptionKey"]);

                    if (!string.IsNullOrEmpty(key))
                    {
                        return key;
                    }
                    else
                    {
                        return "testing@gmail.com";
                    }
                }
                catch
                {
                    return "testing@gmail.com";
                }
            }
        }

        public static class Smtp
        {
            public static string Host
            {
                get
                {
                    return Convert.ToString(ConfigurationManager.AppSettings["SmtpHost"]);
                }
            }

            public static bool EnableSsl
            {
                get
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                }
            }

            public static int Port
            {
                get
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                }
            }

            public static string Email
            {
                get
                {
                    return Convert.ToString(ConfigurationManager.AppSettings["SmtpEmail"]);
                }
            }

            public static string Password
            {
                get
                {
                    return Convert.ToString(ConfigurationManager.AppSettings["SmtpPassword"]);
                }
            }

            public static string FromEmail
            {
                get
                {
                    return Convert.ToString(ConfigurationManager.AppSettings["SmtpFromEmail"]);
                }
            }
        }

        public static class CategoryTerms
        {

        }

        public static bool DoFileWriteTestOnStart
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["DoFileWriteTestOnStart"]);
                }
                catch
                {

                    return false;
                }
            }
        }


        #endregion
    }
}