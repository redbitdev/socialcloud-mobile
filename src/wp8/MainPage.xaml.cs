using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CCAdmin.Resources;
using RedBit.CCAdmin;
using Microsoft.Phone.Notification;
using Microsoft.WindowsAzure.MobileServices;

namespace CCAdmin
{
    public partial class MainPage : PhoneApplicationPage
    {
        int runningProcs = 0;
        ProgressIndicator pi;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            pi = new ProgressIndicator();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            RefreshAll();
        }

        private void RefreshAll()
        {
            runningProcs = 3;
            pi.IsIndeterminate = true;
            pi.IsVisible = true;
            SystemTray.SetProgressIndicator(this, pi);
            Api.Default.GetTweetsAsync((results) =>
            {
                Dispatcher.BeginInvoke(() => { lstTweets.ItemsSource = results.Results; });
                CheckPi();
            });
            Api.Default.GetImagesAsync((results) =>
            {
                Dispatcher.BeginInvoke(() => { lstImages.ItemsSource = results.Results; });
                CheckPi();
            }, 24);
            Api.Default.GetAlertsAsync((results) =>
            {
                Dispatcher.BeginInvoke(() => { lstAlerts.ItemsSource = results.Results; });
                CheckPi();
            });
        }

        private void CheckPi()
        {
            this.runningProcs--;
            if (this.runningProcs == 0)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    pi.IsIndeterminate = false;
                    pi.IsVisible = false;
                });
            }
        }

        private void AlertRules(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AlertRules.xaml", UriKind.Relative));
        }

        private void Blacklist(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Blacklist.xaml", UriKind.Relative));
        }

        private void Promote_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).DataContext as BaseContent;

            if (item.Type == Types.Tweet)
                Api.Default.PromoteTweetItem(item, (results) => { Msg("Tweet Promoted"); });
            else
                Api.Default.PromoteImageItem(item, (results) => { Msg("Image Promoted"); });
            
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void Msg(string txt)
        {
            Dispatcher.BeginInvoke(() => { MessageBox.Show(txt); });
        }

        private void Ban_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).DataContext as BaseContent;

            if (item.Type == Types.Tweet)
                Api.Default.DeleteTweetItem(item, (results) => { Msg("Tweet Banned"); });
            else
                Api.Default.DeleteImageItem(item, (results) => { Msg("Image Banned"); });
        }

        private void btnLogout_Click(object sender, System.EventArgs e)
        {
        	LoginManager.Default.Logout();
            NavigationService.Navigate(new Uri("/Login.xaml?logout=true", UriKind.Relative));
        }

        private void btnAbout_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        
    }
}