using Doujin_Interface.Connection.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doujin_Interface.Connection
{
    public class Connection
    {
        private const string IP = "http://localhost:44323";
        public static HttpStatusCode LoginToServer(string username, string password)
        {
            var form = new LoginForm(username, password);
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
            Console.WriteLine($"Connection : {response}");
            return httpResponse.StatusCode;
        }
        public static HttpStatusCode RegisterToServer(string email, string password, string repeatPassword)
        {
            if (password == repeatPassword)
            {
                var form = new RegisterForm(email, password, repeatPassword);
                string jsonstring = JsonConvert.SerializeObject(form);
                Console.WriteLine(jsonstring);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{IP}/account/register");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonstring);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
                Console.WriteLine($"Connection : {response}");
                return httpResponse.StatusCode;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
            
            
        }

        
    }
}
