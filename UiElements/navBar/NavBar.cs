using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using Sankaku_Interface;

namespace Doujin_Interface.uiElements.navBar
{
    class NavBar
    {
        public static bool alwaysMaxed = false;

        public static NavBarLeftSide CreateNavBar(MainWindow window)
        {
            NavBarLeftSide navBar = new NavBarLeftSide(window);
            List<Grid> blocks = new List<Grid>();
            Grid prefGrid = new Grid();
            Dictionary<Grid, ProgressBar> refDict = new Dictionary<Grid, ProgressBar>()
            {
                {navBar.dashboardGrid, navBar.dashboardBar },
                {navBar.homeGrid, navBar.homeBar },
                {navBar.favGrid, navBar.favBar },
                {navBar.hgamesGrid, navBar.gamesBar },
                {navBar.notificationsGrid, navBar.notificationsBar },
                {navBar.accountGrid, navBar.accountBar },
                {navBar.settingsGrid, navBar.settingsBar },
                {navBar.updateFeedGrid, navBar.updateFeedBar }
            };
            //Bug fix deswegen 2 mal
            navBar.activeWindow.Background = DoujinUtility.MainWindow.animatedBrush;
            navBar.activeWindow.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            navBar.Height = 2000;
            navBar.HorizontalAlignment = HorizontalAlignment.Left;
            navBar.VerticalAlignment = VerticalAlignment.Top;
            navBar.Name = "navBar";
            navBar.RegisterName(navBar.Name, navBar);
            prefGrid.Margin = new Thickness(0, -54, 0, 0);
            if (alwaysMaxed == false)
            {
                navBar.Width = 52;
                navBar.MouseEnter += delegate (object sender, MouseEventArgs e) { NavBar_MouseEnter(sender, e, navBar); };
                navBar.MouseLeave += delegate (object sender, MouseEventArgs e) { NavBar_MouseLeave(sender, e, navBar); };
            }
            else
            {
                navBar.Width = 280;
            }
            foreach (Grid obj in navBar.rootGrid.Children.OfType<Grid>())
            {
                DoujinUtility.FindChild<ProgressBar>(obj, null).Foreground = DoujinUtility.MainWindow.animatedBrush;


                blocks.Add(obj);

            }
            foreach (Grid grid in blocks)
            {
                navBar.Width = 52;
                grid.MouseEnter += delegate (object sender, MouseEventArgs e) { NavBarText_MouseEnter(sender, e, navBar, grid, refDict[grid]); };
                grid.MouseLeave += delegate (object sender, MouseEventArgs e) { NavBarText_MouseLeave(sender, e, navBar, grid, refDict[grid]); };
                grid.Margin = new Thickness(grid.Margin.Left, (prefGrid.Margin.Top + grid.Height + 11), grid.Margin.Right, grid.Margin.Bottom);
                Console.WriteLine(prefGrid.Margin.Top);
                prefGrid = grid;
            }
            return navBar;
        }

        public static void MoveActivIndicator(NavBarLeftSide navBar, Grid grid)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            ThicknessAnimation move = new ThicknessAnimation(new Thickness(navBar.activeWindow.Margin.Left, navBar.activeWindow.Margin.Top, 0, 0), new Thickness(navBar.activeWindow.Margin.Left, grid.Margin.Top + 11, 0, 0), duration);
            navBar.activeWindow.BeginAnimation(Border.MarginProperty, move);
        }
        private static void NavBarText_MouseLeave(object sender, MouseEventArgs e, NavBarLeftSide navBar, Grid grid, ProgressBar progressBar)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(300));
            DoubleAnimation doubleanimation = new DoubleAnimation(progressBar.Value - 100, duration);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);


        }

        private static void NavBarText_MouseEnter(object sender, MouseEventArgs e, NavBarLeftSide navBar, Grid grid, ProgressBar progressBar)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(300));
            DoubleAnimation doubleanimation = new DoubleAnimation(progressBar.Value + 100, duration);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }

        private static void NavBar_MouseLeave(object sender, MouseEventArgs e, NavBarLeftSide navBar)
        {
            if (alwaysMaxed == false)
            {
                //throw new System.ArgumentException("fuck");
                DoubleAnimation getSmoll = new DoubleAnimation();
                getSmoll.From = navBar.Width;
                getSmoll.To = 52;
                getSmoll.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                Storyboard.SetTargetName(getSmoll, navBar.Name);
                Storyboard.SetTargetProperty(getSmoll, new PropertyPath(Window.WidthProperty));
                Storyboard sb1 = new Storyboard();
                sb1.Children.Add(getSmoll);
                sb1.Begin(navBar);

            }
            else
            {
                navBar.Width = 280;
            }

        }

        private static void NavBar_MouseEnter(object sender, MouseEventArgs e, NavBarLeftSide navBar)
        {
            if (alwaysMaxed == false)
            {
                navBar.Width = 52;
                DoubleAnimation getThicc = new DoubleAnimation();
                getThicc.From = 52;
                getThicc.To = 280;
                getThicc.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                Storyboard.SetTargetName(getThicc, navBar.Name);
                Storyboard.SetTargetProperty(getThicc, new PropertyPath(Window.WidthProperty));
                Storyboard sb1 = new Storyboard();
                sb1.Children.Add(getThicc);
                sb1.Begin(navBar);
            }
            else
            {
                navBar.Width = 280;
            }

        }
    }
}
