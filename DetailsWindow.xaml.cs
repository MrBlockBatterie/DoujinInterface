using System;
using System.Collections.Generic;
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

namespace Doujin_Interface
{
    /// <summary>
    /// Interaktionslogik für DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(Doujin doujin)
        {
            InitializeComponent();
            titel.Content = doujin.name;
            cover.Source = new BitmapImage(new Uri(doujin.coverUrl));

            TreeViewItem ids = new TreeViewItem();
            ids.Header = "IDs";
            ids.Foreground = Brushes.White;
            Tags.Items.Add(ids);

                TreeViewItem id = new TreeViewItem();
                id.Header = "local DB ID: " + doujin.id;
                id.Foreground = Brushes.White;
                ids.Items.Add(id);

                TreeViewItem nId = new TreeViewItem();
                nId.Header = "nHentai ID: " + doujin.nhentaiId;
                nId.Foreground = Brushes.White;
                ids.Items.Add(nId);

                TreeViewItem mediaId = new TreeViewItem();
                mediaId.Header = "Media ID: " + doujin.mediaId;
                mediaId.Foreground = Brushes.White;
                ids.Items.Add(mediaId);

            TreeViewItem names = new TreeViewItem();
            names.Header = "Names";
            names.Foreground = Brushes.White;
            Tags.Items.Add(names);

                TreeViewItem prettyName = new TreeViewItem();
                prettyName.Header = "Pretty Name: "+ doujin.name;
                prettyName.Foreground = Brushes.White;
                names.Items.Add(prettyName);

                TreeViewItem fullName = new TreeViewItem();
                fullName.Header = "Full Name: " + doujin.fullName;
                fullName.Foreground = Brushes.White;
                names.Items.Add(fullName);

            if(doujin.Artist() == null)
            {
                TreeViewItem artists = new TreeViewItem();
                artists.Header = "Artists";
                artists.Foreground = Brushes.White;
                Tags.Items.Add(artists);

                foreach (string art in doujin.artists)
                {
                    TreeViewItem artist = new TreeViewItem();
                    artist.Header = art;
                    artist.Foreground = Brushes.White;
                    artists.Items.Add(artist);
                }
            }
            else
            {
                TreeViewItem artist = new TreeViewItem();
                artist.Header = "Artist: " + doujin.Artist();
                artist.Foreground = Brushes.White;
                Tags.Items.Add(artist);
            }

            if (doujin.Character() == null)
            {
                TreeViewItem top = new TreeViewItem();
                top.Header = "Characters";
                top.Foreground = Brushes.White;
                Tags.Items.Add(top);

                foreach (string selec in doujin.character)
                {
                    TreeViewItem child = new TreeViewItem();
                    child.Header = selec;
                    child.Foreground = Brushes.White;
                    top.Items.Add(child);
                }
            }
            else
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = "Character: " + doujin.Character();
                item.Foreground = Brushes.White;
                Tags.Items.Add(item);
            }

            if (doujin.Parody() == null)
            {
                TreeViewItem top = new TreeViewItem();
                top.Header = "Parodys";
                top.Foreground = Brushes.White;
                Tags.Items.Add(top);

                foreach (string selec in doujin.parodys)
                {
                    TreeViewItem child = new TreeViewItem();
                    child.Header = selec;
                    child.Foreground = Brushes.White;
                    top.Items.Add(child);
                }
            }
            else
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = "Parody: " + doujin.Parody();
                item.Foreground = Brushes.White;
                Tags.Items.Add(item);
            }

            if (doujin.Group() == null)
            {
                TreeViewItem top = new TreeViewItem();
                top.Header = "Groups";
                top.Foreground = Brushes.White;
                Tags.Items.Add(top);

                foreach (string selec in doujin.group)
                {
                    TreeViewItem child = new TreeViewItem();
                    child.Header = selec;
                    child.Foreground = Brushes.White;
                    top.Items.Add(child);
                }
            }
            else
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = "Group: " + doujin.Group();
                item.Foreground = Brushes.White;
                Tags.Items.Add(item);
            }

            TreeViewItem tag = new TreeViewItem();
            tag.Header = "Tags:";
            tag.Foreground = Brushes.White;
            Tags.Items.Add(tag);
            foreach(string selec in doujin.tags)
            {
                TreeViewItem child = new TreeViewItem();
                child.Header = selec;
                child.Foreground = Brushes.White;
                tag.Items.Add(child);
            }

            TreeViewItem lang = new TreeViewItem();
            lang.Header = $"Language: {doujin.language}";
            lang.Foreground = Brushes.White;
            Tags.Items.Add(lang);

        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
