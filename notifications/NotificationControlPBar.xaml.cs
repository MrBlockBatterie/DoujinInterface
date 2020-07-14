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

namespace Doujin_Interface.Notifications
{
    /// <summary>
    /// Interaktionslogik für notificationControl.xaml
    /// </summary>
    public partial class NotificationControlPBar : UserControl
    {

        public NotificationControlPBar()
        {
         /*   var myPanel = new StackPanel();
            myPanel.Margin = new Thickness(10);

            var myRectangle = new Rectangle();
            myRectangle.Name = "myRectangle";
            this.RegisterName(myRectangle.Name, myRectangle);
            myRectangle.Width = 100;
            myRectangle.Height = 100;
            myRectangle.Fill = Brushes.Blue;
            myPanel.Children.Add(myRectangle);
       this.Content = myPanel;
            var myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            sb.Children.Add(myDoubleAnimation);
      */  //    Storyboard.SetTargetName(myDoubleAnimation, myRectangle.Name);

            InitializeComponent();
        }
        public void UpdateNotificationWBarNImg(string pBarText, double pBarValue)
        {
            this.pBarText.Text = pBarText;
            this.pbar.Value = pBarValue;
            if (this.pbar.Value >= this.pbar.Maximum)
            {
                Notifications.GetSmoll(this, 0);
            }
        }
    }
}
