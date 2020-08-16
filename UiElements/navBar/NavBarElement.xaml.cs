using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doujin_Interface.UiElements.NavBar
{
    /// <summary>
    /// Interaktionslogik für NavBarElement.xaml
    /// </summary>
    public partial class NavBarElement : UserControl
    {
        public NavBarElement()
        {
            InitializeComponent();
        }
        public NavBarElement(string name, string iconname)
        {
            InitializeComponent();
            text.Text = name;
            icon.Source = new BitmapImage(new Uri($"pack://application:,,,/UiElements/NavBar/{iconname}"));
            
        }
    }
}
