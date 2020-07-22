using Doujin_Interface.Connection.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Doujin_Interface.Connection
{
    public class ApiHelper
    {
        private const string IP = "http://127.0.0.1:44323/api";
        private HttpClient client;
        public ApiHelper()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(IP);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.client = client;
            var response = RegisterToServer("mail@email.de", "Pass1234.", "Pass1234.").Result;
            Console.WriteLine(response);

        }

        
        public async Task<string> RegisterToServer(string email, string password, string repeatPassword)
        {
            if (password == repeatPassword)
            {
                
                //client.BaseAddress = new Uri(IP);
                var form = new RegisterForm(email, password, repeatPassword);
                var data = JsonConvert.SerializeObject(form);
                var data2 = new FormUrlEncodedContent( new[]
                {
                    new KeyValuePair<string,string>("Email","test@email.com"),
                    new KeyValuePair<string,string>("Password","TesT123."),
                    new KeyValuePair<string,string>("ConfirmPassword","TesT123.")
                });
                try
                {
                 
                    
                    using (var response = await client.PutAsJsonAsync("Account/Register",data))
                    {
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsAsync<string>();
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine(response.ReasonPhrase);
                        }
                        return "";
                    }
                    
                }
                catch (Exception e)
                {

                    throw e;
                }
                

            }
            else
            {
                throw new Exception();
            }


        }

        
        public async Task<string> Test(string email, string password, string repeatPassword)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var form = new RegisterForm(email, password, repeatPassword);
                var data = JsonConvert.SerializeObject(form);
                HttpResponseMessage responseMessage = await httpClient.PostAsync("http://localhost:44323/api/Account/Register", new StringContent(data));
                    

                return await responseMessage.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PostAsync(string email, string password, string repeatPassword)
        {
            HttpWebRequest webRequest = HttpWebRequest.CreateHttp("http://localhost:44323/api/Account/Register");
            {
                var form = new RegisterForm(email, password, repeatPassword);
                var formData = JsonConvert.SerializeObject(form);
                byte[] data = Encoding.UTF8.GetBytes(formData);

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string response = await streamReader.ReadToEndAsync();
                    return response;
                }
                
            }
            
        }



    }
    public class Client
    {
        
    }
}

