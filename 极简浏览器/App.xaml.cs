using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace 极简浏览器
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        static string AppStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        JumpList jumplist = new JumpList( );
        public class Program
        {
            public static string InputArgu ="";
            public static string Isnew { get; private set; }
            ///<summary>
            ///应用程序的主入口点。
            ///</summary>
            [STAThread]
            public static void Main(string[] args)
            {
                if (args.Length >= 1)
                {
                    InputArgu = args[0];
                    Isnew = args[1];
                }
                try
                {
                     App app = new App( );
                     app.InitializeComponent( );
                     RuntimeCheckAndAutoFix( );
                     app.Run( );
                }
                catch (XamlParseException e)
                {
                    System.Windows.MessageBox.Show(e.Message, "极简浏览器", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK, System.Windows.MessageBoxOptions.ServiceNotification);
                }
            }
        }

        static void RuntimeCheckAndAutoFix( )
        {
            if (Directory.Exists(AppStartupPath + "\\DataBase") == false)
                Directory.CreateDirectory(AppStartupPath + "\\DataBase");
            if (Directory.Exists(AppStartupPath + "\\logs") == false)
                Directory.CreateDirectory(AppStartupPath + "\\logs");
            if (File.Exists(AppStartupPath + "\\DataBase\\History.db") == false)
                File.Create(AppStartupPath + "\\DataBase\\History.db");
            if (File.Exists(AppStartupPath + "\\DataBase\\BookMark.db") == false)
                File.Create(AppStartupPath + "\\DataBase\\BookMark.db");
            if (File.Exists(AppStartupPath + "\\logs\\log.log") == false)
                File.Create(AppStartupPath + "\\logs\\log.log");
        }

        private void ExpetionOpen(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MainWindow.TaskbarItemInfo.ProgressValue = 100;
                MainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
                MainWindow.TaskbarItemInfo.Overlay = new BitmapImage(new Uri("pack://application:,,,/resource/Error.png"));
                string AppStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess( ).MainModule.FileName);
                string LogPath = AppStartupPath + @"\logs\log.log";
                File.AppendAllText(LogPath ,
                    e.Exception.Message + "\n" + e.Exception.Source + "\n"
                    + e.Exception.TargetSite + "\n" + e.Exception.HelpLink);
                //MessageBox
                string message, innermsg, endmsg;
                message = "很抱歉，程序发生了未处理的异常\n";
                innermsg = "原因:" + e.Exception.Message + "\n" + e.Exception.Source + "程序集中的" + e.Exception.TargetSite + "方法引发了此异常。\n";
                endmsg = "帮助链接:" + e.Exception.HelpLink;
                DialogResult dr = new DialogResult( );
                dr = MessageBox.Show(
                    message + innermsg + endmsg, "极简浏览器",
                    MessageBoxButtons.AbortRetryIgnore,
                    MessageBoxIcon.Error);
                MainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                MainWindow.TaskbarItemInfo.Overlay = null;
                e.Handled = true;
                if (dr == DialogResult.Abort)
                    Current.Shutdown(1);
                if (dr == DialogResult.Retry)
                    e.Handled = false;
                if (dr == DialogResult.Ignore)
                    return;
                return;
            }
            catch (NullReferenceException)
            {
                App.Current.Shutdown( );
            }
        }

        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            JumpList.SetJumpList(App.Current, jumplist);
            JumpTask jumptask = new JumpTask( );
            jumptask.CustomCategory = "任务";
            jumptask.Title = "新建窗口";
            jumptask.ApplicationPath = AppStartupPath + "\\极简浏览器.exe";
            jumptask.IconResourcePath = AppStartupPath + "\\极简浏览器.exe";
            jumptask.Arguments = "about:blank false";
            jumplist.JumpItems.Add(jumptask);
            jumplist.Apply( );
        }
    }
}

