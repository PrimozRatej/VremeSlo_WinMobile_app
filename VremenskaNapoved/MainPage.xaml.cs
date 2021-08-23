using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VremenskaNapoved.Resources;
using System.Threading;
using Windows.Devices.Bluetooth;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;


namespace VremenskaNapoved
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings nastavitve = IsolatedStorageSettings.ApplicationSettings;


        public MainPage()
        {
            InitializeComponent();
            if(!nastavitve.Contains("NACIN"))
            {
                nastavitve.Add("NACIN", " °C");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GLAVNA_INFORMACIJE.TextAlignment = TextAlignment.Center;
            GLAVNA_INFORMACIJE.Text = "\n\nVremenska Napoved v_0.0.1 (Pre_Alpha)\n\n\n";
            GLAVNA_INFORMACIJE.Text += "Program so izdelali:\n- Primož Ratej Cvahte -\n- Tadej Lipar -\n- Blaž Jäger -\n";
            GLAVNA_INFORMACIJE.Text += "Program je bil narejen kot maturitetna naloga.";
        }

        private void GLAVNI_GUMBI_Click(object sender, RoutedEventArgs e)
        {
            Button _myButton = (Button)sender;
            string value = _myButton.CommandParameter.ToString();
            switch (value)
            {
                case "MAPA": NavigationService.Navigate(new Uri("/Mapa.xaml", UriKind.Relative)); break;
                case "NASTAVITVE": NavigationService.Navigate(new Uri("/Nastavitve.xaml", UriKind.Relative)); break;
            }
        }




    }
}