using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Helpers
{
    /// <summary>
    /// Contains utility methods and extention methods.

    /// </summary>
    /// 

    public static class Utilities
    {
        /// <summary>
        /// Returns a new DateTime object which is the result 
        /// of adding the hours onto the original UTC time.
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(5.5);
        }

        public static DateTime IndianStandard(DateTime currentDate)
        {
            TimeZoneInfo abc = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime utc = currentDate;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, abc);
        }

        /// <summary>
        /// Used to create MD5 hash
        /// </summary>
        /// <param name="PlainString"></param>
        /// <returns>string</returns>
        public static string CreateMD5HashFromPlainString(string PlainString)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(PlainString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }



        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.AddHours(5.5).Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }



        public static string ToStringShort(this int num)
        {
            if (num < 1000)
            {
                return num.ToString("#0", CultureInfo.CurrentCulture);
            }

            if (num < 10000)
            {
                num /= 10;
                return (num / 100f).ToString("#.00'K'", CultureInfo.CurrentCulture);
            }

            if (num < 1000000)
            {
                num /= 100;
                return (num / 10f).ToString("#.0'K'", CultureInfo.CurrentCulture);
            }

            if (num < 10000000)
            {
                num /= 10000;
                return (num / 100f).ToString("#.00'M'", CultureInfo.CurrentCulture);
            }

            num /= 100000;
            return (num / 10f).ToString("#,0.0'M'", CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Generic method for converting enum to mvc select list
        /// Useful when you want to bind a Enum to MVC SelectList
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>SelectList</returns>
        public static SelectList EnumToSelectList<TEnum>()
         where TEnum : struct, IComparable, IFormattable, IConvertible
        {

            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(TEnum), x),
                        Value = Enum.GetName(typeof(TEnum), x)
                        //Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
        }

        public static void SendEmail(string toEmailAddress, string emailSubject, string emailMessage, string cc = null, string bcc = null)
        {
            using (MailMessage message = new MailMessage(AppConstants.Smtp.FromEmail, toEmailAddress))
            {
                message.Subject = emailSubject;
                message.Body = emailMessage;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = AppConstants.Smtp.Host;
                    smtpClient.EnableSsl = AppConstants.Smtp.EnableSsl;
                    NetworkCredential NetworkCred = new NetworkCredential(AppConstants.Smtp.Email, AppConstants.Smtp.Password);
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = NetworkCred;
                    smtpClient.Port = AppConstants.Smtp.Port;

                    smtpClient.Send(message);
                }
            }
        }

        public static void SendEmailAsync(string toEmailAddress, string emailSubject, string emailMessage, string cc = null, string bcc = null)
        {
            Thread emailThread = new Thread(delegate ()
            {
                SendEmail(toEmailAddress, emailSubject, emailMessage, cc, bcc);
            });

            emailThread.IsBackground = true;
            emailThread.Start();
        }

        public static string MaskString(string stringToMask)
        {
            int len = stringToMask.Length;
            int firstPartCount = len / 5;
            int lastPartCount = len / 3;

            //take first  characters
            string firstPart = stringToMask.Substring(0, firstPartCount);

            //take last characters
            string lastPart = stringToMask.Substring(len - lastPartCount, lastPartCount);

            //take the middle part (XXXXXXXXX)
            int maskLenght = len - (firstPartCount + lastPartCount);
            string maskedPart = new String('X', maskLenght);

            return firstPart + maskedPart + lastPart;
        }
    }

}