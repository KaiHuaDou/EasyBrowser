using System;
using System.Windows;

namespace 极简浏览器.Api
{
    public class BrowserCore
    {
        void Navigate(string url)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if(window is MainWindow)
                {
                    ((MainWindow) window).wb.Navigate(url);
                }
            }
        }
    }
}
