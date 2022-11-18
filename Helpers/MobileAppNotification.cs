using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace GuidanceConsultancy.Helpers
{
    public class MobileAppNotification
    {

        private string serverKey = "";
        //private string serverKey = "AIzaSyD_j3LKWc3Dd4crux7bRo4vHHOh-zerDMc";
        private string senderId = "";
        private string webAddr = "";



        public string SendNotification(string DeviceToken, string title, string msg)
        {
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = DeviceToken,
                priority = "high",
                content_available = true,

                notification = new
                {
                    body = msg,
                    title = title
                    //sound = "/Assets/Website/ringtone/message-alert.mp3"
                },
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
