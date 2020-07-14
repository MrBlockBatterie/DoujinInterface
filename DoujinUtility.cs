using Microsoft.Win32;
using Newtonsoft.Json;
using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using static Doujin_Interface.Database.DoujinSet;

namespace Doujin_Interface
{
    static public class DoujinUtility
    {
        public static MainWindow MainWindow;
        public static UpdateFeed.UpdateChecker updateChecker;
        public static string GetExtension(int doujinMediaID, Boolean cover, Doujin doujin)
        {
            string page = null;
            string[] fileTypes = {".jpg",".png",".gif"};
            HttpWebResponse response = null;
            switch (cover)
            {
                case false:
                    page = "5t";
                break;

                case true:
                    page = "cover";
                break;
            }

            foreach (string type in fileTypes)
            {
                Console.WriteLine("testing ---> " + type);
                var request = WebRequest.Create("https://t.nhentai.net/galleries/" + doujinMediaID + "/" + page + type);
                try {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    Console.WriteLine("error ---> " + type);
                    continue;
                }
            
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("found ---> " + type);
                        doujin.thumbnailExt = type;
                        return type;

                    case HttpStatusCode.InternalServerError:
                        return fileTypes[0];

                    case HttpStatusCode.NotFound:
                        return fileTypes[0];

                    case HttpStatusCode.Forbidden:
                        return fileTypes[0];

                    case HttpStatusCode.RequestTimeout:
                        return fileTypes[0];

                }

                }
            return null;
        }
        public static Result Search(string search, string tagsIn, string artistIn, string characterIn, string parodyIn, Boolean popOrder, string lang, int page)
        {
            string query = "";
            string url = "https://nhentai.net/api/galleries/search?query=";
            List<Doujin> doujinshi = new List<Doujin>();
            List<System.Windows.Controls.Image> images = new List<Image>();
            Result result = new Result();

            if (search != "")
            {
                query += search + "+";
            }
            if (tagsIn != "")
            {
                if (tagsIn.Contains(","))
                {
                    foreach (string str in tagsIn.Split(','))
                    {
                        query += ("tag%3A\"" + str + "\"+");
                    }
                }
                else
                {
                    query += ("tag%3A\"" + tagsIn + "\"+");
                }
            }
            if (artistIn != "")
            {
                if (artistIn.Contains(","))
                {
                    foreach (string str in artistIn.Split(','))
                    {
                        query += ("artist%3A\"" + str + "\"+");
                    }
                }
                else
                {
                    query += ("artist%3A\"" + artistIn + "\"+");
                }


            }
            if (characterIn != "")
            {
                if (characterIn.Contains(","))
                {
                    foreach (string str in characterIn.Split(','))
                    {
                        query += ("character%3A\"" + str + "\"+");
                    }
                }
                else
                {
                    query += ("character%3A\"" + characterIn + "\"+");
                }
            }
            if (parodyIn != "")
            {
                if (parodyIn.Contains(","))
                {
                    foreach (string str in characterIn.Split(','))
                    {
                        query += ("parody%3A\"" + str + "\"+");
                    }
                }
                else
                {
                    query += ("parody%3A\"" + parodyIn + "\"+");
                }
            }
            query += lang;
            if (popOrder)
            {
                query += "&order=popular";
            }
            query += "&page=" + page;
            Console.WriteLine(url + query);
            var request = WebRequest.Create("https://nhentai.net/api/galleries/search?query=" + query);
            request.ContentType = "application/xml; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();
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
            foreach (XElement element in table.Elements())
            {
                
                if (element.Name == "result")
                {
                    Doujin doujin = new Doujin();
                    doujinshi.Add(doujin);
                    string extension = "";
                    foreach (var title in element.Elements())
                    {
                        if (title.Name == "id")
                        {
                            id = int.Parse(title.Value);
                            doujin.nhentaiId = id;
                            Console.WriteLine($"current id is {id}");
                        }
                        if (title.Name == "media_id")
                        {
                            doujin.mediaId = int.Parse(title.Value);
                            Image img = new Image();
                            images.Add(img);
                            //Console.WriteLine("extension is " + extension);
                            //doujin.coverExt = extension;
                            doujin.coverUrl = "https://t.nhentai.net/galleries/" + title.Value + "/1t";
                            




                        }
                        if (title.Name == "images")
                        {
                            
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
                else
                {
                    continue;
                }
            }
            
            result.images = images;
            result.doujinshi = doujinshi;
            query = "";
            return result;
        }
        
    
            
        
        public static List<string> GetPage(Doujin doujin)
        {
            var request = WebRequest.Create("https://nhentai.net/api/gallery/" + doujin.nhentaiId);
            Console.WriteLine(doujin.nhentaiId);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            XNode node = JsonConvert.DeserializeXNode(text, "Root");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.ToString());
            doc.Save("test.xml");
            List<string> extensions = new List<string>();
            XElement table = XElement.Parse(node.ToString());
            foreach (XElement element in table.Elements())
            {
                        if (element.Name == "images")
                        {

                            foreach (var pages in element.Elements())
                            {
                                if (pages.Name == "pages")
                                {
                                    foreach (var t in pages.Elements())
                                    {
                                        if (t.Value == "j")
                                        {
                                            extensions.Add(".jpg");
                                        }
                                        else if (t.Value == "p")
                                        {
                                            extensions.Add(".png");
                                        }
                                    }
                                }
                            }
                        }
                    
            }
            Console.WriteLine("REEEEEEEEEEEEEEEEEEEEEEEEE " + extensions.Count);
            return extensions;
        }

        public static List<string> GetPage(int nhentaiid)
        {
            var request = WebRequest.Create("https://nhentai.net/api/gallery/" + nhentaiid);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            XNode node = JsonConvert.DeserializeXNode(text, "Root");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.ToString());
            doc.Save("test.xml");
            List<string> extensions = new List<string>();
            XElement table = XElement.Parse(node.ToString());
            foreach (XElement element in table.Elements())
            {
                if (element.Name == "images")
                {

                    foreach (var pages in element.Elements())
                    {
                        if (pages.Name == "pages")
                        {
                            foreach (var t in pages.Elements())
                            {
                                if (t.Value == "j")
                                {
                                    extensions.Add(".jpg");
                                }
                                else if (t.Value == "p")
                                {
                                    extensions.Add(".png");
                                }
                            }
                        }
                    }
                }

            }
            Console.WriteLine("REEEEEEEEEEEEEEEEEEEEEEEEE " + extensions.Count);
            return extensions;
        }
        public static void SaveDoujin(Doujin doujin)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"");

            
        }
        public static void FavoriteSave(Doujin doujin)
        {
            doujin.favorised = true;
            Console.WriteLine("Doujin: " + doujin.fullName + " (" + doujin.id + ") " + "got favorised");
        }

        public static Doujin DataRowToDoujin(DoujinDataRow row)
        {
            return new Doujin(row.ID,row.nHentaiID,row.mediaID,row.pages,row.name,row.fullName,row.extension,row.extension,row.language,row.favorite,row.coverUrl);
        }
        public static void AddDoujinDataRow(Doujin d, DoujinDataDataTable dt)
        {
            DoujinDataRow rowDoujinDataRow = ((DoujinDataRow)(dt.NewRow()));
            object[] columnValuesArray = new object[] {
                        dt.Rows.Count,
                        d.nhentaiId,
                        d.mediaId,
                        d.name,
                        d.fullName,
                        d.Artist(),
                        d.Character(),
                        d.Parody(),
                        d.Group(),
                        d.TagsConcat(),
                        d.language,
                        d.favorised,
                        d.pageCount,
                        d.coverUrl,
                        d.coverExt};
            rowDoujinDataRow.ItemArray = columnValuesArray;
            dt.Rows.Add(rowDoujinDataRow);
        }
        public static bool CheckFavorised(Doujin doujin)
        {
            try
            {
                if (Database.DatabaseControler.favorites.Contains(Database.DatabaseControler.favorites.FindBynHentaiID(doujin.nhentaiId)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool DoujinIsPartOfDataTable(Doujin doujin, DoujinDataDataTable dt)
        {
            try
            {
                if (dt.Contains(dt.FindBynHentaiID(doujin.nhentaiId)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool DoujinIsPartOfDataTable(int nHentaiId, DoujinDataDataTable dt)
        {
            try
            {
                if (dt.Contains(dt.FindBynHentaiID(nHentaiId)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        public static System.Drawing.Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }
        public static void CheckForNews()
        {

        }
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && (string)frameworkElement.Tag == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }


    }
    public class Result
    {
        public List<Doujin> doujinshi = new List<Doujin>();
        public List<Image> images = new List<Image>();
    }
    

}

    