using Sankaku_Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;
using static Doujin_Interface.Database.DoujinSet;

namespace Doujin_Interface
{
    /// <summary>
    /// Interaktionslogik für DoujinViewer.xaml
    /// </summary>
    public partial class DoujinViewer : Window
    {
        private List<Image> images = new List<Image>();
        public Doujin gDoujin = new Doujin();
        private int doujinPage = 1;
        private List<BitmapImage> cache = new List<BitmapImage>();
        private double ratio;
        private double readP;
        private bool streched = true;
        public DoujinViewer(Doujin doujin)
        {
            InitializeComponent();
            viewport.BeginInit();
            gDoujin = doujin;
            Load(doujin);
            viewport.EndInit();
            control.parent = this;
            //gDoujin.pageExt = DoujinUtility.GetPage(gDoujin);
        }
        public DoujinViewer(int mediaid, int nhentaiid, int pages, string coverurl, string ext)
        {
            InitializeComponent();
            viewport.BeginInit();
            gDoujin.mediaId = mediaid;
            gDoujin.nhentaiId = nhentaiid;
            gDoujin.pageCount = pages;
            gDoujin.coverUrl = coverurl;
            gDoujin.coverExt = ext;
            gDoujin.thumbnailExt = ext;
            Load(gDoujin);
            viewport.EndInit();
            gDoujin.pageExt = DoujinUtility.GetPage(nhentaiid);
        }
        //viewport
        public void Load(Doujin doujin)
        {

            for (int i = 1; i < doujin.pageCount; i++)
            {
                //WebClient webClient = new WebClient();
                //webClient.DownloadFile(new Uri("https://i.nhentai.net/galleries/" + gDoujin.mediaId + "/" + i +gDoujin.pageExt.ElementAt(i-1)), i + gDoujin.pageExt.ElementAt(i-1));
                if (doujin.thumbnailExt == null)
                {
                    
                    Console.WriteLine(i);
                    Image img = new Image();
                    

                    img.Source = new BitmapImage(new Uri("https://t.nhentai.net/galleries/" + doujin.mediaId + "/" + i + "t" + DoujinUtility.GetExtension(doujin.mediaId, false, doujin)));
                    
                    img.Width = 162;
                    img.Height = 231;
                    img.Margin = new Thickness(5, 3, 5, 3);
                    
                    viewport.Children.Add(img);
                    images.Add(img);
                }
                else
                {
                    Console.WriteLine(i);
                    Image img = new Image();
                    viewport.Children.Add(img);


                    img.Source = new BitmapImage(new Uri("https://t.nhentai.net/galleries/" + doujin.mediaId + "/" + i + "t" + doujin.pageExt.ElementAt(i-1)));
                    
                    img.Width = 250;
                    img.Height = 347;
                    img.Margin = new Thickness(5, 3, 5, 3);
                    img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
                    images.Add(img);
                    Console.WriteLine(img.Source);
                }
                Console.WriteLine("pages=" + doujin.pageCount + ", entires=" + images.Count);
                pageNumberText.Text = $"Pages: {gDoujin.pageCount}";
            }
            
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewport.Visibility = Visibility.Hidden;
            scrollview.Visibility = Visibility.Hidden;
            page.Visibility = Visibility.Visible;
            pagescroll.Visibility = Visibility.Visible;
            LoadPage(images.IndexOf((Image)sender)+1);
            doujinPage = images.IndexOf((Image)sender)+1;
            UpdatePageNumber();
        }

