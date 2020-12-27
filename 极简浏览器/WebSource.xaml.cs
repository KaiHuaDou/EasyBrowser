using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mshtml;

namespace 极简浏览器
{
    /// <summary>
    /// WebSource.xaml 的交互逻辑
    /// </summary>
    public partial class WebSource : Window
    {
        public WebSource( )
        {
            InitializeComponent( );
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            HTMLDocument dom = (HTMLDocument) MainWindow.document;
            try
            {
                textBox.Text = dom.documentElement.outerHTML;
            }
            catch (NullReferenceException)
            {
                textBox.Text = "<html>\n<title></title>\n<body></body>\n</html>";
            }
            this.Cursor = null;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            window_Loaded(sender, e);
        }
    }
}
