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
using Doujin_Interface.Notifications.NotificationControlListControl;
using Sankaku_Interface;

namespace Doujin_Interface.Notifications
{
    public class Notifications
    {

        public static void NotificationControlForList(string header, string underline, string caption)
        {
            NotificationControlList notificationControlList = new NotificationControlList();
            notificationControlList.headlineText.Text = header;
            notificationControlList.underlineText.Text = underline;
            notificationControlList.captionText.Text = caption;
            notificationControlList.timeTextBox.Text = DateTime.Now.ToShortTimeString();
            DoujinUtility.MainWindow.NotificationList.Add(notificationControlList);

        }

        public static void CreateErrorNotification(Exception e)
        {
            DoujinUtility.MainWindow.notificationPanellul.Children.Add(NotificationNoImg("An error occured","",e.Message));
        }

        public static Control NotificationNoImg(string header, string underline, string caption)
        {
            NotificationControl notificationControl = new NotificationControl();
            //DoubleAnimation da = new DoubleAnimation();
            //da.From = 0;
            //da.To = 360;
            //da.Duration = new Duration(TimeSpan.FromSeconds(3));
            //da.RepeatBehavior = RepeatBehavior.Forever;
            //RotateTransform rt = new RotateTransform();
            //notificationControl.RenderTransform = rt;
            //rt.BeginAnimation(RotateTransform.AngleProperty, da);
            
            notificationControl.Width = 350;
            notificationControl.Height = 80;
            notificationControl.HorizontalAlignment = HorizontalAlignment.Right;
            notificationControl.VerticalAlignment = VerticalAlignment.Bottom;
            notificationControl.Margin = new Thickness(0, 0, 0, 10);
            notificationControl.header.Text = header;
            notificationControl.underline.Text = underline;
            notificationControl.caption.Text = caption;
            notificationControl.Name = "notificationControl";
            notificationControl.LayoutTransform = new RotateTransform(180);
            notificationControl.RegisterName(notificationControl.Name, notificationControl);
            GetThicc(notificationControl);
            GetSmoll(notificationControl);
            NotificationControlForList(header, underline, caption);
            notificationControl.boarder.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            return notificationControl;
        }

        public static NotificationControlPBar CreateNotificationWPBarNImg(string header, string caption, string pBarText, double pBarValue, int pBarMax)
        {
            NotificationControlPBar notificationControl = new NotificationControlPBar();

            notificationControl.Width = 350;
            notificationControl.Height = 80;
            notificationControl.HorizontalAlignment = HorizontalAlignment.Right;
            notificationControl.VerticalAlignment = VerticalAlignment.Bottom;
            notificationControl.Margin = new Thickness(0, 0, 0, 40);
            notificationControl.header.Text = header;
            notificationControl.pBarText.Text = pBarText;
            notificationControl.caption.Text = caption;
            notificationControl.pbar.Maximum = pBarMax;
            notificationControl.pbar.Value = pBarValue;
            notificationControl.Name = "notificationControl";
            notificationControl.RegisterName(notificationControl.Name, notificationControl);
            NotificationControlForList(header + " Finished", "", caption);

            GetThicc(notificationControl);
            return notificationControl;
        }
        public static void UpdateNotificationWBarNImg(NotificationControlPBar nbar, string pBarText, double pBarValue)
        {
            nbar.pBarText.Text = pBarText;
            nbar.pbar.Value = pBarValue;
            if (nbar.pbar.Value >= nbar.pbar.Maximum)
            {
                GetSmoll(nbar, 0);
            }
        }

        public static void GetSmoll(NotificationControlPBar notificationControl, int timeSpanSec = 5)
        {
            DoubleAnimation getSmoll = new DoubleAnimation();
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeSpanSec);
            getSmoll.From = notificationControl.Width;
            getSmoll.To = 0;
            getSmoll.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Storyboard.SetTargetName(getSmoll, notificationControl.Name);
            Storyboard.SetTargetProperty(getSmoll, new PropertyPath(Window.WidthProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(getSmoll);
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = timeSpan;
            dispatcherTimer.Start();

            void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                dispatcherTimer.IsEnabled = false;
                sb1.Begin(notificationControl);
            }
        }
        private static void GetSmoll(NotificationControl notificationControl, int timeSpanSec = 5)
        {
            DoubleAnimation getSmoll = new DoubleAnimation();
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeSpanSec);
            getSmoll.From = notificationControl.Width;
            getSmoll.To = 0;
            getSmoll.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Storyboard.SetTargetName(getSmoll, notificationControl.Name);
            Storyboard.SetTargetProperty(getSmoll, new PropertyPath(Window.WidthProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(getSmoll);
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = timeSpan;
            dispatcherTimer.Start();

            void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                dispatcherTimer.IsEnabled = false;
                sb1.Begin(notificationControl);
            }
        }

        private static void GetThicc(NotificationControlPBar notificationControl)
        {
            DoubleAnimation getThicc = new DoubleAnimation();
            getThicc.From = 0;
            getThicc.To = 350;
            getThicc.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Storyboard.SetTargetName(getThicc, notificationControl.Name);
            Storyboard.SetTargetProperty(getThicc, new PropertyPath(Window.WidthProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(getThicc);
            sb1.Begin(notificationControl);
        }
        private static void GetThicc(NotificationControl notificationControl)
        {
            DoubleAnimation getThicc = new DoubleAnimation();
            getThicc.From = 0;
            getThicc.To = 350;
            getThicc.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Storyboard.SetTargetName(getThicc, notificationControl.Name);
            Storyboard.SetTargetProperty(getThicc, new PropertyPath(Window.WidthProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(getThicc);
            sb1.Begin(notificationControl);
        }

        public static void AddNotifications()
        {
            foreach (NotificationControlList obj in DoujinUtility.MainWindow.NotificationList)
            {
                DoujinUtility.MainWindow.notificationsgrid.Children.Add(obj);
            }
        }
    }

}
