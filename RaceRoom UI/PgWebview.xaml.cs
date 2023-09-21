using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System;
using System.Windows.Controls;

namespace RaceRoom_UI
{
    /// <summary>
    /// Lógica de interacción para PgWebview.xaml
    /// </summary>
    public partial class PgWebview : Page
    {
        public PgWebview(string url)
        {
            InitializeComponent();
            webView1.Visibility = System.Windows.Visibility.Hidden;
            webView1.Source = new Uri(@url);
        }

        private void webView1_ContentLoading(object sender, EventArgs e)
        {
            // Show status.
            
        }

        private void webView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            // Cancel navigation if URL is not allowed. (Implemetation of IsAllowedUri not shown.)
            
        }

        private void webView1_DOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs args)
        {
            // Show status.
            if (args.Uri != null)
            {
                //statusTextBlock.Text = "Content for " + args.Uri.ToString() + " has finished loading";
            }
        }

        private void webView1_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess == true)
            {
                //statusTextBlock.Text = "Navigation to " + args.Uri.ToString() + " completed successfully.";
                loading.Visibility = System.Windows.Visibility.Hidden;
                webView1.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                //statusTextBlock.Text = "Navigation to: " + args.Uri.ToString() + " failed with error " + args.WebErrorStatus.ToString();
            }
        }
    }
}
