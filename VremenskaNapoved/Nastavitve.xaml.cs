using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Threading;
using Windows.Devices.Bluetooth;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;

namespace VremenskaNapoved
{
    
    public partial class Nastavitve : PhoneApplicationPage
    {
        IsolatedStorageSettings nastavitve = IsolatedStorageSettings.ApplicationSettings;
        public Nastavitve()
        {
            InitializeComponent();
        }

        private void RB3_Checked(object sender, RoutedEventArgs e)
        {
            nastavitve.Remove("NACIN");
            nastavitve.Add("NACIN", " °F");
            nastavitve.Save();
        }

        private void RB2_Checked(object sender, RoutedEventArgs e)
        {
            nastavitve.Remove("NACIN");
            nastavitve.Add("NACIN", " °K");
            nastavitve.Save();
        }

        private void RB1_Checked(object sender, RoutedEventArgs e)
        {
            nastavitve.Remove("NACIN");
            nastavitve.Add("NACIN", " °C");
            nastavitve.Save();
        }
    }
}