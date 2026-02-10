using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace NsccSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Hook events
            Browser.NavigationCompleted += Browser_NavigationCompleted;
            Loaded += (_, __) => InitWebView();

            // Keyboard shortcuts: Alt+Left/Right, Ctrl+R
            PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private async void InitWebView()
        {
            // Initialize WebView2 (Edge/Chromium-based)
            await Browser.EnsureCoreWebView2Async();

            // Keep Back/Forward enabled state in sync with history
            Browser.CoreWebView2.HistoryChanged += (_, __) => UpdateNavButtons();

            // Make target=_blank links open in the same view
            Browser.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                e.Handled = true;
                if (!string.IsNullOrEmpty(e.Uri))
                    Browser.CoreWebView2.Navigate(e.Uri);
            };

            // Optional UI settings (you can disable context menu/devtools if desired)
            // Browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true; // default is true
            // Browser.CoreWebView2.Settings.AreDevToolsEnabled = true;

            // Start page
            Navigate("https://www.nscc.ca/");
        }

        private void UpdateNavButtons()
        {
            BtnBack.IsEnabled = Browser.CoreWebView2?.CanGoBack == true;
            BtnForward.IsEnabled = Browser.CoreWebView2?.CanGoForward == true;
        }

        private void Browser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            UpdateNavButtons();

            if (Browser?.Source != null)
                AddressBox.Text = Browser.Source.ToString();

            if (!e.IsSuccess)
                MessageBox.Show($"Navigation failed: {e.WebErrorStatus}", "Navigation Error");
        }

        private void Navigate(string url)
        {
            try
            {
                if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    url = "https://" + url;

                if (Browser.CoreWebView2 != null)
                    Browser.CoreWebView2.Navigate(url);
                else
                    Browser.Source = new Uri(url); // will navigate after init
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Please enter a valid URL.", "Invalid Address");
            }
        }

        // ===== Toolbar buttons =====
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CoreWebView2?.CanGoBack == true)
                Browser.CoreWebView2.GoBack();
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CoreWebView2?.CanGoForward == true)
                Browser.CoreWebView2.GoForward();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Browser.CoreWebView2?.Reload();
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Navigate(AddressBox.Text.Trim());
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Left)
            {
                BtnBack_Click(sender, e); e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Right)
            {
                BtnForward_Click(sender, e); e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.R)
            {
                BtnRefresh_Click(sender, e); e.Handled = true;
            }
        }

        // ===== Menu items → navigate INSIDE the embedded browser =====
        private void About_Click(object sender, RoutedEventArgs e)
            => Navigate("https://www.nscc.ca/about/index.asp");          // About NSCC

        private void Programs_Click(object sender, RoutedEventArgs e)
            => Navigate("https://www.nscc.ca/programs-and-courses/index.asp"); // Programs & courses

        private void Campuses_Click(object sender, RoutedEventArgs e)
            => Navigate("https://www.nscc.ca/campuses/index.asp");       // Campuses & locations

        private void Contact_Click(object sender, RoutedEventArgs e)
            => Navigate("https://www.nscc.ca/contact/default.aspx");     // Contact

        private void GoToSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            var win = new SecondWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.ShowDialog();
        }
    }
}