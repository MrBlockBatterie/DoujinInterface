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

namespace Doujin_Interface.uiElements.navBar
{
    /// <summary>
    /// Interaktionslogik für NavBarLeftSide.xaml
    /// </summary>
    public partial class NavBarLeftSide : UserControl
    {
        MainWindow window;
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
                }
            }
        }
        private UiState _uiState = UiState.Home;
    
        public NavBarLeftSide()
        {
            InitializeComponent();
            
        }
        public NavBarLeftSide(MainWindow mainWindow)
        {
            InitializeComponent();
            window = mainWindow;
        }

        private void homeText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void homeGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Home)
            {
                uiState = UiState.Home;
                NavBar.MoveActivIndicator(this, homeGrid);
                NavBar.alwaysMaxed = false;
            }
        }

        private void favGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Favorites)
            {
                uiState = UiState.Favorites;
                NavBar.MoveActivIndicator(this, favGrid);
                NavBar.alwaysMaxed = false;
            }
        }

        private void notificationsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Notifications)
            {
                uiState = UiState.Notifications;
                NavBar.MoveActivIndicator(this, notificationsGrid);
                NavBar.alwaysMaxed = true;
                Notifications.Notifications.AddNotifications();
            }
        }

        private void updateFeedGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Notifyer)
            {
                uiState = UiState.Notifyer;
                NavBar.MoveActivIndicator(this, updateFeedGrid);
                NavBar.alwaysMaxed = false;
            }
        }

        private void accountGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Account)
            {
                uiState = UiState.Account;
                NavBar.MoveActivIndicator(this, accountGrid);
                NavBar.alwaysMaxed = false;
            }
        }

        private void settingsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (uiState != UiState.Settings)
            {
                uiState = UiState.Settings;
                NavBar.MoveActivIndicator(this, settingsGrid);
                NavBar.alwaysMaxed = true;
            }
        }
    }
}
