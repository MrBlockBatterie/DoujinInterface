using Doujin_Interface.Connection.JSON;
using Doujin_Interface.Connection.Models;
using Newtonsoft.Json;
using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Doujin_Interface.Connection
{
    public class ApiHelper
    {
        private const string IP = "http://localhost:44323";
        private HttpClient client;
        public AuthenticatedUser user;
        
        public ApiHelper()
        {
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = new WebProxy("https://localhost:44323/", true),
                UseProxy = true
            };

            HttpClient client = new HttpClient(/*handler: httpClientHandler*/);
            client.BaseAddress = new Uri("http://localhost:44323/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            this.client = client;

            
            //var response = RegisterToServer("davis@email.de", "Pass1234.", "Pass1234.").Result;
            //_user = GetToken("davis@email.de", "Pass1234.").Result;
            //Console.WriteLine(_user.Access_Token);


        }


        public async Task<string> OldPost(string email, string password, string repeatPassword)
        {
            if (password == repeatPassword)
            {

                //client.BaseAddress = new Uri(IP);
                var form = new RegisterForm(email, password, repeatPassword, "");
                var data = JsonConvert.SerializeObject(form);
                var data2 = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("Email","test@email.com"),
                    new KeyValuePair<string,string>("Password","TesT123."),
                    new KeyValuePair<string,string>("ConfirmPassword","TesT123.")
                });
                try
                {


                    using (var response = await client.PutAsJsonAsync("Account/Register", data))
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
                var form = new RegisterForm(email, password, repeatPassword, "");
                var data = JsonConvert.SerializeObject(form);
                HttpResponseMessage responseMessage = await httpClient.PostAsync("http://localhost:44323/api/Account/Register", new StringContent(data));


                return await responseMessage.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> RegisterToServer(string email, string password, string repeatPassword, string username)
        {
            HttpWebRequest webRequest = HttpWebRequest.CreateHttp(IP + "/api/Account/Register");
            {
                var form = new RegisterForm(email, password, repeatPassword, username);
                var formData = JsonConvert.SerializeObject(form);
                byte[] data = Encoding.UTF8.GetBytes(formData);

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;
                webRequest.Proxy = new WebProxy("https://localhost:44323/", true);


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

        private async Task<AuthenticatedUser> GetToken(string name, string password)
        {

            HttpWebRequest webRequest = HttpWebRequest.CreateHttp(IP + "/token");
            {

                byte[] data = Encoding.UTF8.GetBytes($"grant_type=password&username={name}&password={password}");

                webRequest.Method = "POST";
                webRequest.ContentType = "raw/text";
                webRequest.ContentLength = data.Length;
                //webRequest.Proxy = new WebProxy("https://localhost:44323/", true);


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
                    var user = JsonConvert.DeserializeObject<AuthenticatedUser>(response);
                    Properties.Settings.Default.TokenExpiration = DateTime.Now.AddDays(14);
                    return user;
                }

            }
        }
        public async Task RecommendDoujin(string username, int doujinId)
        {
            var data = new RecommandationBindingModel
            {
                DoujinId = doujinId,
                FriendName = username
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Access_Token);
            using (var response = await client.PostAsJsonAsync("api/user/recommendations/add", data))
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

        public async Task<RecommendationViewModel> GetRecommendedDoujin()
        {
            if (user != null)
            {
                try
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Access_Token);
                    using (var response = await client.GetAsync("api/user/recommendations/get"))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<RecommendationViewModel>();
                            Console.WriteLine(result);
                            return result;
                        }
                        else
                        {
                            Console.WriteLine(response.ReasonPhrase);
                            return null;
                        }

                    }

                }
                catch (Exception e)
                {

                    throw e;
                }
                /*
                HttpWebRequest webRequest = HttpWebRequest.CreateHttp(IP + "/api/user/recommendations/get");
                {
                    
                    
                    byte[] data = Encoding.UTF8.GetBytes("");

                    webRequest.Method = "POST";
                    webRequest.Headers.Add("Authorization", "Bearer " + _user.Access_Token);
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = data.Length;
                    webRequest.Proxy = new WebProxy("https://localhost:44323/", true);


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
                        //return response;
                    }

                }
                */
            }
            else
            {
                //login handling
                return null;
            }
        }

        public async Task AddFriend(string username)
        {
            var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("FriendName",username)
                });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Access_Token);
            using (var response = await client.PutAsJsonAsync("api/user/friends/add", data))
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

        public async Task<HttpStatusCode> Login(string name, string password)
        {
            user = await GetToken(name, password);
            Properties.Settings.Default.User = JsonConvert.SerializeObject(user);
            return HttpStatusCode.OK;
        }

        public async Task<MutualFriends> GetFriends(string name = null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Access_Token);
            //MutualFriends result = new MutualFriends();

            //var response = await client.GetStringAsync("/api/user/friends/get");
            //Console.WriteLine(response);
            //using (var response = await client.GetAsync("/api/user/friends/get"))
            //{

            //    if (response.IsSuccessStatusCode)
            //    {
            //        result = await response.Content.ReadAsAsync<MutualFriends>();
            //        Console.WriteLine(result);
            //    }
            //    else
            //    {
            //        Console.WriteLine(response.ReasonPhrase);
            //    }

            //}
            HttpWebRequest webRequest = HttpWebRequest.CreateHttp(IP + "/api/user/friends/get");
            {


                byte[] data = Encoding.UTF8.GetBytes("");

                webRequest.Method = "GET";
                webRequest.Headers.Add("Authorization", "Bearer " + user.Access_Token);
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;
                webRequest.Proxy = new WebProxy("https://localhost:44323/", true);


                //using (Stream stream = webRequest.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}

            }

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string response = await streamReader.ReadToEndAsync();
                    var result = JsonConvert.DeserializeObject<MutualFriends>(response);
                    return result;
                }

            }

            
        }




    }

}

