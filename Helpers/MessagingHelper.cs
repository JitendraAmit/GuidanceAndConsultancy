using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace GuidanceConsultancy.Helpers
{
    public class MessagingHelper
    {
        private string UserName;
        private string ProviderName;
        private string Password;
        private string SenderId;



        public MessagingHelper()
        {
            //get here from config file

             ProviderName = ConfigurationManager.AppSettings["SMSApi_ProviderName"];
             UserName = ConfigurationManager.AppSettings["SMSApi_UserName"];
             Password = ConfigurationManager.AppSettings["SMSApi_Password"];
             SenderId = ConfigurationManager.AppSettings["SMSApi_SenderID"];
        }

        public string PushSms(string mobieNumber, string message)
        {
           
            var responseString = "";
            string msg = WebUtility.UrlEncode(message);

            var apiUrl = string.Format("http://testing.com/vendorsms/pushsms.aspx?user={0}&password={1}&msisdn={2}&sid={3}&msg={4}&fl=0&gwid=2", UserName, Password, mobieNumber, SenderId, msg);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            //request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseString = reader.ReadToEnd();
            }


            return responseString;
        }
    }
}
