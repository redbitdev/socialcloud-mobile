using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RedBit.CCAdmin;

namespace CCAdmin
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (LoginManager.Default.IsAuthenticated)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                var msg = NavigationContext.QueryString.Keys.Contains("logout");
                if (msg)
                    NavigationService.RemoveBackEntry();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ((ApplicationBarIconButton)sender).IsEnabled = false;
            LoginManager.Default.LoginAsync(() =>
            {
                System.Diagnostics.Debug.WriteLine(LoginManager.Default.IsAuthenticated);
                ((ApplicationBarIconButton)sender).IsEnabled = true;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            },
            () =>
            {
                System.Diagnostics.Debug.WriteLine("use canceled");
            });
        }
    }
}