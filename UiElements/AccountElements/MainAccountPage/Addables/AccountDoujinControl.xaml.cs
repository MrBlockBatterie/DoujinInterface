using Doujin_Interface.Database;
using Doujin_Interface.uiElements;
using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace Doujin_Interface
{
    /// <summary>
    /// Interaktionslogik für DoujinControl.xaml
    /// </summary>
    public partial class AccountDoujinControl : UserControl
    {
        private double prevSize;
        private double tagsPrevSize;
        private double gridPrevSize;
        private bool expanded = false;
        private bool allreadyRead = false;
        public AccountDoujinControl()
        {
            InitializeComponent();
            prevSize = this.Height;
            tagsPrevSize = doujinTags.Height;
            gridPrevSize = infoGrid.Height;
        }
        public AccountDoujinControl(int nhentaiId, string doujinrater, double rating, string comment)
        {
            InitializeComponent();
            prevSize = this.Height;
            tagsPrevSize = doujinTags.Height;
            gridPrevSize = infoGrid.Height;
            Doujin doujin = new Doujin(nhentaiId);
            doujinRecomender.Text = doujinrater;
            doujinRatingText.Text = $"Rating: {rating}/5";
            doujinComment.Text = comment;

            foreach (Database.DoujinSet.DoujinDataRow row in Database.DatabaseControler.favorites)
            {
                if (row.nHentaiID == doujin.nhentaiId)
                {

                    doujin = new Doujin(row.nHentaiID);
                    doujin.favorised = true;

                    Console.WriteLine("fav matched");
                    heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_fav.png"));


                }
            }


            var source = new BitmapImage(new Uri(doujin.coverUrl));
            img.Source = source;
            double ratio = source.Width / source.Height;
            img.ToolTip = doujin.name;
            img.Tag = doujin.nhentaiId;
            img.Width = 160;
            img.Height = 230;
            img.Margin = new Thickness(5, 3, 5, 3);
            img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
            img.MouseRightButtonDown += Img_MouseRightButtonDown;
            heart.MouseLeftButtonDown += Heart_MouseLeftButtonDown;
            Thickness margin = Margin;
            margin.Right = 10;
            margin.Bottom = 10;
            Margin = margin;
            doujinName.Text = doujin.name;
            doujinCreator.Text = doujin.ArtistsConcat();
            doujinTags.Text = doujin.TagsConcat();
            var testRow = DatabaseControler.doujinCache.NewRow();
            testRow[0] = nhentaiId;
            testRow[1] = true;
            
            bool contains = DatabaseControler.doujinCache.AsEnumerable().Any(row => nhentaiId == row.Field<Int32>("hentaiID"));
            if (contains)
            {
                throw new Exception(nhentaiId.ToString());
            }
        }
        public AccountDoujinControl(Doujin doujin, string doujinrater, double rating, string comment)
        {
            InitializeComponent();
            prevSize = this.Height;
            tagsPrevSize = doujinTags.Height;
            gridPrevSize = infoGrid.Height;
            doujinRecomender.Text = doujinrater;
            doujinRatingText.Text = $"Rating: {rating}/5";
            doujinComment.Text = comment;



            foreach (Database.DoujinSet.DoujinDataRow row in Database.DatabaseControler.favorites)
            {
                if (row.nHentaiID == doujin.nhentaiId)
                {

                    //doujin = new Doujin(row.nHentaiID);
                    doujin.favorised = true;

                    Console.WriteLine("fav matched");
                    heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_fav.png"));


                }
            }


            var source = new BitmapImage(new Uri(doujin.coverUrl));
            img.Source = source;
            double ratio = source.Width / source.Height;
            img.ToolTip = doujin.name;
            img.Tag = (int)doujin.nhentaiId;
            img.Width = 160;
            img.Height = 230;
            img.Margin = new Thickness(5, 3, 5, 3);
            img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
            img.MouseRightButtonDown += Img_MouseRightButtonDown;
            heart.MouseLeftButtonDown += Heart_MouseLeftButtonDown;
            Thickness margin = Margin;
            margin.Right = 10;
            margin.Bottom = 10;
            Margin = margin;
            doujinName.Text = doujin.name;
            doujinCreator.Text = doujin.ArtistsConcat();
            doujinTags.Text = string.Join(", ", doujin.tags);
            Tag = doujin.nhentaiId.ToString();
           

            bool contains = DatabaseControler.doujinCache.AsEnumerable().Any(row => doujin.nhentaiId == row.Field<Int32>("hentaiID"));
            if (contains)
            {
                img.Opacity = 0.3;
            }
        }



            private void Heart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image obj = (Image)sender;
            Grid grid = (Grid)obj.Parent;
            DoujinControl control = (DoujinControl)grid.Parent;
            int nhId = (int)control.img.Tag;
            if (DoujinUtility.CheckFavorised(new Doujin(nhId)))
            {
                DatabaseControler.favorites.FindBynHentaiID(nhId).favorite = false;
                //DatabaseControler.mainDataTable.FindBynHentaiID(nhId).favorite = false;
                control.heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_nofav.png"));
                DatabaseControler.favorites.RemoveDoujinDataRow(DatabaseControler.favorites.FindBynHentaiID(nhId));

                DatabaseControler.favorites.WriteXml(DatabaseControler.favDataPath);
            }
            else
            {
                Doujin doujin = new Doujin(nhId);
                //DatabaseControler.mainDataTable.FindBynHentaiID(nhId).favorite = true;
                control.heart.Source = new BitmapImage(new Uri("pack://application:,,,/UiElements/heart_fav.png"));
                DoujinUtility.AddDoujinDataRow(doujin, DatabaseControler.favorites);
                DatabaseControler.favorites.WriteXml(DatabaseControler.favDataPath);
                //.Add();

                var notify = Notifications.Notifications.NotificationNoImg(DoujinUtility.MainWindow, doujin.name, "", "The doujin got favorised and you can acess it at your favourite page");

                DoujinUtility.MainWindow.notificationPanellul.Children.Add(notify);
            }
        }

        private void Img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            DetailsWindow detailsWindow = new DetailsWindow(new Doujin((int)img.Tag));
            detailsWindow.Show();
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            using (var doujin = new Doujin((int)img.Tag))
            {
                var viewer = new DoujinViewer(doujin);
                viewer.Show();
            }
        }

        public async Task DownloadThread(int nHentaiId, WinForms.FolderBrowserDialog folder)
        {
            Doujin doujin = new Doujin(nHentaiId);
            string newName = doujin.name.Replace("/", "").Replace("?", "").Replace("%", "").Replace("*", "").Replace(":", "").Replace("|", "").Replace(".", "").Replace("<", "").Replace(">", "").Replace('"'.ToString(), "");

            if (!Directory.Exists($@"{folder.SelectedPath}/{newName}"))
            {
                using (WebClient client = new WebClient())
                {
                    int i = 1;
                    var notify = Notifications.Notifications.CreateNotificationWPBarNImg("Download", $"{doujin.name} is being downloaded", $"{i} of {doujin.pageCount}", 1, doujin.pageCount);
                    notify.LayoutTransform = new RotateTransform(180);
                    var window = DoujinUtility.MainWindow;
                    window.notificationPanellul.Children.Add(notify);

                    foreach (string ext in DoujinUtility.GetPage(doujin))
                    {
                        Console.WriteLine($"https://i.nhentai.net/galleries/{doujin.mediaId}/{i}{ext}, {folder.SelectedPath}/{newName}/{i}{ext}");
                        Directory.CreateDirectory($@"{folder.SelectedPath}/{newName}");
                        await Task.Run(() => client.DownloadFile(new Uri("https://i.nhentai.net/galleries/" + doujin.mediaId + "/" + i + ext), $@"{folder.SelectedPath}/{newName}/{i}{ext}"));
                        notify.UpdateNotificationWBarNImg($"{i} of {doujin.pageCount}", i);
                        i++;
                    }

                    // OR 
                }
            }
            else
            {
                Console.WriteLine("DAS GIBTS DOCH SCHON!!!!11!!elf");
            }


        }
        private async void StartTask(int nHentaiId, WinForms.FolderBrowserDialog folder)
        {
            await DownloadThread(nHentaiId, folder);
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //f
            var fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            var blurIn = new DoubleAnimation(0.4, TimeSpan.FromSeconds(0.2));
            heart.BeginAnimation(Image.OpacityProperty,fadeIn);
            download.BeginAnimation(Image.OpacityProperty, fadeIn);
            blur.BeginAnimation(Image.OpacityProperty, blurIn);
        }
        

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            var fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            heart.BeginAnimation(Image.OpacityProperty, fadeOut);
            download.BeginAnimation(Image.OpacityProperty, fadeOut);
            blur.BeginAnimation(Image.OpacityProperty, fadeOut);
        }

        private void download_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var nHentaiId = (int)img.Tag;
            using (WinForms.FolderBrowserDialog folder = new WinForms.FolderBrowserDialog())
            {
                folder.SelectedPath = Properties.Settings.Default.Savepath;
                var result = folder.ShowDialog();
                if (result == WinForms.DialogResult.OK)
                {
                    
                    StartTask(nHentaiId, folder);
                    
                    //var thread = new Thread(() => DownloadThread(nHentaiId, folder));
                    
                }
            }     
        }

        private void infoGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void infoGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (expanded)
            {
                var fadeOut = new DoubleAnimation(prevSize, TimeSpan.FromSeconds(0.2));
                var tagsFadeOut = new DoubleAnimation(tagsPrevSize, TimeSpan.FromSeconds(0.2));
                var gridFadeOut = new DoubleAnimation(gridPrevSize, TimeSpan.FromSeconds(0.2));
                infoGrid.BeginAnimation(Grid.HeightProperty, gridFadeOut);
                doujinTags.BeginAnimation(UserControl.HeightProperty, tagsFadeOut);
                this.BeginAnimation(UserControl.HeightProperty, fadeOut);
                expanded = false;
            }
            
        }

        private void infoGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var fadeIn = new DoubleAnimation(prevSize + 100, TimeSpan.FromSeconds(0.2));
            var tagsFadeIn = new DoubleAnimation(tagsPrevSize + 100, TimeSpan.FromSeconds(0.2));
            var gridFadeIn = new DoubleAnimation(gridPrevSize + 100, TimeSpan.FromSeconds(0.2));
            var fadeOut = new DoubleAnimation(prevSize, TimeSpan.FromSeconds(0.2));
            var tagsFadeOut = new DoubleAnimation(tagsPrevSize, TimeSpan.FromSeconds(0.2));
            var gridFadeOut = new DoubleAnimation(gridPrevSize, TimeSpan.FromSeconds(0.2));
            if (expanded)
            {
                infoGrid.BeginAnimation(Grid.HeightProperty, gridFadeOut);
                doujinTags.BeginAnimation(UserControl.HeightProperty, tagsFadeOut);
                this.BeginAnimation(UserControl.HeightProperty, fadeOut);
                expanded = false;
            }
            else
            {
                infoGrid.BeginAnimation(Grid.HeightProperty, gridFadeIn);
                doujinTags.BeginAnimation(TextBlock.HeightProperty, tagsFadeIn);
                this.BeginAnimation(UserControl.HeightProperty, fadeIn);
                expanded = true;
            }
        }
    }
}
