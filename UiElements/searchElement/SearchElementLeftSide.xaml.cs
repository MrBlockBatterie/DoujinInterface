using Doujin_Interface.Database;
using Doujin_Interface.Notifications;
using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doujin_Interface.uiElements.searchElement
{
    /// <summary>
    /// Interaktionslogik für SearchElementLeftSide.xaml
    /// </summary>
    public partial class SearchElementLeftSide : UserControl
    {
        public string dirPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private SearchLang langEnum = SearchLang.ENGLISH;
        
        WrapPanel picgrid;
        Label loadMore;
        Grid grid;
        MainWindow window;
        Watchlist watchlist;
        public SearchElementLeftSide(WrapPanel picgridIn, Grid gridIn, Window windowIn)
        {
            InitializeComponent();
            picgrid = picgridIn;
            grid = gridIn;
            window = (MainWindow)windowIn;
            window.Closing += delegate { watchlist.Close(); };
            watchlist = new Watchlist(generalSearch, tagsSearch, artistSearch, charaSearch, parodySearch);
            watchlist.Show();

            this.Height = 1080;
            this.Width = 228;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Name = "sElement";
            this.Margin = new Thickness(50, 0, 0, 0);
        }
        
        
        private bool popOrder = false;
        string searchLang()
        {
            switch (langEnum)
            {
                case SearchLang.ENGLISH:
                    return "language%3Aenglish";
                case SearchLang.GERMAN:
                    return "language%3Agerman";
                case SearchLang.JAPANESE:
                    return "language%3Ajapanese";
                case SearchLang.ALL:
                    return "";
            }
            return "";
        }
        public void Page(int page)
        {
            try
            {
                Result result = DoujinUtility.Search(generalSearch.Text, tagsSearch.Text, artistSearch.Text, charaSearch.Text, parodySearch.Text, popOrder, searchLang(), page);
                List<Doujin> localDoujinList = result.doujinshi;
                foreach (Image img in result.images)
                {
                    //Image img = new Image();
                    DoujinControl doujinControl = new DoujinControl();

                    Doujin doujin = result.doujinshi.ElementAt(result.images.IndexOf(img));
                    
                    Console.WriteLine(result.doujinshi.ElementAt(result.images.IndexOf(img)).coverUrl);

                    foreach (Database.DoujinSet.DoujinDataRow row in Database.DatabaseControler.favorites)
                    {
                        if (row.nHentaiID == doujin.nhentaiId)
                        {

                            doujin = new Doujin(row.nHentaiID);
                            doujin.favorised = true;

                            Console.WriteLine("fav matched");
                            doujinControl.heart.Source = new BitmapImage(new Uri("pack://application:,,,/heart_fav.png"));


                        }
                    }

                    picgrid.Children.Add(doujinControl);
                    var source = new BitmapImage(new Uri(result.doujinshi.ElementAt(result.images.IndexOf(img)).coverUrl));
                    doujinControl.img.Source = source;
                    double ratio = source.Width / source.Height;
                    doujinControl.img.ToolTip = doujin.name;
                    doujinControl.img.Tag = doujin.nhentaiId;
                    doujinControl.img.Width = 160;
                    doujinControl.img.Height = 230;
                    doujinControl.img.Margin = new Thickness(5, 3, 5, 3);
                    doujinControl.img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
                    doujinControl.img.MouseRightButtonDown += Img_MouseRightButtonDown;
                    doujinControl.heart.MouseLeftButtonDown += Heart_MouseLeftButtonDown;
                    Thickness margin = doujinControl.Margin;
                    margin.Right = 10;
                    margin.Bottom = 10;
                    doujinControl.Margin = margin;
                    doujinControl.doujinName.Text = doujin.name;
                    doujinControl.doujinCreator.Text = doujin.ArtistsConcat();
                    doujinControl.doujinTags.Text = doujin.TagsConcat();

                    //DoujinUtility.AddDoujinDataRow(doujin, DatabaseControler.mainDataTable);
                }
                DatabaseControler.mainDataTable.Init(localDoujinList);
            }
            catch
            {

            }


        }

        private void Heart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image obj = (Image)sender;
            Grid grid = (Grid)obj.Parent;
            DoujinControl control = (DoujinControl)grid.Parent;
            int nhId = (int)control.img.Tag;
            if(DoujinUtility.CheckFavorised(new Doujin(nhId)))
            {
                DatabaseControler.favorites.FindBynHentaiID(nhId).favorite = false;
                DatabaseControler.mainDataTable.FindBynHentaiID(nhId).favorite = false;
                control.heart.Source = new BitmapImage(new Uri("pack://application:,,,/heart_nofav.png"));
                DatabaseControler.favorites.RemoveDoujinDataRow(DatabaseControler.favorites.FindBynHentaiID(nhId));
                
                DatabaseControler.favorites.WriteXml("favs.xml");
            }
            else
            {
                Doujin doujin = new Doujin(nhId);
                DatabaseControler.mainDataTable.FindBynHentaiID(nhId).favorite = true;
                control.heart.Source = new BitmapImage(new Uri("pack://application:,,,/heart_fav.png"));
                DoujinUtility.AddDoujinDataRow(doujin, DatabaseControler.favorites);
                DatabaseControler.favorites.WriteXml("favs.xml");
                //.Add();

                var notify = Notifications.Notifications.NotificationNoImg(window, doujin.name, "", "The doujin got favorised and you can acess it at your favourite page");
                
                DoujinUtility.MainWindow.notificationPanellul.Children.Add(notify);
            }

        }

        private void Img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            DetailsWindow detailsWindow = new DetailsWindow(new Doujin((int)img.Tag));
            detailsWindow.Show();
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            var img = (Image)sender;
            using (var doujin = new Doujin((int)img.Tag))
            {
                var viewer = new DoujinViewer(doujin);
                viewer.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoujinUtility.MainWindow.scroll.ScrollToVerticalOffset(0);
            DatabaseControler.mainDataTable.Clear();
            picgrid.Children.Clear();
            
            window.page = 2;
            Page(1);
        }

        private void langEnButton_Checked(object sender, RoutedEventArgs e)
        {
            langEnum = SearchLang.ENGLISH;
        }

        private void langDeButton_Checked(object sender, RoutedEventArgs e)
        {
            langEnum = SearchLang.GERMAN;
        }

        private void langJpButton_Checked(object sender, RoutedEventArgs e)
        {
            langEnum = SearchLang.JAPANESE;
        }

        private void langAllButton_Checked(object sender, RoutedEventArgs e)
        {
            langEnum = SearchLang.ALL;
        }

        private void sortPopButton_Checked(object sender, RoutedEventArgs e)
        {
            popOrder = true;
        }

        private void sortDateButton_Checked(object sender, RoutedEventArgs e)
        {
            popOrder = false;
        }
    }
    public enum SearchLang
    {
        ENGLISH,
        GERMAN,
        JAPANESE,
        ALL
    }
}

