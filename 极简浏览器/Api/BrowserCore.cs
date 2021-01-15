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
                if (window is MainWindow)
                {
                    return (MainWindow) window;
                }
            }
            throw new InvalidOperationException( );
        }
        public static void Navigate(string url)
        {
            GetInstance( ).wb.Navigate(url);
        }

        public static void GoBack( )
        {
            if (GetInstance( ).wb.CanGoBack == true)
                GetInstance( ).wb.GoBack( );
        }

        public static void GoForward( )
        {
            if (GetInstance( ).wb.CanGoForward == true)
                GetInstance( ).wb.GoForward( );
        }
    }
}
