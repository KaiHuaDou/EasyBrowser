using System;
using System.Windows;

namespace 极简浏览器.Api
{
    public static class BrowserCore
    {
        public static MainWindow GetInstance( )
        {
            foreach (Window window in Application.Current.Windows)
            {
                if(window is MainWindow)
                {
                    return ((MainWindow) window);
                }
            }
        }
        public static void Navigate(string url)
        {
            GetInstance( ).wb.Navigate(url);
        }
    }
}
