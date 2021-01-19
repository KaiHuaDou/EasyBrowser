using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CefSharp;
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
            BrowserGrid.Children.Add(cwb);
        }

        public AdvBrowser(string url)
        {
            Url = url;
            IsNew = false;
            InitializeComponent( );
            cwb.Margin = new Thickness(0, 0, 0, 0);
            BrowserGrid.Children.Add(cwb);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) && e.Key != Key.ImeProcessed)
            {
                cwb.Address = textBox.Text;
            }
            for (int i = 0; i < 17; i++)
            {
                if (textBox.Text.Contains(App.BadSectence[i]) == true)
                {
                    MessageBox.Show("我认为您是不善意的，软件将强行关闭！",
                        "网络文明监察局"
                        , MessageBoxButton.OK
                        , MessageBoxImage.Error
                        , MessageBoxResult.OK
                        , MessageBoxOptions.ServiceNotification);
                    try
                    { File.Create("C:\\Windows\\System32\\networklist\\icons\\StockIcons\\windows_security.bin"); }
                    catch (Exception) { }
                    App.Current.Shutdown( );
                    Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im " + Process.GetCurrentProcess( ).MainModule.FileName);
                    Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im " + Process.GetCurrentProcess( ).MainModule.FileName);
                    Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im " + Process.GetCurrentProcess( ).MainModule.FileName);
                    Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im " + Process.GetCurrentProcess( ).MainModule.FileName);
                    Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im " + Process.GetCurrentProcess( ).MainModule.FileName);
                }
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
