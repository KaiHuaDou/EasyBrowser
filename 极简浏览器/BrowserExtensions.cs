using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Help help = new Help( );
            help.Show( );
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            About about = new About( );
            about.Show( );
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            WebSource websource = new WebSource( );
            websource.Show( );
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting( );
            setting.Show( );
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            History history = new History( );
            history.Show( );
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            History history = new History( );
            history.Show( );
        }
        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            FileApi.Write(textBox.Text, FileType.BookMark);
        }

        //GoBack GoForward Refresh 
        private void button_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.GoBack( );
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.GoForward( );
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            BrowserCore.Refresh( );
        }

    }
}
