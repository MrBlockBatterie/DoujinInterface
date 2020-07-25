using Doujin_Interface.Connection;
using Newtonsoft.Json;
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

namespace Doujin_Interface.UiElements.AccountElements.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class RegisterElementE : UserControl
    {
        bool registerMode = false;

        public RegisterElementE()
        {
            InitializeComponent();

            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.IsEnabled = true;
            Visibility = Visibility.Hidden;


        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (registerMode)
            {
                if (checkBox1.IsChecked.Value && checkBox2.IsChecked.Value)
                {
                    if (password.Password == password_Copy.Password)
                    {
                        var apiHelper = new ApiHelper();
                        await apiHelper.RegisterToServer(emailText.Text, password.Password, password_Copy.Password, username.Text);
                        if (await apiHelper.Login(username.Text, password.Password) == System.Net.HttpStatusCode.OK)
                        {
                            Visibility = Visibility.Hidden;
                            DoujinUtility.MainWindow.accountElement.user = JsonConvert.DeserializeObject<Connection.Models.AuthenticatedUser>(Properties.Settings.Default.User);
                            DoujinUtility.MainWindow.accountElement.loggedIn = true;
                            DoujinUtility.MainWindow.accountElement.Visibility = Visibility.Visible;
                            DoujinUtility.MainWindow.accountElement.apiHelper = apiHelper;
                        }
                    }
                }
            }
            else if (!registerMode)
            {
                var apiHelper = new ApiHelper();
                if (await apiHelper.Login(username.Text, password.Password) == System.Net.HttpStatusCode.OK)
                {
                    Visibility = Visibility.Hidden;
                    DoujinUtility.MainWindow.accountElement.user = JsonConvert.DeserializeObject<Connection.Models.AuthenticatedUser>(Properties.Settings.Default.User);
                    DoujinUtility.MainWindow.accountElement.loggedIn = true;
                    DoujinUtility.MainWindow.accountElement.Visibility = Visibility.Visible;
                    DoujinUtility.MainWindow.accountElement.apiHelper = apiHelper;
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElement[] elements = new UIElement[]
            {
                password_Copy,
                passwort_copyBorder,
                emailBorder,
                emailText,
                checkBox1,
                checkBox2
            };

            if (registerMode)
            {
                registerMode = false;
                foreach (var item in elements)
                {
                    item.Visibility = Visibility.Hidden;
                }
                button.Content = "Login";
                text.Text = "Click here to Register";
            }
            else if (!registerMode)
            {
                registerMode = true;
                foreach (var item in elements)
                {
                    item.Visibility = Visibility.Visible;
                }
                button.Content = "Register";
                text.Text = "Click here to Login";
            }
        }
    }
}
