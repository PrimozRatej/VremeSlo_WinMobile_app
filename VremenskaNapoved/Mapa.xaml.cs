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
using System.Windows.Media;

namespace VremenskaNapoved
{
    public partial class Mapa : PhoneApplicationPage
    {
        //--------------------------------------------------------------JAVNI PODATKI
        IsolatedStorageSettings nastavitve = IsolatedStorageSettings.ApplicationSettings; //za shranjevanje podatkov






        //--------------------------------------------------------------METODE
        public Mapa()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] mesta = new string[5]
            {
                "http://www.vreme.us/vreme-15-dni-ljubljana.html",
                "http://www.vreme.us/vreme-15-dni-maribor.html",
                "http://www.vreme.us/vreme-15-dni-novo-mesto.html",
                "http://www.vreme.us/vreme-15-dni-bovec.html",
                "http://www.vreme.us/vreme-15-dni-koper.html"
            };





            try
            {
                for (int i = 0; i < mesta.Length; i++)
                {
                    WebClient WC = new WebClient();
                    WC.DownloadStringAsync(new Uri(mesta[i], UriKind.Absolute), i.ToString());
                    WC.DownloadStringCompleted += PRENESI_HTML;
                }



            }
            catch
            {

            }

        }







        //----------------------------------------------------------------ZA BUTTON BACKGROUND
        private void PRENESI_HTML(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string state = (string)e.UserState;
                string celHTML = e.Result;
                string[] vrstice = Regex.Split(celHTML, "\n");
                string naslov_slike = "";
                foreach (string vrstica in vrstice)
                {
                    if (vrstica.Contains("src=\"http://www.vreme.us/slike/vreme/"))
                    {
                        char[] znaki = vrstica.ToCharArray();
                        for (int i = 0; i < znaki.Length; i++)
                        {
                            if (znaki[i] == 's' && znaki[i + 1] == 'r' && znaki[i + 2] == 'c' && znaki[i + 3] == '=' && znaki[i + 4] == '"')
                            {
                                i += 5;
                                for (; i < znaki.Length; i++)
                                {
                                    if (znaki[i] == '"') break;
                                    else naslov_slike += znaki[i];
                                }
                                break;
                            }

                        }
                        break;
                    }
                }
                if (nastavitve.Contains(state)) { nastavitve.Remove(state); nastavitve.Add(state, naslov_slike); }
                else { nastavitve.Add(state, naslov_slike); }
                NAREDI_IMAGE_GUMB(state);
            }
            catch
            {
                MessageBox.Show("Imate napako pri prenosu in procesiranju HTML datoteke. Preverite vašo internetno povezavo in poskusite znova!");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        public void NAREDI_IMAGE_GUMB(string state)
        {
            Uri naslov_do_slike = SPREMENI_NASLOV_SLIKE(nastavitve[state].ToString());
            ImageBrush background = new ImageBrush();
            background.ImageSource = new System.Windows.Media.Imaging.BitmapImage(naslov_do_slike);
            background.Opacity = 100;
            switch (state)
            {
                case "0": GUMB_LJUBLJANA.Background = background; break;
                case "1": GUMB_MARIBOR.Background = background; break;
                case "2": GUMB_NOVOMESTO.Background = background; break;
                case "3": GUMB_BOVEC.Background = background; break;
                case "4": GUMB_KOPER.Background = background; break;
            }

        }


        private Uri SPREMENI_NASLOV_SLIKE(string vrni)
        {
            char[] vmesna = vrni.ToCharArray();
            string vmesna2 = "SLIKE/SLIKICE/";

            for (int i = 32; i < vmesna.Length; i++)
            {
                vmesna2 += vmesna[i];
            }
            return new Uri(vmesna2, UriKind.Relative);
        }



















        //--------------------------------------------------------------KLIC METODE GUMBOV
        private void GUMB_Click(object sender, RoutedEventArgs e)
        {
            Button _myButton = (Button)sender;
            string value = _myButton.CommandParameter.ToString();
            if (nastavitve.Contains("KRAJ")) { nastavitve.Remove("KRAJ"); nastavitve.Add("KRAJ", value); }
            else { nastavitve.Add("KRAJ", value); }
            NavigationService.Navigate(new Uri("/GLEJ.xaml", UriKind.Relative));
        }







    }
}