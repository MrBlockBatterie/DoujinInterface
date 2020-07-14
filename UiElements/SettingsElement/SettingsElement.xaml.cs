using System;
using System.Collections.Generic;
using System.Configuration;
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
using WinForms = System.Windows.Forms;

namespace Doujin_Interface.UiElements.SettingsElement
{
    /// <summary>
    /// Interaktionslogik für SettingsElement.xaml
    /// </summary>
    public partial class SettingsElement : UserControl
    {
        public SettingsElement()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            this.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Savepath = doujinFilePath.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (WinForms.FolderBrowserDialog folder = new WinForms.FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == WinForms.DialogResult.OK)
                {
                    doujinFilePath.Text = folder.SelectedPath;
                }
            }
        }
    }
}
