using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
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
            catch (UriFormatException ex)
            {
                //MessageBox.Show(App.Program.InputArgu);
                File.AppendAllText(AppStartupPath + "\\logs\\" + 
                    DateTime.Now.Year.ToString( ) +
                    DateTime.Now.Month.ToString( ) +
                    DateTime.Now.Day.ToString( ) +
                    DateTime.Now.Hour.ToString( ) +
                    DateTime.Now.Minute.ToString( ) +
                    DateTime.Now.Second.ToString( ) +
                    DateTime.Now.Millisecond.ToString( ) + 
                    ".log",
                    ex.Message + "\nSite:" + App.Program.InputArgu);
                this.Close( );
            }
            catch (Exception)
            {
                ;
            }
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string str = textBox.Text;
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
                            textBox.Text = "http://" + str;
                        }
                        catch (Exception) { }
                        return;
                    }
                    label1.Content = "URL错误";
                }
                textBox.Text = str;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("OMH");
                BrowserCore.Navigate("https://www.baidu.com/#ie=UTF-8&wd=" + textBox.Text);
            }
        }


        private void Check(object sender, NavigationEventArgs e)
        {
            pb.Value = 100;
            pb.Visibility = Visibility.Collapsed;
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
                textBox.Text = wb.Source.ToString( );
                string Data = textBox.Text + " " + this.Title.Replace(" - 极简浏览器", "") + " " + DateTime.Now.Date;
                FileApi.Write(textBox.Text + "\n", FileType.History);
            }
            else
            {
                textBox.Text = "";
            }

        }

        private void sizechange(object sender, SizeChangedEventArgs e)
        {
            if (this.Width - 155 > 0)
            {
                textBox.Width = this.ActualWidth - 155;
                startusBar.Width = this.ActualWidth - 72;
            }
        }


        private void Running(object sender, NavigatingCancelEventArgs e)
        {
            label1.Content = "正在加载中...";
            Storyboard story = new Storyboard( );
            DoubleAnimation da = new DoubleAnimation(0, 90, new Duration(TimeSpan.FromSeconds(20)));
            Storyboard.SetTarget(da, pb);
            Storyboard.SetTargetProperty(da, new PropertyPath("Value"));
            story.Children.Add(da);
            story.Begin( );
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            NewInstance.StartNewInstance("about:blank");
        }

        private void StatusBar_ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (startusBar.Visibility == Visibility.Visible)
            {
                startusBar.Visibility = Visibility.Collapsed;
                menu.Visibility = Visibility.Collapsed;
                BrowserCore.GetInstance( ).wb.Margin = new Thickness(0, 30, 0, 0);
                HidestartusBar.Header = "显示状态栏";
            }
            else
            {
                startusBar.Visibility = Visibility.Visible;
                menu.Visibility = Visibility.Visible;
                BrowserCore.GetInstance( ).wb.Margin = new Thickness(0, 30, 0, 35);
                HidestartusBar.Header = "隐藏状态栏";
            }
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) && e.Key != Key.ImeProcessed)
            {
                Load(sender, new RoutedEventArgs( ));
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            wb.Dispose( );
        }

        private void mi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BrowserCore.Navigate(((string) ((ListBox) sender).SelectedItem));
        }
    }
}
