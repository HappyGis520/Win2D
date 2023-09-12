using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;

namespace Win2D_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        CanvasControl _CanvasControl = null;//画布控件
        public MainWindow()
        {
            InitializeComponent();
            this.win2dControl.Loaded += Win2dControl_Loaded; ;

        }

        private void Win2dControl_Loaded(object sender, RoutedEventArgs e)
        {
            win2dControl.Inition();
        }

        private void Intion()
        {
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            win2dControl.BeginDraw();
            win2dControl.DrawLine();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
