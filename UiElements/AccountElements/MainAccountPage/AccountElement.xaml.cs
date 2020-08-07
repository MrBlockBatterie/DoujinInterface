using Doujin_Interface.Connection;
using Doujin_Interface.Connection.Models;
using Newtonsoft.Json;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doujin_Interface.UiElements.AccountElements.MainAccountPage
{
    /// <summary>
    /// Interaktionslogik für AccountElement.xaml
    /// </summary>
    public partial class AccountElement : UserControl
    {
        
        Grid currentWindowButtonGrid;
        public AuthenticatedUser user;
        private bool _loggedIn = false;
        public bool loggedIn { 
            get
            {
                return _loggedIn;
            }
            set
            {
                _loggedIn = value;
                DoujinUtility.loggedIn = value;
                loggedInAs.Text = $"logged in as {user.UserName}";
            }
        }
        private ApiHelper _apiHelper;
        public ApiHelper apiHelper
        {
            get
            {
                if (_apiHelper != null)
                {
                    return _apiHelper;
                }
                else
                {
                    _apiHelper = new ApiHelper {user = this.user};
                    return _apiHelper;
                }
            }
            set
            {
                _apiHelper = value;
            }
        }
        public AccountElement()
        {
            InitializeComponent();
            List<Grid> tabSelectorGrids = new List<Grid>();
            Grid prefGrid = new Grid();
            prefGrid.Margin = new Thickness(0, 0, 0, 0);
            currentWindowButtonGrid = showcaseGrid;
            Dictionary<Grid, ProgressBar> refDict = new Dictionary<Grid, ProgressBar>()
            {
                {this.showcaseGrid, this.showcaseBar },
                {this.recommendedGrid, this.recommendedBar },
                {this.friendsGrid, this.friendsBar }
            };

            foreach (Grid obj in this.tabSelectorGrid.Children.OfType<Grid>())
            {
                DoujinUtility.FindChild<ProgressBar>(obj, null).Foreground = DoujinUtility.MainWindow.animatedBrush;
                tabSelectorGrids.Add(obj);

            }
            foreach (Grid grid in tabSelectorGrids)
            {
                grid.MouseEnter += delegate (object sender, MouseEventArgs e) { Example_EnterAnimationEvent(sender, e, this, grid, refDict[grid]); };
                grid.MouseLeave += delegate (object sender, MouseEventArgs e) { Example_LeaveAnimationEvent(sender, e, this, grid, refDict[grid]); };
                
                Console.WriteLine(prefGrid.Margin.Top);
                prefGrid = grid;
            }

            void Example_EnterAnimationEvent(object sender, MouseEventArgs e, AccountElement navBar, Grid grid, ProgressBar progressBar)
            {

            }

            void Example_LeaveAnimationEvent(object sender, MouseEventArgs e, AccountElement navBar, Grid grid, ProgressBar progressBar)
            {

            }
        }
        private void control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(tabSelectorGrid);
            int i = 1;
            foreach (Grid grid in this.tabSelectorGrid.Children.OfType<Grid>())
            {
                grid.Margin = new Thickness(grid.Margin.Left, ((tabSelectorGrid.RenderSize.Height / (childCount + 8) * (i + 4)) - ((showcaseGrid.Height / 2))), grid.Margin.Right, grid.Margin.Bottom);
                i++;
                //(((tabSelectorGrid.RenderSize.Height / (childCount + 1)) * (i + 1)) - (showcaseGrid.Height / 2))
            }
            moveActiveIndicator();
        }

        private void showcaseGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            accountTabs.SelectedIndex = 0;
            currentWindowButtonGrid = showcaseGrid;
            moveActiveIndicator();
        }

        private async void recommendedGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            accountTabs.SelectedIndex = 1;
            currentWindowButtonGrid = recommendedGrid;
            recommendedTab.apiHelper = apiHelper;
            await recommendedTab.ShowRecommendations();
            moveActiveIndicator();
        }

        private async void friendsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            accountTabs.SelectedIndex = 2;
            currentWindowButtonGrid = friendsGrid;
            friendsTab.apiHelper = apiHelper;
            await friendsTab.ShowFriends();
            moveActiveIndicator();
        }

        private void moveActiveIndicator()
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            ThicknessAnimation move = new ThicknessAnimation(new Thickness(activeIndicator.Margin.Left, activeIndicator.Margin.Top, 0, 0), new Thickness(activeIndicator.Margin.Left, currentWindowButtonGrid.Margin.Top, 0, 0), duration);
            activeIndicator.BeginAnimation(Border.MarginProperty, move);
        }

    }
}