        private void LoadPage(int p)
        {
            if (cache.Count == 0)
            {
                for (int i = 1; i <= gDoujin.pageCount; i++)
                {
                    var bmp = new BitmapImage(new Uri("https://i.nhentai.net/galleries/" + gDoujin.mediaId + "/" + i + gDoujin.pageExt.ElementAt(i - 1)));
                    cache.Add(bmp);
                }
                
                page.Source = cache.ElementAt(p - 1);
                ratio = page.Width / page.Height;
                //Console.WriteLine($"Doujin Viewer: Ratio is {ratio}, {page.Source.Width}/{page.Source.Height}");
            }
            else
            {
                page.Source = cache.ElementAt(p - 1);
            }

            readP = (double)p / (double)gDoujin.pageCount;
            Console.WriteLine($"Doujin Viewer: p={p}, page count={gDoujin.pageCount}, readP={readP}");
            if (readP >= 0.8 && !(Database.DatabaseControler.doujinCache.AsEnumerable().Any(row => gDoujin.nhentaiId == row.Field<Int32>("hentaiID"))))
            {
                Database.DatabaseControler.doujinCache.AddDoujinCacheRow(gDoujin.nhentaiId, true);
                Database.DatabaseControler.doujinCache.WriteXml("cache.xml");

                
                if (DoujinUtility.FindChild<DoujinControl>(DoujinUtility.MainWindow.picgrid, gDoujin.nhentaiId.ToString()) != null)
                {
                    DoujinUtility.FindChild<DoujinControl>(DoujinUtility.MainWindow.picgrid, gDoujin.nhentaiId.ToString()).img.Opacity = 0.3;
                }
            }
        
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void scrollview_KeyDown(object sender, KeyEventArgs e)
        {
            if (page.IsVisible)
            {
                if (e.Key == Key.Right || e.Key == Key.D)
                {
                    if (doujinPage < gDoujin.pageCount)
                    {
                        pagescroll.ScrollToHome();
                        doujinPage++;
                        LoadPage(doujinPage);
                        UpdatePageNumber();
                    }

                }

                if (e.Key == Key.Left || e.Key == Key.A)
                {
                    if (doujinPage > 1)
                    {
                        pagescroll.ScrollToHome();
                        doujinPage--;
                        LoadPage(doujinPage);
                        UpdatePageNumber();
                    }

                }
                if (e.Key == Key.Up || e.Key == Key.W)
                {
                    pagescroll.ScrollToVerticalOffset(pagescroll.VerticalOffset - 50);
                    /*
                    viewport.Visibility = Visibility.Visible;
                    scrollview.Visibility = Visibility.Visible;
                    page.Visibility = Visibility.Hidden;
                    pagescroll.Visibility = Visibility.Hidden;
                    */
                }
                if (e.Key == Key.E && streched)
                {
                    double stretch = (viewport.ActualWidth / page.ActualWidth)/2;
                    
                    Console.WriteLine($"Doujin Viewer: factor:{stretch}, {this.Width}/{page.ActualWidth}");
                    page.Height = scrollview.RenderSize.Height - control.Height;
                    page.Stretch = Stretch.Uniform;
                    var margin = page.Margin;
                    margin.Top = control.Height;
                    page.Margin = margin;
                    pagescroll.IsEnabled = false;
                    streched = false;


                }
                if (e.Key == Key.Q && !streched)
                {
                    //Console.WriteLine($"Doujin Viewer: Ratio is {ratio}");
                    page.Width = scrollview.Width;
                    page.Height = page.Height * ratio;
                    page.Stretch = Stretch.Uniform;
                    pagescroll.IsEnabled = true;
                    streched = true;
                }
                if (e.Key == Key.Down)
                {
                    pagescroll.ScrollToVerticalOffset(pagescroll.VerticalOffset + 50);
                }
                
            }
            if (e.Key == Key.F)
            {
                Boolean flag = true;

                foreach (var test in MainWindow.favs)
                {
                    if (test.nHentaiID == gDoujin.nhentaiId)
                    {
                        MainWindow.favs.RemoveDoujinDataRow(test);
                        flag = false;
                        Console.WriteLine("unfaved");
                        break;
                    }
                }
                if (flag)
                {
                    MainWindow.dt[gDoujin.id - 1].favorite = true;
                    var newRow = MainWindow.dt[gDoujin.id - 1];
                    MainWindow.favs.AddDoujinDataRow(MainWindow.favs.Count, newRow.nHentaiID, newRow.mediaID, newRow.name, newRow.fullName, newRow.artist, newRow.character, newRow.parody, newRow.group, newRow.tags, newRow.language, true, newRow.pages, newRow.coverUrl, newRow.extension);
                    Console.WriteLine("faved");
                }

                MainWindow.favs.WriteXml(Database.DatabaseControler.favDataPath);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        public void Strech()
        {
            if (!streched)
            {
                page.Width = scrollview.Width;
                page.Height = page.Height * ratio;
                page.Stretch = Stretch.Uniform;
                pagescroll.IsEnabled = true;
                streched = true;
            }
            else
            {
                double stretch = (viewport.ActualWidth / page.ActualWidth) / 2;

                Console.WriteLine($"Doujin Viewer: factor:{stretch}, {this.Width}/{page.ActualWidth}");
                page.Height = scrollview.RenderSize.Height - control.Height;
                page.Stretch = Stretch.Uniform;
                var margin = page.Margin;
                margin.Top = control.Height;
                page.Margin = margin;
                pagescroll.IsEnabled = false;
                streched = false;
            }
        }
        public void PageNext()
        {
            if (doujinPage < gDoujin.pageCount)
            {
                pagescroll.ScrollToHome();
                doujinPage++;
                LoadPage(doujinPage);
                UpdatePageNumber();
            }
        }
        public void PagePrev()
        {
            if (doujinPage > 1)
            {
                pagescroll.ScrollToHome();
                doujinPage--;
                LoadPage(doujinPage);
                UpdatePageNumber();
            }
        }
        public void Home()
        {
            viewport.Visibility = Visibility.Visible;
            scrollview.Visibility = Visibility.Visible;
            page.Visibility = Visibility.Hidden;
            pagescroll.Visibility = Visibility.Hidden;
            pageNumberText.Text = $"Pages: {gDoujin.pageCount}";
        }
        public void Fav()
        {

        }
        public void UpdatePageNumber()
        {
            pageNumberText.Text = $"Page: {doujinPage + 1}/{gDoujin.pageCount}";
            pageNumberText.Opacity = 100;
            PageNumberFadeOut();
        }
        public void PageNumberFadeOut()
        {
            RegisterName("FadeOutName", pageNumberBackground);
            DoubleAnimationUsingKeyFrames fadeOutAnimation = new DoubleAnimationUsingKeyFrames();
            fadeOutAnimation.Duration = TimeSpan.FromSeconds(4);
            fadeOutAnimation.KeyFrames.Add(
                new SplineDoubleKeyFrame(
                    100,
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)))
                );
            fadeOutAnimation.KeyFrames.Add(
                new SplineDoubleKeyFrame(
                    0,
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4)),
                    new KeySpline(0.25,0.5,0.75,1.0))
                );
            Storyboard.SetTargetName(fadeOutAnimation, "FadeOutName");
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));
            Storyboard fadeOutStoryboard = new Storyboard();
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            fadeOutStoryboard.Begin(pageNumberBackground);
        }
        
    }
}



