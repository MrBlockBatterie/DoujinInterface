using Doujin_Interface.Connection;
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
            if (checkBox1.IsChecked.Value && checkBox2.IsChecked.Value)
            {
                if (password.Password == password_Copy.Password)
                {
                    var apiHelper = new ApiHelper();
                    await apiHelper.RegisterToServer(emailText.Text, password.Password, password_Copy.Password, username.Text); 
                }
            }
        }
    }
}
