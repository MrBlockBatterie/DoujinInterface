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

namespace Doujin_Interface.uiElements.searchElement
{
   class SE
    {
        public static SearchElementLeftSide CreateSearchElement(WrapPanel picgrid, Grid gridIn, Window windowIn)
        {
            SearchElementLeftSide sElement = new SearchElementLeftSide( picgrid, gridIn, windowIn);
            sElement.Height = 1080;
            sElement.Width = 228;
            sElement.HorizontalAlignment = HorizontalAlignment.Left;
            sElement.VerticalAlignment = VerticalAlignment.Top;
            sElement.Name = "sElement";
            sElement.Margin = new Thickness(50, 0, 0, 0);
            sElement.artistSearch.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.charaSearch.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.generalSearch.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.parodySearch.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.tagsSearch.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.langugageBorder.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.sortByBorder.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.langAllButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.langDeButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.langEnButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.langJpButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.sortPopButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.sortDateButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.searchButton.BorderBrush = DoujinUtility.MainWindow.animatedBrush;
            sElement.searchButton.Background = DoujinUtility.MainWindow.animatedBrush;
            return sElement;
        }
    }
}
