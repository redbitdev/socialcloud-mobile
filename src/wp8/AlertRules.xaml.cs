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
    public partial class AlertRules : PhoneApplicationPage
    {
        public AlertRules()
        {
            InitializeComponent();

            RefreshList();
        }

        private void RefreshList()
        {
            Api.Default.GetRulesAsync((results) =>
            {
                if (results.Error != null)
                {
                    // there was an error
                    Console.WriteLine(results.Error.Message);
                }
                else
                {
                    Dispatcher.BeginInvoke(() => { lstRules.ItemsSource = results.Results; });
                }

            });
        }
    }
}