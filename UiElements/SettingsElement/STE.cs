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

namespace Doujin_Interface.UiElements.SettingsElement
{
    class STE
    {
        public static Control CreateSettingsElement()
        {
            NewSettingsElement settingsElement = new NewSettingsElement();
            settingsElement.HorizontalAlignment = HorizontalAlignment.Center;
            settingsElement.VerticalAlignment = VerticalAlignment.Top;
            settingsElement.IsEnabled = false;
            return settingsElement;
        }
    }
}
