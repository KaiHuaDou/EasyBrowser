using System.Windows;
using System.Windows.Input;
using CefSharp.Wpf;

namespace 极简浏览器Cef
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ChromiumWebBrowser cwb;
        public MainWindow( )
        {
            InitializeComponent( );
            cwb = new ChromiumWebBrowser("https://ie.icoa.cn/");
            BrowserGrid.Children.Add(cwb);
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
                cwb.Address = textBox.Text;
        }

        private void cwbGoBack(object sender, RoutedEventArgs e)
        {
            if(cwb.CanGoBack == true)
            {
                cwb.GetBrowser( ).GoBack( );
            }
        }

        private void cwbGoForward(object sender, RoutedEventArgs e)
        {
            if (cwb.CanGoForward == true)
            {
                cwb.GetBrowser( ).GoForward( );
            }
        }

        private void cwbRefresh(object sender, RoutedEventArgs e)
        {
            cwb.GetBrowser( ).Reload( );
        }
    }
}
