using Doujin_Interface.Database;
using Doujin_Interface.Notifications;
using Doujin_Interface.UiElements.UpdateFeed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Doujin_Interface.UpdateFeed
{
    public class UpdateChecker
    {
        public Dictionary<int,string> newReleases = new Dictionary<int, string>();
        private static Database.DoujinSet.KeepUpdatedDataTable db = DatabaseControler.updates;
        private NotifyerElement notifyerElement;
        public UpdateChecker(NotifyerElement notifyerElement)
        {
            if (File.Exists("ReleaseNotifyer.xml"))
            {
                
                this.notifyerElement = notifyerElement;
                CheckForUpdates();
            }
            
        }
        
        public void GetLastId(Database.DoujinSet.KeepUpdatedRow row)
        {
            string apiUrl = row.ApiUrl;
            var request = WebRequest.Create(apiUrl);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            Dictionary<int, string> releases = new Dictionary<int, string>();
            XNode node = JsonConvert.DeserializeXNode(text, "Root");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.ToString());

            XElement table = XElement.Parse(node.ToString());
            
            var v1 = table.Elements().ElementAt(0);


            if (v1.Name == "result")
            {
                foreach (var title in v1.Elements())
                {
                    Console.WriteLine($"Release Notifyer: {title.Value}");
                    if (title.Name == "id")
                    {
                        row.LastID = int.Parse(title.Value);
                        break;
                    }

                }
            }
            
        }
        public void CheckForUpdates()
        {
            foreach (var row in DatabaseControler.updates)
            {
                Dictionary<int, string> releases = new Dictionary<int, string>();
                var page = 1;
                var c = 1;
            Redo:
                var request = WebRequest.Create($"{row.ApiUrl}&page={page}");
                string text;
                HttpWebResponse response = null;
                try {response = (HttpWebResponse)request.GetResponse(); }
                catch { Thread.Sleep(1000); goto Redo; };
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }
                XNode node = JsonConvert.DeserializeXNode(text, "Root");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(node.ToString());
                XElement table = XElement.Parse(node.ToString());
                foreach (XElement element in table.Elements())
                {   
                    if (element.Name == "result")
                    {
                        foreach (var title in element.Elements())
                        {
                            if (title.Name == "id")
                            {
                                var compare = int.Parse(title.Value);
                                if (compare == row.LastID && releases.Count > 0)
                                {
                                    foreach (var item in releases)
                                    {

                                        if (!newReleases.ContainsKey(item.Key))
                                        {
                                            newReleases.Add(item.Key, item.Value);
                                        }
                                    }
                                    row.LastID = releases.First().Key;
                                    releases.Clear();
                                    if (row.ID == db.Count-1)
                                    {
                                        Console.WriteLine($"Release Notifyer: called end");
                                        goto End;
                                    }
                                    else
                                    {
                                        c = 1;
                                        Console.WriteLine($"Release Notifyer: called next");
                                        goto Next;
                                    }
                                }
                                else if (compare == row.LastID && releases.Count == 0)
                                {
                                    Console.WriteLine($"Release Notifyer: nothing to report");
                                    if (row.ID == db.Count - 1)
                                    {
                                        Console.WriteLine($"Release Notifyer: called end");
                                        goto End;
                                    }
                                    else
                                    {
                                        c = 1;
                                        Console.WriteLine($"Release Notifyer: called next");
                                        goto Next;
                                    }

                                }
                                else if (compare != row.LastID)
                                {
                                    releases.Add(compare, row.Name);
                                    Console.WriteLine($"Release Notifyer: {compare} with tag {row.Name} has been added");
                                    c++;
                                    if (c == 26)
                                    {
                                        c = 1;
                                        page++;
                                        Console.WriteLine($"Release Notifyer: called redo");
                                        goto Redo;
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }
                Next:
                continue;
            }
        End:
            Console.WriteLine($"Release Notifyer: end");
            if (newReleases.Count > 0)
            {
                foreach (var item in newReleases.Values.Distinct())
                {
                    //Notifications.Notifications.NotificationNoImg(DoujinUtility.MainWindow, $"The tag {item} has new Doujin on nHentai!","","");
                    Console.WriteLine($"Release Notifyer: The tag {item} has new Doujin on nHentai!");
                }

            }
            Thread.Sleep(1000);
            notifyerElement.DisplayNewUploads(this);
            

        }
        public void AddObject(string name, Types type)
        {
            string apiUrl = "https://nhentai.net/api/galleries/search?query=";
            
            switch (type)
            {
                case Types.Arist:
                    apiUrl += $"artist%3A\"{name}\"";
                    break;
                case Types.Character:
                    apiUrl += $"character%3A\"{name}\"";
                    break;
                case Types.Group:
                    apiUrl += $"group%3A\"{name}\"";
                    break;
                case Types.Parody:
                    apiUrl += $"parody%3A\"{name}\"";
                    break;
                case Types.Tag:
                    apiUrl += $"tag%3A\"{name}\"";
                    break;


            }
            var row = db.AddKeepUpdatedRow(db.Count, name, apiUrl, 0);
            GetLastId(row);
            db.WriteXml("ReleaseNotifyer.xml");
        }
    }
}

