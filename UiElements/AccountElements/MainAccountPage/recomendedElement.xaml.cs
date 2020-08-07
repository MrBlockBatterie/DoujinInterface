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

namespace Doujin_Interface.UiElements.AccountElements.MainAccountPage
{
    /// <summary>
    /// Interaktionslogik für showcaseElement.xaml
    /// </summary>
    public partial class RecomendedElement : UserControl
    {
        private bool initialized;
        public ApiHelper apiHelper;
        public RecomendedElement()
        {
            InitializeComponent();
        }

        public async Task ShowRecommendations()
        {
            if(initialized == false)
            {
                try
                {
                    var recommendationsModel = await apiHelper.GetRecommendedDoujin();
                    var recommendationByUser = recommendationsModel.Users;
                    foreach (var item in recommendationByUser)
                    {
                        TextBlock seperator = new TextBlock { 
                            Height = 40,
                            FontSize = 30,
                            Width = recommendations.ActualWidth-10,
                            Text = item.Username,
                            Foreground = Brushes.White
                        };
                        recommendations.Children.Add(seperator);
                        foreach (var doujin in item.Ids)
                        {
                            DoujinControl doujinControl = new DoujinControl(doujin);
                            recommendations.Children.Add(doujinControl);
                        }
                    }
                    initialized = true;

                }
                catch (Exception e)
                {

                    DoujinUtility.MainWindow.notificationPanellul.Children.Add(Notifications.Notifications.NotificationNoImg("An error occured", "", e.Message));
                }
            }
        }
    }
}
