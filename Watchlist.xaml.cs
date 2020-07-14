using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace Doujin_Interface
{
    /// <summary>
    /// Interaktionslogik für Wwatchlist.xaml
    /// </summary>
    public partial class Watchlist : Window
    {
        private XDocument doc;
        private XElement xroot;
        private XElement xEGeneral;
        private XElement xETags;
        private XElement xEArtist;
        private XElement xEChara;
        private XElement xEParody;
        private List<XElement> xElements = new List<XElement>();
        private TextBox[] textIn;
        
        private Parents[] parents = {Parents.GENERAL,
                                     Parents.TAGS,
                                     Parents.ARTIST,
                                     Parents.CHARA,
                                     Parents.PARODY};
                                                    
        public Watchlist(TextBox a, TextBox b, TextBox c, TextBox d, TextBox e)
        {
            InitializeComponent();
            CheckFile();
            textIn = new TextBox[] { a, b, c, d, e };
        }
        private void WriteXml(string content, XElement element)
        {
            
        }
        private void ParseXml()
        {
            doc = XDocument.Load("watchlist.xml");
            xroot = doc.Element("root");
            xEGeneral = xroot.Element("general");
            xETags = xroot.Element("tags");
            xEArtist = xroot.Element("artist");
            xEChara = xroot.Element("chara");
            xEParody = xroot.Element("parody");
            int c = 0;
            foreach(TreeViewItem te in Root.Items)
            {
                var sxe = xroot.Elements().ToList().ElementAt(c).Elements();
                foreach (XElement xe in sxe)
                {
                    
                    AddItem(xe.Value,te);
                }
                c++;
                
            }
        }
        private void CheckFile()
        {
            if (File.Exists("watchlist.xml"))
            {
                ParseXml();
            }
            else
            {
                doc = new XDocument();
                xroot = new XElement("root");
                doc.Add(xroot);
                xEGeneral = new XElement("general");
                xroot.Add(xEGeneral);
                xETags = new XElement("tags");
                xroot.Add(xETags);
                xEArtist = new XElement("artist");
                xroot.Add(xEArtist);
                xEChara = new XElement("chara");
                xroot.Add(xEChara);
                xEParody = new XElement("parody");
                xroot.Add(xEParody);
                doc.Save("watchlist.xml");
            }
        }
        private void AddItem(TextBox boxIn, TreeViewItem parentIn)
        {

            var item = new TreeViewItem();
            var x = parents[Root.Items.IndexOf(parentIn)];
            XElement nXe = new XElement(x.ToString(),boxIn.Text);
            item.Header = boxIn.Text;
            boxIn.Text = "";
            item.Foreground = Brushes.White;
            parentIn.Items.Insert(parentIn.Items.IndexOf(boxIn), item);
            

            
            switch (x)
            {
                case Parents.GENERAL:
                    xEGeneral.Add(nXe);
                    break;
                case Parents.TAGS:
                    xETags.Add(nXe);
                    break;
                case Parents.ARTIST:
                    xEArtist.Add(nXe);
                    break;
                case Parents.CHARA:
                    xEChara.Add(nXe);
                    break;
                case Parents.PARODY:
                    xEParody.Add(nXe);
                    break;

            }
            doc.Save("watchlist.xml");
            item.MouseDoubleClick += Item_MouseLeftButtonDown;
        }
        private void AddItem(string contentIn, TreeViewItem parentIn)
        {
            var item = new TreeViewItem();
            item.Header = contentIn;
            item.Foreground = Brushes.White;
            parentIn.Items.Insert(0, item);
            item.MouseDoubleClick += Item_MouseLeftButtonDown;
        }

        private void Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (TreeViewItem)sender;
            var x = parents[Root.Items.IndexOf(item.Parent)];
            TextBox destBox = null;
            switch (x)
            {
                case Parents.GENERAL:
                    destBox = textIn[0];
                    break;
                case Parents.TAGS:
                    destBox = textIn[1];
                    break;
                case Parents.ARTIST:
                    destBox = textIn[2];
                    break;
                case Parents.CHARA:
                    destBox = textIn[3];
                    break;
                case Parents.PARODY:
                    destBox = textIn[4];
                    break;
            }
            if (destBox.Text != "")
            {
                destBox.Text = (destBox.Text + "," + item.Header);
            }
            else
            { 
                destBox.Text += item.Header;
            }    
        }
        private void ParodyWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ParodyWatch.Text != "")
                {
                    AddItem(ParodyWatch, ParodyList);
                }
            }
        }

        private void CharaWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CharaWatch.Text != "")
                {
                    AddItem(CharaWatch, CharaList);
                }
            }
        }

        private void ArtistWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ArtistWatch.Text != "")
                {
                    AddItem(ArtistWatch, ArtistList);
                }
            }
        }

        private void TagsWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TagsWatch.Text != "")
                {
                    AddItem(TagsWatch, TagsList);
                }
            }
        }

        private void GeneralWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (GeneralWatch.Text != "")
                {
                    AddItem(GeneralWatch, GeneralList);
                }
            }
        }
    }
    
    
    public enum Parents
    {
        GENERAL,
        TAGS,
        ARTIST,
        CHARA,
        PARODY
    }
}
