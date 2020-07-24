using Doujin_Interface.Database;
using Doujin_Interface.UpdateFeed;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Doujin_Interface.UiElements.UpdateFeed
{
    /// <summary>
    /// Interaktionslogik für NotifyerWindow.xaml
    /// </summary>
    public partial class NotifyerElement : UserControl
    {
        public UpdateChecker updateChecker;
        public NotifyerElement()
        {
            InitializeComponent();
            if (File.Exists("ReleaseNotifyer.xml"))
            {
                DatabaseControler.updates.ReadXml("ReleaseNotifyer.xml");
                foreach (var row in Database.DatabaseControler.updates)
                {
                    var item = new ListBoxItem();
                    item.Content = row.Name;
                    item.Tag = row.ID;
                    item.Foreground = Brushes.White;
                    list.Items.Add(item);
                }

            }



            updateChecker = new UpdateChecker(this);
            this.Visibility = Visibility.Hidden;
            var margin = this.Margin;
            margin.Left = 84;
            Margin = margin;

        }
        public async Task<List<DoujinControl>> DisplayNewUploads(UpdateChecker checker, string distinct)
        {
            List<DoujinControl> list = new List<DoujinControl>();

            
            foreach (var item in checker.newReleases)
            {
                if (item.Value == distinct)
                {
                    DoujinControl control;
                    control = new DoujinControl(item.Key);
                    list.Add(control);
                    await Task.Run(delegate
                    {
                        int i = 1;
                        i++;
                    });
                }
                
            }
            return list;


        }
        public async Task Test(UpdateChecker checker)
        {
            foreach (var distinct in checker.newReleases.Values.Distinct())
            {
                var scroll = new ScrollViewer();
                var wrap = new WrapPanel();
                var tab = new TabItem();
                tab.Header = distinct;
                tab.Content = scroll;
                tabs.Items.Add(tab);
                scroll.Content = wrap;
                foreach (var item in await DisplayNewUploads(checker, distinct))
                {
                    wrap.Children.Add(item);
                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var type = (Types)comboBox.SelectedIndex;
            var item = new ListBoxItem();
            item.Content = input.Text;

            item.Foreground = Brushes.White;
            list.Items.Add(item);
            updateChecker.AddObject(input.Text, type);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (ListBoxItem)list.SelectedItem;

            list.Items.Remove(item);
        }
    }
}
