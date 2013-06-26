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
    public partial class Blacklist : PhoneApplicationPage
    {
        public Blacklist()
        {
            InitializeComponent();

            RefreshList();
        }

        private void RefreshList()
        {
            Api.Default.GetBlacklistAsync((results) =>
            {
                if (results.Error != null)
                {
                    // there was an error
                    Console.WriteLine(results.Error.Message);
                }
                else
                {
                    Dispatcher.BeginInvoke(() => { lstWords.ItemsSource = results.Results; });
                }

            });
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).DataContext as BlacklistItem;

            Api.Default.DeleteBlackListItem(item.Value, (result) =>
            {
                RefreshList();
            });
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var ip = new InputPrompt();
            ip.Title = "Enter a word";
            ip.Completed += ip_Completed;
            ip.Show();
        }

        void ip_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            Api.Default.AddBlackListItem(e.Result, (result) =>
                {
                    RefreshList();
                });
        }
    }
}