using Doujin_Interface.Connection.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doujin_Interface.Connection
{
    public class Connection
    {
        private const string IP = "http://localhost:9696";
        public static string LoginToServer(string username, string password)
        {
            var form = new LoginForm(username, HashPassword(password));
            string jsonstring = JsonConvert.SerializeObject(form);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{IP}/api/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonstring);
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            return response;
        }

        public static string HashPassword(string password)
        {
            return password;
        }
        private async Task<string> GetResponse(HttpWebRequest httpWebRequest)
        {
            HttpWebResponse httpResponse;
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            return response;
        }
    }
}
