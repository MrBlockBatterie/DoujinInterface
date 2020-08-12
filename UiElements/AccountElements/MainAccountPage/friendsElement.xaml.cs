using Doujin_Interface.Connection;
using Doujin_Interface.Notifications;
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


namespace Doujin_Interface.UiElements.AccountElements.MainAccountPage
{
    /// <summary>
    /// Interaktionslogik für showcaseElement.xaml
    /// </summary>
    public partial class FriendsElement : UserControl
    {
        private bool initialized = false;
        public ApiHelper apiHelper;
        public FriendsElement()
        {
            InitializeComponent();
        }

        public async Task ShowFriends()
        {

            if (initialized == false)
            {
                try
                {
                    var mutuals = await apiHelper.GetFriends();
                    foreach (var item in mutuals.Friends)
                    {
                        FriendControl friend = new FriendControl(item, 0, 0);
                        friendsGrid.Children.Add(friend);
                    }
                    initialized = true;
                }
                catch (Exception e)
                {
                    DoujinUtility.MainWindow.notificationPanellul.Children.Add(Notifications.Notifications.NotificationNoImg("An error occured", "", e.Message));
                } 
            }
            else
            {

            }
        }

        private async void addButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
               
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            await apiHelper.AddFriend(addFriendsBox.Text);
        }

        private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            friendsGrid.Children.Clear();
            initialized = false;
            await ShowFriends();
        }
    }
}
