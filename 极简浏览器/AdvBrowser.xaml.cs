using System.Windows;
using System.Windows.Input;
using CefSharp.Wpf;

namespace 极简浏览器
{
    /// <summary>
    /// AdvBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class AdvBrowser : Window
    {
        ChromiumWebBrowser cwb = new ChromiumWebBrowser();
        bool IsNew = true;
        string Url = "";
        public AdvBrowser( )
        {
            InitializeComponent( );
            cwb.Margin = new Thickness(0, 0, 0, 0);
            cwb.TargetUpdated += Cwb_TargetUpdated;
            BrowserGrid.Children.Add(cwb);
        }

        public AdvBrowser(string url)
        {
            Url = url;
            IsNew = false;
            InitializeComponent( );
            cwb.Margin = new Thickness(0, 0, 0, 0);
            cwb.TargetUpdated += Cwb_TargetUpdated;
            BrowserGrid.Children.Add(cwb);
        }
        private void Cwb_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            e.Handled = true;
            AdvBrowser ab = new AdvBrowser((string)e.TargetObject.GetValue(e.Property));
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) && e.Key != Key.ImeProcessed)
            {
                cwb.Address = textBox.Text;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(cwb.CanGoBack == true)
            {
                cwb.Back( );
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(cwb.CanGoForward == true)
            {
                cwb.Forward( );
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if(cwb.CanReload == true)
            {
                cwb.Reload( );
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(IsNew == false)
            {
                cwb.Address = Url;
            }
        }
    }
}
