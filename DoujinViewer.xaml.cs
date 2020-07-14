﻿using Sankaku_Interface;
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
        public DoujinViewer(Doujin doujin)
        {
            InitializeComponent();
            viewport.BeginInit();
            gDoujin = doujin;
            Load(doujin);
            viewport.EndInit();
            
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
                ratio = page.Source.Width / page.Source.Height;
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
                        pagescroll.ScrollToHorizontalOffset(1);
                        doujinPage++;
                        LoadPage(doujinPage);
                        
                    }

                }

                if (e.Key == Key.Left || e.Key == Key.A)
                {
                    if (doujinPage > 1)
                    {
                        pagescroll.ScrollToHorizontalOffset(1);
                        doujinPage--;
                        LoadPage(doujinPage);
                        
                    }

                }
                if (e.Key == Key.Up || e.Key == Key.W)
                {
                    viewport.Visibility = Visibility.Visible;
                    scrollview.Visibility = Visibility.Visible;
                    page.Visibility = Visibility.Hidden;
                    pagescroll.Visibility = Visibility.Hidden;
                }
                if (e.Key == Key.E)
                {
                    double stretch = (this.ActualWidth / page.ActualWidth)/2;
                    Console.WriteLine($"Doujin Viewer: factor:{stretch}, {this.Width}/{page.ActualWidth}");
                    page.Height = pagescroll.ActualHeight;
                    page.Stretch = Stretch.Uniform;
                    var margin = page.Margin;
                    margin.Top = 0;
                    page.Margin = margin;
                    pagescroll.IsEnabled = false;


                }
                if (e.Key == Key.Q)
                {
                    page.Width = scrollview.ActualWidth;
                    page.Height = page.Height * ratio;
                    page.Stretch = Stretch.Uniform;
                    pagescroll.IsEnabled = true;
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

                MainWindow.favs.WriteXml("favs.xml");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        
    }
}



