using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using mshtml;
using 极简浏览器.Api;

namespace 极简浏览器
{
    public partial class MainWindow : Window
    {
        //WebBrowser Help Functions
        //ProcessStartInfo psi;
        private void WebBrowserOnNewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            dynamic activeElement = ((HTMLDocument)browser.Document).activeElement;       
            string link = activeElement.ToString( );
            NewInstance.StartNewInstance(link);
            e.Cancel = true;
        }

        static void SuppressScriptErrors(WebBrowser webBrowser)
        {
            webBrowser.Navigating += (s, e) =>
            {
                var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fiComWebBrowser == null)
                    return;

                object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
                if (objComWebBrowser == null)
                    return;

                objComWebBrowser.GetType( ).InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
            };
        }

        //MenuItem Clicks Event
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help( );
            help.Show( );
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About( );
            about.Show( );
        }
        private void AdvBrowser_Click(object sender, RoutedEventArgs e)
        {
            AdvBrowser ab = new AdvBrowser( );
            ab.Show( );
        }
        private void ViewSource_Click(object sender, RoutedEventArgs e)
        {
            WebSource websource = new WebSource( );
            websource.Show( );
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting( );
            setting.Show( );
        }

        private void ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            History history = new History( );
            history.Show( );
        }

        private void ViewBookMark_Click(object sender, RoutedEventArgs e)
        {
            History history = new History( );
            history.Show( );
        }
        private void SetBookMark_Click(object sender, RoutedEventArgs e)
        {
            FileApi.Write(UrlTextBox.Text, FileType.BookMark);
        }

        //GoBack GoForward Refresh 
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.GoBack( );
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.GoForward( );
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.Refresh( );
            LoadProgressBar.Visibility = Visibility.Collapsed;
        }
        private void AddNewPageButton_Click(object sender, RoutedEventArgs e)
        {
            NewInstance.StartNewInstance("about:blank");
        }
    }
}
