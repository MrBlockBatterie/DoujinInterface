using Sankaku_Interface;
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
using WinForms = System.Windows.Forms;

namespace Doujin_Interface.UiElements.SettingsElement
{
    /// <summary>
    /// Interaktionslogik für NewSettingsElement.xaml
    /// </summary>
    public partial class NewSettingsElement : UserControl
    {
        
        public NewSettingsElement()
        {
            InitializeComponent();
            doujinSavePath.Text = Properties.Settings.Default.Savepath;
            this.Visibility = Visibility.Hidden;
            this.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Savepath = doujinSavePath.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (WinForms.FolderBrowserDialog folder = new WinForms.FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == WinForms.DialogResult.OK)
                {
                    doujinSavePath.Text = folder.SelectedPath;
                }
            }
        }

        private void doujinSavePath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (WinForms.FolderBrowserDialog folder = new WinForms.FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result == WinForms.DialogResult.OK)
                {
                    doujinSavePath.Text = folder.SelectedPath;
                }
            }
        }

        private void doRGB_click(object sender, RoutedEventArgs e)
        {
            NameScope.SetNameScope(this, new NameScope());
            //SolidColorBrush animatedBrush = new SolidColorBrush();
            btnDoRGB.Background = DoujinUtility.MainWindow.animatedBrush;

            btntest.Background = DoujinUtility.MainWindow.animatedBrush;

            DoujinUtility.MainWindow.animatedBrush.Color = Color.FromArgb(255, 255, 0, 0);
            this.RegisterName("AnimatedBrush", DoujinUtility.MainWindow.animatedBrush);
            ColorAnimationUsingKeyFrames colorAnimation = new ColorAnimationUsingKeyFrames();
            colorAnimation.Duration = TimeSpan.FromSeconds(12);
            Color redColor = new Color();
            redColor = Color.FromArgb(255, 255, 0, 0);
            Color greenColor = new Color();
            greenColor = Color.FromArgb(255, 0, 255, 0);
            Color blueColor = new Color();
            blueColor = Color.FromArgb(255, 0, 0, 255);
            colorAnimation.KeyFrames.Add(
                new LinearColorKeyFrame(
                    greenColor, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4.0))) // KeyTime
                );
            colorAnimation.KeyFrames.Add(
                new LinearColorKeyFrame(
                    blueColor, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(8.0))) // KeyTime
                );
            colorAnimation.KeyFrames.Add(
                new LinearColorKeyFrame(
                    redColor, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(12.0))) // KeyTime
                );
           //colorAnimation.AutoReverse = true;
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(colorAnimation, "AnimatedBrush");
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard myStoryboard = new Storyboard();
            myStoryboard.Children.Add(colorAnimation);
            myStoryboard.Begin(this.btnDoRGB);

            /* btnDoRGB.Background = new SolidColorBrush(Colors.Red);
             ColorAnimation animation2;
             animation2 = new ColorAnimation();
             animation2.From = Color.FromArgb(100, 255, 0, 0);
             animation2.To = Color.FromArgb(100, 0, 255, 255);
             animation2.Duration = new Duration(TimeSpan.FromSeconds(1));
             animation2.AutoReverse = true;
             animation2.RepeatBehavior = RepeatBehavior.Forever;
             btnDoRGB.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
            */
        }

        private void test(object sender, RoutedEventArgs e)
        {

        }
    }
}
