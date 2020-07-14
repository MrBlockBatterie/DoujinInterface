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
            List<TextBlock> blocks = new List<TextBlock>();
            Dictionary<TextBlock, ProgressBar> refDict = new Dictionary<TextBlock, ProgressBar>() 
            {
                {navBar.homeText, navBar.homeBar },
                {navBar.favText, navBar.favBar },
                {navBar.gamesText, navBar.gamesBar },
                {navBar.notificationsText, navBar.notificationsBar },
                {navBar.accountText, navBar.accountBar },
                {navBar.settingsText, navBar.settingsBar },
                {navBar.updateFeedText, navBar.updateFeedBar }
            };
            //Bug fix deswegen 2 mal
            navBar.activeWindow.Background = DoujinUtility.MainWindow.animatedBrush;
            navBar.activeWindow.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            navBar.Height = 2000;
            navBar.HorizontalAlignment = HorizontalAlignment.Left;
            navBar.VerticalAlignment = VerticalAlignment.Top;
            navBar.Name = "navBar";
            navBar.RegisterName(navBar.Name, navBar);
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
                foreach (object obj in navBar.rootGrid.Children)
            {
                if (obj.GetType() == typeof(TextBlock))
                {
                    blocks.Add((TextBlock)obj);
                }
            }
            foreach (TextBlock txt in blocks)
            {
                    navBar.Width = 52;
                    txt.MouseEnter += delegate (object sender, MouseEventArgs e) { NavBarText_MouseEnter(sender, e, navBar, txt, refDict[txt]); };
                    txt.MouseLeave += delegate (object sender, MouseEventArgs e) { NavBarText_MouseLeave(sender, e, navBar, txt, refDict[txt]); };
            }
            return navBar;
        }

        public static void MoveActivIndicator(NavBarLeftSide navBar, TextBlock textblock)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            ThicknessAnimation move = new ThicknessAnimation(new Thickness(navBar.activeWindow.Margin.Left, navBar.activeWindow.Margin.Top, 0, 0), new Thickness(navBar.activeWindow.Margin.Left, textblock.Margin.Top, 0, 0), duration);
            navBar.activeWindow.BeginAnimation(Border.MarginProperty, move);
        }
        private static void NavBarText_MouseLeave(object sender, MouseEventArgs e, NavBarLeftSide navBar, TextBlock textBlock, ProgressBar progressBar)
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(300));
            DoubleAnimation doubleanimation = new DoubleAnimation(progressBar.Value - 100, duration);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

            
        }

        private static void NavBarText_MouseEnter(object sender, MouseEventArgs e, NavBarLeftSide navBar, TextBlock textBox, ProgressBar progressBar)
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
