using System.Windows;
using Microsoft.Win32;

namespace RegstryIE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int version = 0;
            if ((string)comboBox.SelectedItem == "IE11")
            {
                version = 11001;
            }
            if ((string) comboBox.SelectedItem == "IE10")
            {
                version = 10000;
            }
            if ((string) comboBox.SelectedItem == "IE9")
            {
                version = 9999;
            }
            if ((string) comboBox.SelectedItem == "IE8")
            {
                version = 8001;
            }
            if ((string) comboBox.SelectedItem == "IE6/7")
            {
                version = 7001;
            }
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION",
                "极简浏览器.exe", version);
            MessageBox.Show("注册完成！", "RegistryIE", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
        }
    }
}
