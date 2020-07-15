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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doujin_Interface.UiElements.DoujinViewer
{
    /// <summary>
    /// Interaktionslogik für ViewerControl.xaml
    /// </summary>
    public partial class ViewerControl : UserControl
    {
        public Doujin_Interface.DoujinViewer parent;
        public ViewerControl()
        {
            InitializeComponent();
            Init();
        }

        private void ButtonInit()
        {
            var childCount = VisualTreeHelper.GetChildrenCount(buttonGrid);
            for (int i = 0; i < childCount; i++)
            {
                Image btn = (Image)VisualTreeHelper.GetChild(buttonGrid, i);
                var margin = btn.Margin;
                double marginL = ((buttonGrid.RenderSize.Width / (childCount + 1)) * (i + 1)) - (btn.Width / 2);
                margin.Top = 2;
                margin.Left = marginL;
                btn.Margin = margin;
            }
        }
        private void Init()
        {
            
            buttonBack.MouseDown += delegate { parent.PagePrev(); };
            buttonFav.MouseDown += delegate { parent.Fav(); };
            buttonHome.MouseDown += delegate { parent.Home(); };
            buttonSize.MouseDown += delegate { parent.Strech(); };
            buttonNext.MouseDown += delegate { parent.PageNext(); };
        }

        private void control_Initialized(object sender, EventArgs e)
        {
            
        }

        private void buttonGrid_Initialized(object sender, EventArgs e)
        {
           
        }

        private void control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ButtonInit();
        }
    }
}
