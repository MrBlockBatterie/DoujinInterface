using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Doujin_Interface
{
    class HIDoujin : DoujinTemplate
    {


        public HIDoujin(string name, string language, int hitomiId)
        {
            this.hentaiId = hitomiId;
            this.name = name;
            this.language = language;
            using (var client = new WebClient())
            {

                client.Headers.Add("user-agent", "Only a test!");
                Console.WriteLine($"https://hitomi.la/doujinshi/{name.Replace(" ", "-").ToLower()}-{language}-{hitomiId}.html");
                string html = client.DownloadString($"https://hitomi.la/doujinshi/{name.Replace(" ", "-").ToLower()}-{language}-{hitomiId}.html");


                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute href = link.Attributes["href"];   
                    if (href != null)
                    {
                        if (href.Value.Contains("/reader/"))
                        {
                            Console.WriteLine(href.Value);
                            GetPages(href.Value);
                        }
                    }
                }

            }

        }
        private int GetPages(string htmlIn)
        {
            using (var client = new WebClient())
            {

                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " + "Windows NT 5.2; .NET CLR 1.0.3705;)");
                Console.WriteLine($"https://hitomi.la{htmlIn}");
                string html = client.DownloadString($"https://ltn.hitomi.la/galleries/1620312.js");

                html = html.Remove(0, 18);
                Console.WriteLine("\"root\":" +  html);

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + html + "}");
                doc.Save("deserialize.xml");


                return 0; //Int32.Parse(href.Value);
            }
        }
    }
}
