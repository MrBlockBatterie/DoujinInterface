using Doujin_Interface.Connection.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.Connection
{
    public class Client
    {
        private const string IP = "http://localhost:44323/api";
        private HttpClient client;
        public Client()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(IP);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.client = client;
            
        }
        public async Task RegisterToServer(string email, string password, string repeatPassword)
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
                    
                    using (var response = await client.PostAsJsonAsync("/Account/Register",data))
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
        
        }
    }

