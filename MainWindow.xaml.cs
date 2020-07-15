using Doujin_Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml;
using System.Xml.Linq;
using static Doujin_Interface.Database.DoujinSet;
using Doujin_Interface.Notifications;
using System.Threading;
using Doujin_Interface.uiElements.navBar;
using Doujin_Interface.uiElements.searchElement;
using Doujin_Interface.UiElements.SettingsElement;
using Doujin_Interface.Database;
using Doujin_Interface.Notifications.NotificationControlListControl;
using Doujin_Interface.ConnectionStuff;
using Doujin_Interface.UiElements.AccountElements;
using Doujin_Interface.ConnectionStuff.Shared;
using Doujin_Interface.UiElements.UpdateFeed;
using Doujin_Interface.UiElements.AccountElements.LoginAndRegister;

namespace Sankaku_Interface
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public string url = "https://nhentai.net/api/galleries/search?query=";
        public string lang = "language%3Aenglish";
        public Boolean popOrder = false;
        public string query;
        Boolean fKey = false;
        Boolean strgKey = false;
        public string dirPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static DoujinDataDataTable dt = new DoujinDataDataTable();
        public static DoujinDataDataTable favs = new DoujinDataDataTable();
        private List<Image> images = new List<Image>();
        private List<Doujin> doujinshi = new List<Doujin>();
        private List<Image> favDoujinImage = new List<Image>();
        private List<Doujin> favDoujin = new List<Doujin>();
        private List<int> favId = new List<int>();
        public List<NotificationControlList> NotificationList = new List<NotificationControlList>();
        private static readonly HttpClient client = new HttpClient();
        public string[] extensions = {".png",".jpg"};
        public int page = 1;
        public SearchElementLeftSide search;
        public NewSettingsElement settingElement = new NewSettingsElement();
        public RegisterElementE registerAndLoginElement;
        public NotifyerElement notifyer = new NotifyerElement(); //notifyer
        public SolidColorBrush animatedBrush = new SolidColorBrush();
        public MainWindow()
        {
            
            InitializeComponent();
            

            DoujinUtility.MainWindow = this;
            DoujinUtility.MainWindow.animatedBrush.Color = Color.FromArgb(255, 138, 0, 219);
            //var test = new HIDoujin("Bokurano@Mii-chan | Our @Mii-chan","english", 1278968);

            //test2.AddObject("petenshi",Doujin_Interface.UpdateFeed.Types.Arist);
            //(throw new NotImplementedException();

            //Application.Current.Resources["UiDarkGray"] = new SolidColorBrush(Color.FromArgb(100, 200, 20, 20));
            //Color.FromArgb(100, 200, 20, 20);









            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            var searchElement = new SE();
            search = SE.CreateSearchElement(picgrid, rootGrid, this);
            rootGrid.Children.Add(search);

            registerAndLoginElement = RegisterElement.CreateRegisterElement();
            rootGrid.Children.Add(registerAndLoginElement);
            rootGrid.Children.Add(notifyer);
            
            
            //has to be added last
            var navBar = new NavBar();
            rootGrid.Children.Add(NavBar.CreateNavBar(this));

           


            Thickness margin = settingElement.Margin;
            margin.Left = 283;
            settingElement.Margin = margin;
            rootGrid.Children.Add(settingElement);

            if (File.Exists(DatabaseControler.favDataPath))
            {
                DatabaseControler.favorites.ReadXml(DatabaseControler.favDataPath);
                foreach (var row in DatabaseControler.favorites)
                {
                    row.favorite = true;
                    favId.Add(row.nHentaiID);
                    favDoujin.Add(DoujinUtility.DataRowToDoujin(row));
                    Console.WriteLine("fav added to list " + row.nHentaiID );
                    
                }
            }
            if (File.Exists("cache.xml"))
            {
                DatabaseControler.doujinCache.ReadXml("cache.xml");
                
            }
            //Page(1);




        }
        public void DisplayNotifyer()
        {
            notifyer.Visibility = Visibility.Visible;
            notifyer.IsEnabled = true;
            search.Visibility = Visibility.Hidden;
        }
        public void DisplayAccount()
        {
            registerAndLoginElement.Visibility = Visibility.Visible;
            search.Visibility = Visibility.Hidden;
        }
        public void DisplaySettings()
        {
            settingElement.Visibility = Visibility.Visible;
            settingElement.IsEnabled = true;
            search.Visibility = Visibility.Hidden;
        }

        public void DisplayNotifications()
        {
            search.Visibility = Visibility.Hidden;
            notificationscroll.Visibility = Visibility.Visible;
            notificationscroll.IsEnabled = true;
        }

        public void DisplayFavorites()
        {
            search.Visibility = Visibility.Hidden;
            favscroll.Visibility = Visibility.Visible;
            favscroll.IsEnabled = true;

            
            if (favgrid.Children.Count < DatabaseControler.favorites.Count)
            {
                foreach (DoujinDataRow row in DatabaseControler.favorites)
                {
                    /*
                    DoujinControl doujinControl = new DoujinControl(row.nHentaiID);
                    doujinControl.heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_fav.png"));
                    favgrid.Children.Add(doujinControl);
                    */
                    
                    Doujin doujin = DoujinUtility.DataRowToDoujin(row);
                    if (DoujinUtility.DoujinIsPartOfDataTable(doujin, DatabaseControler.localDummy))
                    {
                        continue;
                    }
                    DoujinControl doujinControl = new DoujinControl();
                    
                    favgrid.Children.Add(doujinControl);
                    
                    var source = new BitmapImage(new Uri(row.coverUrl));
                    doujinControl.img.Source = source;
                    doujinControl.img.ToolTip = doujin.name;
                    doujinControl.img.Tag = doujin.nhentaiId;
                    doujinControl.img.Width = 160;
                    doujinControl.img.Height = 230;
                    doujinControl.img.Margin = new Thickness(5, 3, 5, 3);
                    doujinControl.img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
                    doujinControl.img.MouseRightButtonDown += Img_MouseRightButtonDown;
                    doujinControl.heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_fav.png"));
                    doujinControl.heart.MouseLeftButtonDown += Heart_MouseLeftButtonDown;
                    Thickness margin = doujinControl.Margin;
                    margin.Right = 10;
                    margin.Bottom = 10;
                    doujinControl.Margin = margin;
                    doujinControl.doujinName.Text = doujin.name;
                    doujinControl.doujinCreator.Text = doujin.ArtistsConcat();
                    doujinControl.doujinTags.Text = doujin.TagsConcat();
                    


                    DatabaseControler.localDummy.AddDoujinDataRow(DatabaseControler.mainDataTable.Rows.Count, row.nHentaiID, row.mediaID, row.name, row.fullName, row.artist, row.character, row.parody, row.group, row.tags, row.language, true, row.pages, row.coverUrl, row.extension);
                }
                
                
            }
            else if (favgrid.Children.Count > DatabaseControler.favorites.Count)
            {
                List<DoujinControl> controlList = new List<DoujinControl>();
                foreach (DoujinControl doujinControl in favgrid.Children)
                {
                    if (!DoujinUtility.DoujinIsPartOfDataTable((int)doujinControl.img.Tag, DatabaseControler.favorites))
                    {
                        controlList.Add(doujinControl);
                        DatabaseControler.localDummy.RemoveDoujinDataRow(DatabaseControler.localDummy.FindBynHentaiID((int)doujinControl.img.Tag));

                    }
                }
                foreach (DoujinControl doujinControl in controlList)
                {
                    favgrid.Children.Remove(doujinControl);
                }
            }
            
        }

  

        public void DisplayHome()
        {
            scroll.Visibility = Visibility.Visible;
            scroll.IsEnabled = true;
            search.Visibility = Visibility.Visible;

        }

        private void Heart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            Grid grid = (Grid)img.Parent;
            DoujinControl control = (DoujinControl)grid.Parent;
            DatabaseControler.favorites.RemoveDoujinDataRow(DatabaseControler.favorites.FindBynHentaiID((int)control.img.Tag));
            favgrid.Children.Remove(control);
            if (DoujinUtility.DoujinIsPartOfDataTable((int)control.img.Tag, DatabaseControler.mainDataTable))
            {
                var picGridItem = (DoujinControl)picgrid.Children[DatabaseControler.mainDataTable.FindBynHentaiID((int)control.img.Tag).ID];
                picGridItem.heart.Source = new BitmapImage(new Uri("pack://application:,,,/heart_nofav.png"));
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                fKey = true;
            }
            if (e.Key == Key.LeftCtrl)
            {
                strgKey = true;
            }
            if (strgKey && fKey)
            {
                
            }
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                fKey = false;
            }
            if (e.Key == Key.LeftCtrl)
            {
                strgKey = false;
            }
        }

        private void Img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            DetailsWindow detailsWindow = new DetailsWindow(new Doujin((int)img.Tag));
            detailsWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

        }

        public void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            var viewer = new DoujinViewer(new Doujin((int)img.Tag));
            viewer.Show();            
        }
        public void Img_MouseLeftButtonDown(int mediaid, int nhentaiid, int pages, string coverurl, string ext)
        {

            var viewer = new DoujinViewer(mediaid,nhentaiid,pages,coverurl,ext);
            viewer.Show();
        }
        

        private void FavImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] param = favDoujinImage.ElementAt(favDoujinImage.IndexOf((Image)sender)).Tag.ToString().Split(',');
            var viewer = new DoujinViewer(int.Parse(param[0]), int.Parse(param[1]), int.Parse(param[2]), param[3], param[4]);
            viewer.Show();
        }

        private void LoadMore_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            page++;
            //Page(page);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            picgrid.Children.Clear();
            doujinshi.Clear();
            images.Clear();
            dt.Clear();
            //favs.Clear();
            page = 1;
            
            //Page(1);
        }
        
        private void scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = (ScrollViewer)sender;
            //Console.WriteLine($"{e.VerticalOffset} von {scroll.ScrollableHeight}");
            if (e.VerticalOffset == scroll.ScrollableHeight)
            {
                search.Page(page);
                page++;
            }
            
        }

        private void picgrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (notifyer.updateChecker.newReleases.Count > 0)
            {
                foreach (var item in notifyer.updateChecker.newReleases.Values.Distinct())
                {
                    this.notificationPanellul.Children.Add(Notifications.NotificationNoImg(this, $"New Doujin uploads available", $"{item}", ""));
                }
            }
            else
            {
                this.notificationPanellul.Children.Add(Notifications.NotificationNoImg(this, $"No new uploads on subscribed tags", "", ""));
            }
        }
    }
}
