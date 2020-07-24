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
    /// Interaktionslogik für AccountElement.xaml
    /// </summary>
    public partial class AccountElement : UserControl
    {
        public AccountElement()
        {
            InitializeComponent();
            List<Grid> tabSelectorGrids = new List<Grid>();
            Grid prefGrid = new Grid();
            prefGrid.Margin = new Thickness(0, 0, 0, 0);
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
        }

        private void showcaseGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void recommendedGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void friendsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
