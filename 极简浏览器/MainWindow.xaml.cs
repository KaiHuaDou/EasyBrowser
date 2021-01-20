using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using mshtml;
using 极简浏览器.Api;

namespace 极简浏览器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string AppStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess( ).MainModule.FileName);
        string Url = "";
        public static object document;
        Timer timer = new Timer(10000);
        string Isnew;
        public MainWindow( )
        {
            Initialize( );
            Isnew = App.Program.Isnew;
        }
        public MainWindow(string url)
        {
            Initialize( );
            Url = url;
            Isnew = App.Program.Isnew;
        }
        public void Initialize( )
        {
            //Initialize
            InitializeComponent( );

            //WebBrowserHelper
            var webBrowserHelper = new WebBrowserHelper(wb);
            webBrowserHelper.NewWindow += WebBrowserOnNewWindow;

            //SuppressScriptErrors
            SuppressScriptErrors(wb);

            //360BrowserKill
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Process.GetProcessesByName("360se").Length != 0)
            {
                Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im 360se.exe");
                Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im 360se.exe");
                Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im 360se.exe");
                Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im 360se.exe");
                Process.Start("%SystemRoot%\\System32\\taskkill.exe", " /f /im 360se.exe");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start( );
            try
            {
                if (App.Program.InputArgu != "")
                {
                    BrowserCore.Navigate(App.Program.InputArgu);
                }
                else if (Url != "" && Url != null && Url != ".")
                {
                    BrowserCore.Navigate(Url);
                }
                else if (!(Isnew == "false"))
                {
                    string PathStart = File.ReadAllText(AppStartupPath + "\\DataBase\\Config.db");
                    if (PathStart == "")
                    {
                        File.WriteAllText(AppStartupPath + "\\DataBase\\Config.db", "about:blank");
                        BrowserCore.Navigate("about:blank");
                    }
                    else
                    {
                        BrowserCore.Navigate(PathStart);

                    }
                }
            }
            catch (Exception)
            {
                this.Close( );
            }
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string str = UrlTextBox.Text;
            if (str == "about:blank")
            {
                BrowserCore.Navigate("about:blank");
                return;
            }
            try
            {
                //处理str
                if (str.Substring(0, 4) != "http" &&
                    str.Substring(0, 4) != "file" &&
                    str.Substring(0, 4) != "ftp" &&
                    str.Contains("."))
                {
                    str = "http://" + str;
                }
                else if (!str.Contains(".") && !str.Contains("C:") && !str.Contains("D:"))
                {
                    str = "https://www.baidu.com/#ie=UTF-8&wd=" + str;
                }
                else if (str.Contains(":\\"))
                {
                    str = "file:///" + str;
                }

                //加载
                try
                {
                    BrowserCore.Navigate(str);
                }
                catch (UriFormatException)
                {
                    if (str.Substring(0, 4) == "http")
                    {
                        try
                        {
                            BrowserCore.Navigate("http://" + str);
                            UrlTextBox.Text = "http://" + str;
                        }
                        catch (Exception) { }
                        return;
                    }
                    label1.Content = "URL错误";
                }
                UrlTextBox.Text = str;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("OMH");
                BrowserCore.Navigate("https://www.baidu.com/#ie=UTF-8&wd=" + UrlTextBox.Text);
            }
        }


        private void Check(object sender, NavigationEventArgs e)
        {
            LoadProgressBar.Value = 100;
            LoadProgressBar.Visibility = Visibility.Collapsed;
            label1.Content = "加载完成";
            document = wb.Document;
            string title = ((HTMLDocument) document).title;
            if (title == null || title == "")
            {
                this.Title = "新标签页 - 极简浏览器";
            }
            else
            {
                this.Title = title + " - 极简浏览器";
            }
            if (wb.Source.ToString( ) != "about:blank")
            {
                UrlTextBox.Text = wb.Source.ToString( );
                string Data = UrlTextBox.Text + " " + this.Title.Replace(" - 极简浏览器", "") + " " + DateTime.Now.Date;
                FileApi.Write(UrlTextBox.Text, FileType.History);
            }
            else
            {
                UrlTextBox.Text = "";
            }

        }

        private void Running(object sender, NavigatingCancelEventArgs e)
        {
            label1.Content = "正在加载中...";
            LoadProgressBar.Visibility = Visibility.Visible;
            Storyboard story = new Storyboard( );
            DoubleAnimation da = new DoubleAnimation(0, 100, new Duration(TimeSpan.FromSeconds(10)));
            Storyboard.SetTarget(da, LoadProgressBar);
            Storyboard.SetTargetProperty(da, new PropertyPath("Value"));
            story.Children.Add(da);
            story.Begin( );
        }

        private void StatusBar_ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (startusBar.Visibility == Visibility.Visible)
            {
                startusBar.Visibility = Visibility.Collapsed;
                OptionMenu.Visibility = Visibility.Collapsed;
                BrowserCore.GetInstance( ).wb.Margin = new Thickness(0, 37, 0, 0);
                HidestartusBar.Header = "显示状态栏";
            }
            else
            {
                startusBar.Visibility = Visibility.Visible;
                OptionMenu.Visibility = Visibility.Visible;
                BrowserCore.GetInstance( ).wb.Margin = new Thickness(0, 37, 0, 35);
                HidestartusBar.Header = "隐藏状态栏";
            }
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) && e.Key != Key.ImeProcessed)
            {
                Load(sender, new RoutedEventArgs( ));
            }
            for (int i = 0; i < 17; i++)
            {
                if (UrlTextBox.Text.Contains(App.BadSectence[i]) == true)
                {
                    MessageBox.Show("我认为您是不善意的，软件将强行关闭！", "网络文明监察局", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
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

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            BrowserCore.GetInstance( ).wb.Dispose( );
        }
    }
}
