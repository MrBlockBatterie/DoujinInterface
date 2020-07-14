using Newtonsoft.Json;
using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static Doujin_Interface.Database.DoujinSet;

namespace Doujin_Interface
{
    public class Doujin : IDisposable
    {
        public int id;
        public int nhentaiId;       //implemented
        public int mediaId;         //implemented
        public int pageCount;       //implemented
        public string name;         //implemented
        public string fullName;     //implemented
        public string coverExt;     //implemented
        public string thumbnailExt;      //implemented
        public string language;     //implemented
        public Boolean translated = false;                  //implemented
        public bool favorised = false;
        public string coverUrl;                             //implemented
        public List<string> pageExt = new List<string>();
        public List<string> group = new List<string>();     //implemented
        public List<string> character = new List<string>(); //implemented
        public List<string> parodys = new List<string>();   //implemented
        public List<string> artists = new List<string>();   //implemented
        public List<string> tags = new List<string>();      //implemented

        public Doujin()
        {
            //GetFavorised();
        }

        

        public Doujin(int id, int nhId, int mediaId, int pageCount, string name, string fullname, string coverExt, string thumbnailExt, string language, bool faved, string coverUrl)
        {
            this.id = id;
            this.nhentaiId = nhId;
            this.mediaId = mediaId;
            this.pageCount = pageCount;
            this.name = name;
            this.fullName = fullname;
            this.coverExt = coverExt;
            this.thumbnailExt = thumbnailExt;
            this.language = language;
            this.favorised = faved;
            this.coverUrl = coverUrl;
                
            
        }
        public Doujin(int nhIDin)
        {
            Retry:
            //this.id = Database.DatabaseControler.mainDataTable.FindBynHentaiID(nhIDin).ID;
            var request = WebRequest.Create("https://nhentai.net/api/gallery/" + nhIDin);
            Console.WriteLine($"https://nhentai.net/api/gallery/{nhIDin}");
            string text;
            HttpWebResponse response = null;
            try {response = (HttpWebResponse)request.GetResponse(); }
            catch 
            {
                
                    System.Threading.Thread.Sleep(100);
                    goto Retry;
                
            };
            
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            XNode node = JsonConvert.DeserializeXNode(text, "Root");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.ToString());
            //doc.Save(artistIn + ".xml");
            int id = 0;

            XElement table = XElement.Parse(node.ToString());
            foreach (XElement title in table.Elements())
            {

                
                    Doujin doujin = this;
                    string extension = "";
                if (title.Name == "id")
                {
                    id = int.Parse(title.Value);
                    this.nhentaiId = id;
                }
                if (title.Name == "media_id")
                {
                    doujin.mediaId = int.Parse(title.Value);
                    
                    doujin.coverUrl = "https://t.nhentai.net/galleries/" + title.Value + "/1t";




                }
                if (title.Name == "images")
                {
                    
                        foreach (var pages in title.Elements())
                        {
                            if (pages.Name == "pages")
                            {
                                foreach (var t in pages.Elements())
                                {
                                    if (t.Value == "j")
                                    {
                                        doujin.pageExt.Add(".jpg");
                                    }
                                    else if (t.Value == "p")
                                    {
                                        doujin.pageExt.Add(".png");
                                    }
                                }
                            }
                        }
                    

                    foreach (var pages in title.Elements())
                    {
                        if (pages.Name == "thumbnail")
                        {
                            foreach (var t in pages.Elements())
                            {
                                if (t.Value == "j")
                                {
                                    Console.WriteLine(".jpg");
                                    extension = ".jpg";
                                    doujin.coverExt = ".jpg";
                                    doujin.thumbnailExt = ".jpg";
                                }
                                else if (t.Value == "p")
                                {
                                    Console.WriteLine(".png");
                                    extension = ".png";
                                    doujin.coverExt = ".png";
                                    doujin.thumbnailExt = ".png";

                                }

                            }
                            doujin.coverUrl = "https://t.nhentai.net/galleries/" + doujin.mediaId + "/1t" + extension;

                        }
                    }
                }
                if (title.Name == "tags")
                {

                    foreach (var tag in title.Elements())
                    {
                        if (tag.Name == "url")
                        {

                            string[] split = tag.Value.Split('/');

                            if (split[1] == "tag")
                            {
                                doujin.tags.Add(split[2]);
                            }
                            else if (split[1] == "group")
                            {
                                doujin.group.Add(split[2]);
                            }
                            else if (split[1] == "artist")
                            {
                                doujin.artists.Add(split[2]);
                            }
                            else if (split[1] == "character")
                            {
                                doujin.character.Add(split[2]);
                            }
                            else if (split[1] == "language" && split[2] == "translated")
                            {
                                doujin.translated = true;
                            }
                            else if (split[1] == "language")
                            {
                                doujin.language = split[2];
                            }
                            else if (split[1] == "parody")
                            {
                                doujin.parodys.Add(split[2]);
                            }
                        }

                    }
                }


                if (title.Name == "title")
                {
                    foreach (var pretty in title.Elements())
                    {
                        if (pretty.Name == "pretty")
                        {
                            Console.WriteLine(pretty.Value);
                            doujin.name = pretty.Value;
                        }
                        if (pretty.Name == "english")
                        {
                            doujin.fullName = pretty.Value;
                        }
                    }
                }
                if (title.Name == "num_pages")
                {
                    doujin.pageCount = int.Parse(title.Value);
                }
                else
                {
                    continue;
                }
                
            }
        }
        private void GetFavorised()
        {
            DoujinDataRow favRow = MainWindow.favs.FindBynHentaiID(this.nhentaiId);
            foreach (DoujinDataRow mainRow in MainWindow.dt)
            {
                if (mainRow.nHentaiID == favRow.nHentaiID)
                {
                    int id = mainRow.ID;
                    MainWindow.dt.RemoveDoujinDataRow(mainRow);
                    MainWindow.dt.AddDoujinDataRow(id, favRow.nHentaiID, favRow.mediaID, favRow.name, favRow.fullName, favRow.artist, favRow.character, favRow.parody, favRow.group, favRow.tags, favRow.language, favRow.favorite, favRow.pages, favRow.coverUrl, favRow.extension);
                    
                }
            }
        }
        public string Artist()
        {
            if (artists.Count > 1)
            {
                return artists.First();
            }
            else
            {
                if (artists.Count != 0)
                {
                    return artists.First();
                }
                else
                {
                    return "none";
                }
            }    
        }
        public string Parody()
        {
            if (parodys.Count > 1)
            {
                return parodys.First();
            }
            else
            {
                if (parodys.Count != 0)
                {
                    return parodys.First();
                }
                else
                {
                    return "none";
                }
            }
        }
        public string Character()
        {
            if (character.Count > 1)
            {
                return character.First();
            }
            else
            {
                if(character.Count != 0)
                {
                    return character.First();
                }
                else
                {
                    return "none";
                }
                
            }
        }
        public string Group()
        {
            if (group.Count > 1)
            {
                return group.First();
            }
            else
            {
                if (group.Count != 0)
                {
                    return group.First();
                }
                else
                {
                    return "none";
                }
            }
          
        }
        public string ArtistsConcat()
        {
            return string.Join(",", artists);
        }
        public string GroupsConcat()
        {
            return string.Join(",", group);
        }
        public string CharactersConcat()
        {
            return string.Join(",", character);
        }
        public string ParodysConcat()
        {
            return string.Join(",", parodys);
        }
        public string TagsConcat()
        {
            return string.Join(",", tags);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine($"Doujin {name} is getting Disposed");
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.


                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        //~Doujin()
        //{
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        //}

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}