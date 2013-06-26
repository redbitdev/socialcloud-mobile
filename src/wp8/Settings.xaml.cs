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
using Coding4Fun.Toolkit.Controls;

namespace CCAdmin
{
    public partial class Settings : PhoneApplicationPage
    {
        

        public Settings()
        {
            InitializeComponent();
            RefreshList();
        }

        private void RefreshList()
        {
            Api.Default.GetSettingsAsync((results) =>
            {
                if (results.Error != null)
                {
                    // there was an error
                    Console.WriteLine(results.Error.Message);
                }
                else
                {
                    Dispatcher.BeginInvoke(() => { lstSettings.ItemsSource = results.Results; });
                }

            });
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).DataContext as Setting;

            var ip = new InputPrompt();
            ip.DataContext = item;
            ip.Title = "Enter the new value for " + item.Key;
            ip.Value = item.Value;
            ip.Completed += ip_Completed;
            ip.Show();
        }

        void ip_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            var item = (sender as InputPrompt).DataContext as Setting;
            var newValue = e.Result;


            //TODO: No Update Settings?????
            //Api.Default.
        }
    }
}