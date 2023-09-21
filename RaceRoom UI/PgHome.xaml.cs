using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RaceRoom_UI
{
    /// <summary>
    /// Lógica de interacción para PgHome.xaml
    /// </summary>
    public partial class PgHome : Page
    {
        public PgHome()
        {
            InitializeComponent();
            Webview.Visibility = System.Windows.Visibility.Hidden;
            twitter.Source = new Uri("https://twitter.com/raceroom"); //https://syndication.twitter.com/srv/timeline-profile/screen-name/raceroom

            var fullFilePath = @"https://massets.raceroomracing.com/schedule%20post.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
        }

        private void WebView_ContentLoading(object sender, EventArgs args)
        {
            // Show status.

        }

        private void WebView_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            // Cancel navigation if URL is not allowed. (Implemetation of IsAllowedUri not shown.)

        }

        private void WebView_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess == true)
            {
                loading.Visibility = System.Windows.Visibility.Hidden;
                Webview.Visibility = System.Windows.Visibility.Visible;
                twitter.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // Show status.
            }
        }

    }
}
