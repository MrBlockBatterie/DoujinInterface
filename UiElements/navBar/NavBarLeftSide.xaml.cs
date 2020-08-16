using Doujin_Interface.UiElements;
using Sankaku_Interface;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Doujin_Interface.Notifications;
using Doujin_Interface.UiElements.NavBar;
using System.Windows.Markup.Localizer;

namespace Doujin_Interface.uiElements.navBar
{
    /// <summary>
    /// Interaktionslogik für NavBarLeftSide.xaml
    /// </summary>
    public partial class NavBarLeftSide : UserControl
    {
        MainWindow window;

        private static NavBarElement homeElement = new NavBarElement("Home", "homeIcon.png");
        private static NavBarElement favElement = new NavBarElement("Favorites", "favIcon.png");
        private static NavBarElement bookmarkElement = new NavBarElement("Bookmarks", "bookmarkIcon.png");
        private static NavBarElement hgamesElement = new NavBarElement("Games", "gameIcon.png");
        private static NavBarElement notificationsElement = new NavBarElement("Notficiations", "notificationsIcon.png");
        private static NavBarElement updateFeedElement = new NavBarElement("Update Feed", "notificationsIcon.png");
        private static NavBarElement accountElement = new NavBarElement("Account", "accountIcon.png");
        private static NavBarElement settingsElement = new NavBarElement("Settings", "settingsIcon.png");
        private NavBarElement[] children =
        {
            homeElement,    
            favElement,
            bookmarkElement,
            hgamesElement,
            notificationsElement,
            updateFeedElement,
            accountElement,
            settingsElement
        };
        UiState uiState
        {
            get { return _uiState; }
            set
            {
                switch (_uiState)
                {
                    case UiState.Home:
                        window.scroll.Visibility = Visibility.Hidden;
                        window.scroll.IsEnabled = false;
                        break;
                    case UiState.Favorites:
                        window.favscroll.Visibility = Visibility.Hidden;
                        window.favscroll.IsEnabled = false;
                        break;
                    case UiState.Games:
                        break;
                    case UiState.Account:
                        window.registerAndLoginElement.Visibility = Visibility.Hidden;
                        window.accountElement.Visibility = Visibility.Hidden;
                        break;
                    case UiState.Notifications:
                        window.notificationscroll.Visibility = Visibility.Hidden;
                        window.notificationsgrid.Children.Clear();
                        break;
                    case UiState.Settings:
                        window.settingElement.Visibility = Visibility.Hidden;
                        window.settingElement.IsEnabled = false;
                        break;
                    case UiState.Notifyer:
                        window.notifyer.Visibility = Visibility.Hidden;
                        window.notifyer.IsEnabled = false;
                        break;
                    case UiState.Bookmarks:
                        window.bookmarks.Visibility = Visibility.Hidden;
                        break;


                }
                _uiState = value;
                switch (_uiState)
                {
                    case UiState.Home:
                        window.DisplayHome();
                        break;
                    case UiState.Favorites:
                        window.DisplayFavorites();
                        break;
                    case UiState.Games:
                        break;
                    case UiState.Account:
                        window.DisplayAccount();
                        break;
                    case UiState.Notifications:
                        window.DisplayNotifications();
                        break;
                    case UiState.Settings:
                        window.DisplaySettings();
                        break;
                    case UiState.Notifyer:
                        window.DisplayNotifyer();
                        break;
                    case UiState.Bookmarks:
                        window.DisplayBookmarks();
                        break;
                }
            }
        }
        private UiState _uiState = UiState.Home;
    

        public NavBarLeftSide()
        {
            InitializeComponent();
            CreateItems();
            for (int i = 0; i < rootGrid.Children.OfType<NavBarElement>().Count(); i++)
            {
                var item = rootGrid.Children.OfType<NavBarElement>().ElementAt(i);
                //item.Margin = new Thickness { Top = 54*(i+1), Left = 10 };
                var margin = item.Margin;
                margin.Top = 54 * (i + 1);
                margin.Left = 10;
                item.Margin = margin;
                item.VerticalAlignment = VerticalAlignment.Top;
                item.HorizontalAlignment = HorizontalAlignment.Left;
                Panel.SetZIndex(item, 10 + i);
            }
            
        }

        private void CreateItems()
        {
            homeElement.MouseLeftButtonDown += homeGrid_MouseLeftButtonDown;
            favElement.MouseLeftButtonDown += favGrid_MouseLeftButtonDown;
            //hgamesElement.MouseLeftButtonDown += homeGrid_MouseLeftButtonDown;
            notificationsElement.MouseLeftButtonDown += notificationsGrid_MouseLeftButtonDown;
            updateFeedElement.MouseLeftButtonDown += updateFeedGrid_MouseLeftButtonDown;
            accountElement.MouseLeftButtonDown += accountGrid_MouseLeftButtonDown;
            settingsElement.MouseLeftButtonDown += settingsGrid_MouseLeftButtonDown;
            bookmarkElement.MouseLeftButtonDown += BookmarkElement_MouseLeftButtonDown;
            foreach (var item in children)
            { 
                rootGrid.Children.Add(item);
            }
                
        }

        private void BookmarkElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Bookmarks)
            {
                uiState = UiState.Bookmarks;
                NavBar.MoveActiveIndicator(this, bookmarkElement);
                NavBar.alwaysMaxed = false;
            }
        }

        public NavBarLeftSide(MainWindow mainWindow)
        {
            InitializeComponent();
            window = mainWindow;
            CreateItems();
            for (int i = 0; i < rootGrid.Children.OfType<NavBarElement>().Count(); i++)
            {
                var item = rootGrid.Children.OfType<NavBarElement>().ElementAt(i);
                item.Margin = new Thickness { Top = (54 * (i + 1)) + 10 , Left = 11 };
                item.VerticalAlignment = VerticalAlignment.Top;
                item.HorizontalAlignment = HorizontalAlignment.Left;
                Panel.SetZIndex(item, 10 + i);
            }
        }

        private void homeText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void homeGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Home)
            {
                uiState = UiState.Home;
                NavBar.MoveActiveIndicator(this, homeElement);
                NavBar.alwaysMaxed = false;
            }
        }

        private void favGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Favorites)
            {
                uiState = UiState.Favorites;
                NavBar.MoveActiveIndicator(this, favElement);
                NavBar.alwaysMaxed = false;
            }
        }

        private void notificationsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Notifications)
            {
                uiState = UiState.Notifications;
                NavBar.MoveActiveIndicator(this, notificationsElement);
                NavBar.alwaysMaxed = true;
                Notifications.Notifications.AddNotifications();
            }
        }

        private void updateFeedGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Notifyer)
            {
                uiState = UiState.Notifyer;
                NavBar.MoveActiveIndicator(this, updateFeedElement);
                NavBar.alwaysMaxed = false;
            }
        }

        private void accountGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Account)
            {
                uiState = UiState.Account;
                NavBar.MoveActiveIndicator(this, accountElement);
                NavBar.alwaysMaxed = false;
            }
        }

        private void settingsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Settings)
            {
                uiState = UiState.Settings;
                NavBar.MoveActiveIndicator(this, settingsElement);
                NavBar.alwaysMaxed = true;
            }
        }
    }
}
