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
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace VremenskaNapoved
{
    public partial class Glej : PhoneApplicationPage
    {
        IsolatedStorageSettings nastavitve = IsolatedStorageSettings.ApplicationSettings;
        public string LLINK = "";
        StackPanel D1 = new StackPanel();
        StackPanel D2 = new StackPanel();
        StackPanel D3 = new StackPanel();
        StackPanel D4 = new StackPanel();
        StackPanel D5 = new StackPanel();


        public Glej()
        {
            InitializeComponent();
            Image LOADING1 = new Image();
            Image LOADING2 = new Image();
            Image LOADING3 = new Image();
            Image LOADING4 = new Image();
            Image LOADING5 = new Image();
            Uri SOURCE_LOADING = new Uri("SLIKE/NALAGANJE.png",UriKind.Relative);
            LOADING1.Source = new BitmapImage(SOURCE_LOADING);
            LOADING2.Source = new BitmapImage(SOURCE_LOADING);
            LOADING3.Source = new BitmapImage(SOURCE_LOADING);
            LOADING4.Source = new BitmapImage(SOURCE_LOADING);
            LOADING5.Source = new BitmapImage(SOURCE_LOADING);

            XX01.Children.Add(LOADING1);
            XX02.Children.Add(LOADING2);
            XX03.Children.Add(LOADING3);
            XX04.Children.Add(LOADING4);
            XX05.Children.Add(LOADING5);


        }


        protected override void OnNavigatedTo(NavigationEventArgs e)//OB NAVIGACIJI NA STRAN
        {
            WebClient WC = new WebClient();
            
            WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("V15"), UriKind.Absolute));
            WC.DownloadStringCompleted += PRENESI_HTML_V15;
            
            

//            WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("V15"), UriKind.Absolute));
//           WC.DownloadStringCompleted += PRENESI_HTML_V15;
        }

        public void PRENESI_HTML_V15(object sender, DownloadStringCompletedEventArgs e)//PROCESIRANJE
        {
            try
            {
                string[] vse_vrstice_html_datoteke = Regex.Split(e.Result, "\n");

                int STEJ = 0;
                for (int i = 0; i < vse_vrstice_html_datoteke.Length; i++)
                {
                    string vrstica = vse_vrstice_html_datoteke[i];
                    if (vrstica.Contains("<table width=\"1265\" border=\"0\" style=\"border-collapse") || vrstica.Contains("</td><td align=right><font size=3 color=blue>"))
                    {
                        string datum, napoved;
                        Uri slika;
                        int temperatura_najnizja, temperatura_najvisja;

                        char[] znaki = vrstica.ToCharArray();
                        char[] znaki2 = vse_vrstice_html_datoteke[i + 1].ToCharArray();

                        if (STEJ == 0) datum = "Danes";
                        else if (STEJ == 1) datum = "Jutri";
                        else datum = BRANJE_PODATKOV.V15_GLEJ_DATUM(znaki, vrstica);
                        if (datum.Contains("&#268;")) datum = datum.Replace("&#268;", "Č");
                        napoved = BRANJE_PODATKOV.V15_GLEJ_NAPOVED(znaki);
                        temperatura_najnizja = BRANJE_PODATKOV.V15_GLEJ_TEMPERATURO_NAJNIZJO(znaki2);
                        temperatura_najvisja = BRANJE_PODATKOV.V15_GLEJ_TEMPERATURO_NAJVISJA(znaki);
                        slika = BRANJE_PODATKOV.V15_GLEJ_SLIKO(znaki, i);
                        Uri slika_source = new Uri("SLIKE/V15.png",UriKind.Relative);
                        LLINK = "SLIKE/V15.png";
                        NAREDI_PANEL(datum, napoved, slika, temperatura_najnizja, temperatura_najvisja, slika_source, STEJ);
                        STEJ++;
                        if (STEJ == 5) break;
                    }
                }

                WebClient WC = new WebClient();
                WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("ACC"), UriKind.Absolute));
                WC.DownloadStringCompleted += PRENESI_HTML_ACC;
            }
            catch
            {
                NAREDI_PANEL_FAIL(1);
                NAREDI_PANEL_FAIL(2);
                NAREDI_PANEL_FAIL(3);
                NAREDI_PANEL_FAIL(4);
                NAREDI_PANEL_FAIL(5);
                WebClient WC = new WebClient();
                WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("ACC"), UriKind.Absolute));
                WC.DownloadStringCompleted += PRENESI_HTML_ACC;
            }
        }

        public void PRENESI_HTML_ACC(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string[] HTML_VRSTICE = Regex.Split(e.Result, "\n");
                Uri slika_source = new Uri("SLIKE/ACC.png",UriKind.Relative);
                for (int i = 0; i < HTML_VRSTICE.Length; i++)
                {
                    if (HTML_VRSTICE[i].Contains("<div class=\"fc\">"))
                    {
                        string vrstica = BRANJE_PODATKOV.ACC_BRANJE_PODATKOV(Regex.Split(HTML_VRSTICE[i], "URNA NAPOVED"));



                        char[] vmesna1 = vrstica.ToCharArray();

                        string nov = "";
                        for (int ii = 0; ii < vmesna1.Length; ii++)
                        {
                            if (vmesna1[ii] == '"')
                            {
                                for (; ii < vmesna1.Length; ii++)
                                {
                                    if (vmesna1[ii] == '×') break;
                                }
                            }
                            else nov += vmesna1[ii];
                        }

                        string[] RAZBIJ_NA_DNEVE = Regex.Split(nov, "××");


                        for (int ii = 0; ii < RAZBIJ_NA_DNEVE.Length; ii++)
                        {
                            string[] podatki = Regex.Split(RAZBIJ_NA_DNEVE[ii], ">");
                            if(ii == 0)podatki[0] = "Danes";
                            if(ii == 1)podatki[0] = "Jutri";
                            string[] temperatura = Regex.Split(podatki[1], " / ");
                            int temp_nizka = int.Parse(temperatura[0]);
                            int temp_visoka = int.Parse(temperatura[1]);
                            Uri kotejebe = new Uri(podatki[2], UriKind.Absolute);
                            LLINK = "SLIKE/ACC.png";
                            NAREDI_PANEL(podatki[0], "", kotejebe, temp_nizka, temp_visoka, slika_source, ii);
                        }
                        break;
                    }
                }

                WebClient WC = new WebClient();
                WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("VTK"), UriKind.Absolute));
                WC.DownloadStringCompleted += PRENESI_HTML_VTK;



            }
            catch
            {
                NAREDI_PANEL_FAIL(1);
                NAREDI_PANEL_FAIL(2);
                NAREDI_PANEL_FAIL(3);
                NAREDI_PANEL_FAIL(4);
                NAREDI_PANEL_FAIL(5);
                WebClient WC = new WebClient();
                WC.DownloadStringAsync(new Uri(IZLUSCI_STRING_KRAJA("VTK"), UriKind.Absolute));
                WC.DownloadStringCompleted += PRENESI_HTML_ACC;
            }

        }


        public void PRENESI_HTML_VTK(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string[] HTML_VRSTICE = Regex.Split(e.Result, "\n");
                Uri slika_source = new Uri("SLIKE/VTK.png",UriKind.Relative);
                int COUNT = 0;
                for (int i = 0; i < HTML_VRSTICE.Length; i++)
                {
                    
                    if (HTML_VRSTICE[i].Contains("<div class=\"wp-forecast-fc-oneday\">") && HTML_VRSTICE[i+1].Contains("<div class=\"wp-forecast-fc-head\">"))
                    {
                        string datum;
                        if (COUNT == 1) datum = "Jutri";
                        else if (COUNT == 0) datum = "Danes";
                        else datum = BRANJE_PODATKOV.VTK_BERI_DATUM(HTML_VRSTICE[i + 1]); 
                        int temp_min = BRANJE_PODATKOV.VTK_BERI_TEMP_MIN(HTML_VRSTICE[i+19]); 
                        int temp_max = BRANJE_PODATKOV.VTK_BERI_TEMP_MAX(HTML_VRSTICE[i+9]);
                        string napoved = BRANJE_PODATKOV.VTK_BERI_NAPOVED(HTML_VRSTICE[i+8]);
                        Uri slika = new Uri(BRANJE_PODATKOV.VTK_BERI_SLIKA(HTML_VRSTICE[i+5]), UriKind.Absolute);
                        LLINK = "SLIKE/VTK.png";
                        NAREDI_PANEL(datum, napoved, slika, temp_min, temp_max, slika_source, COUNT);
                        COUNT++;
                    }
                }
                XX01.Children.Clear();
                XX02.Children.Clear();
                XX03.Children.Clear();
                XX04.Children.Clear();
                XX05.Children.Clear();
                XX01.Children.Add(D1);
                XX02.Children.Add(D2);
                XX03.Children.Add(D3);
                XX04.Children.Add(D4);
                XX05.Children.Add(D5);


            }
            catch
            {
                XX01.Children.Clear();
                XX02.Children.Clear();
                XX03.Children.Clear();
                XX04.Children.Clear();
                XX05.Children.Clear();
                NAREDI_PANEL_FAIL(1);
                NAREDI_PANEL_FAIL(2);
                NAREDI_PANEL_FAIL(3);
                NAREDI_PANEL_FAIL(4);
                NAREDI_PANEL_FAIL(5);
                XX01.Children.Add(D1);
                XX02.Children.Add(D2);
                XX03.Children.Add(D3);
                XX04.Children.Add(D4);
                XX05.Children.Add(D5);
            }
        }


        








        //-------------------------------------------------------------------------------------------------OSTALE METODE
        public string IZLUSCI_STRING_KRAJA(string METODA) //METODA ZA DOLOČITEV CILJNEGA HTML NASLOVA KRAJA
        {
            string vrni = nastavitve["KRAJ"].ToString();



            if (METODA == "V15")
            {
                switch (vrni)
                {
                    case "LJ": vrni = "http://www.vreme.us/vreme-15-dni-ljubljana.html"; PIVOT.Title = "LJUBLJANA"; break;
                    case "MB": vrni = "http://www.vreme.us/vreme-15-dni-maribor.html"; PIVOT.Title = "MARIBOR"; break;
                    case "NM": vrni = "http://www.vreme.us/vreme-15-dni-novo-mesto.html"; PIVOT.Title = "NOVO MESTO"; break;
                    case "BO": vrni = "http://www.vreme.us/vreme-15-dni-bovec.html"; PIVOT.Title = "BOVEC"; break;
                    case "KP": vrni = "http://www.vreme.us/vreme-15-dni-koper.html"; PIVOT.Title = "KOPER"; break;
                }
            }
            else if (METODA == "ACC")
            {
                switch (vrni)
                {
                    case "LJ": vrni = "http://www.siol.net/Vreme.aspx?wloc=cityid:299198"; PIVOT.Title = "LJUBLJANA"; break;
                    case "MB": vrni = "http://www.siol.net/Vreme.aspx?wloc=cityid:299438"; PIVOT.Title = "MARIBOR"; break;
                    case "NM": vrni = "http://www.siol.net/Vreme.aspx?wloc=cityid:300067"; PIVOT.Title = "NOVO MESTO"; break;
                    case "BO": vrni = "http://www.siol.net/Vreme.aspx?wloc=cityid:298808"; PIVOT.Title = "BOVEC"; break;
                    case "KP": vrni = "http://www.siol.net/Vreme.aspx?wloc=cityid:298684"; PIVOT.Title = "KOPER"; break;
                }
            }
            else if (METODA == "VTK")
            {
                switch (vrni)
                {
                    case "LJ": vrni = "http://www.vremesi.com/vremenska-napoved-ljubljana/"; PIVOT.Title = "LJUBLJANA"; break;
                    case "MB": vrni = "http://www.vremesi.com/vremenska-napoved-maribor/"; PIVOT.Title = "MARIBOR"; break;
                    case "NM": vrni = "http://www.vremesi.com/vremenska-napoved-novo-mesto/"; PIVOT.Title = "NOVO MESTO"; break;
                    case "BO": vrni = "http://www.vremesi.com/vremenska-napoved-bovec/"; PIVOT.Title = "BOVEC"; break;
                    case "KP": vrni = "http://www.vremesi.com/vremenska-napoved-koper/"; PIVOT.Title = "KOPER"; break;
                }
            }


            return vrni;
        }

        private void NAREDI_PANEL(string datum, string napoved, Uri slika, int temperatura_najnizja, int temperatura_najvisja, Uri slika_source, int DAN) //DODAJANJE STACKPANELA NA PROGRAMSKI NAČIN
        {
            Grid Vsebina2 = new Grid();
            Vsebina2.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 220, 220, 220));

            ColumnDefinition CD1 = new ColumnDefinition();
            ColumnDefinition CD2 = new ColumnDefinition();
            ColumnDefinition CD3 = new ColumnDefinition();
            CD1.Width = new GridLength(140);
            CD2.Width = new GridLength(160);
            CD2.Width = new GridLength(160);

            RowDefinition RD1 = new RowDefinition();
            RowDefinition RD2 = new RowDefinition();
            RowDefinition RD3 = new RowDefinition();
            RowDefinition RD4 = new RowDefinition();
            RD1.Height = new GridLength(100);
            RD2.Height = new GridLength(100);
            RD3.Height = new GridLength(10);
            RD3.Height = new GridLength(120);

            Vsebina2.ColumnDefinitions.Add(CD1);
            Vsebina2.ColumnDefinitions.Add(CD2);
            Vsebina2.ColumnDefinitions.Add(CD3);
            Vsebina2.RowDefinitions.Add(RD1);
            Vsebina2.RowDefinitions.Add(RD2);
            Vsebina2.RowDefinitions.Add(RD3);
            Vsebina2.RowDefinitions.Add(RD4);

            double temperatura_najnizja_nova = temperatura_najnizja;
            double temperatura_najvisja_nova = temperatura_najvisja;
            string nacintemp = "°C";

            if(nastavitve["NACIN"].ToString() == " °F")
            {
                temperatura_najvisja_nova = temperatura_najvisja * 9 / 5 + 32;
                temperatura_najnizja_nova = temperatura_najnizja * 9 / 5 + 32;
                nacintemp = "°F";
            }
            else if (nastavitve["NACIN"].ToString() == " °K")
            {
                //274.15 K == 1°C
                //273.15 K == 1°C
                if(temperatura_najnizja <= 0)
                {
                      temperatura_najnizja_nova = 273.15 - Math.Abs(temperatura_najnizja);
                }
                else
                {
                    temperatura_najnizja_nova = 273.15 + temperatura_najnizja;
                }
                if (temperatura_najvisja <= 0)
                {
                    temperatura_najvisja_nova = 273.15 - Math.Abs(temperatura_najvisja);
                }
                else
                {
                    temperatura_najvisja_nova = 273.15 + temperatura_najvisja;
                }
                nacintemp = "°K";
            }


            Image SLIKA = new Image();
            SLIKA.Height = 120;
            SLIKA.Width = 120;
            SLIKA.HorizontalAlignment = HorizontalAlignment.Center;
            SLIKA.VerticalAlignment = VerticalAlignment.Center;
            SLIKA.Source = new BitmapImage(slika);

            TextBlock DATUM = new TextBlock();
            DATUM.Text = datum;
            DATUM.HorizontalAlignment = HorizontalAlignment.Center;
            DATUM.VerticalAlignment = VerticalAlignment.Center;
            DATUM.FontSize = 25;
            DATUM.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));

            TextBlock TEMPERATURA = new TextBlock();
            TEMPERATURA.Text = "Najvišja\nTemperatura\n" + temperatura_najvisja_nova + nacintemp;
            TEMPERATURA.TextAlignment = TextAlignment.Center;
            TEMPERATURA.HorizontalAlignment = HorizontalAlignment.Center;
            TEMPERATURA.VerticalAlignment = VerticalAlignment.Center;
            TEMPERATURA.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));

            TextBlock TEMPERATURA2 = new TextBlock();
            if(temperatura_najnizja==-250)
            {
                TEMPERATURA2.Text = "NaN";
            }
            else
            {
                TEMPERATURA2.Text = "Najnižja\nTemperatura\n" + temperatura_najnizja_nova + nacintemp;
            }
            TEMPERATURA2.TextAlignment = TextAlignment.Center;
            TEMPERATURA2.HorizontalAlignment = HorizontalAlignment.Center;
            TEMPERATURA2.VerticalAlignment = VerticalAlignment.Center;
            TEMPERATURA2.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 255));

            TextBlock NAPOVED = new TextBlock();
            NAPOVED.Text = napoved;
            NAPOVED.VerticalAlignment = VerticalAlignment.Top;
            NAPOVED.HorizontalAlignment = HorizontalAlignment.Center;
            NAPOVED.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));

            Image SOURCE = new Image();
            SOURCE.Height = 110;
            SOURCE.Width = 470;
            SOURCE.HorizontalAlignment = HorizontalAlignment.Center;
            SOURCE.VerticalAlignment = VerticalAlignment.Center;
            SOURCE.Source = new BitmapImage(slika_source);
            //SOURCE.DoubleTap += new EventHandler<System.Windows.Input.GestureEventArgs>(OnButtonDoubleTap);
            
            Grid.SetColumn(SLIKA, 0);
            Grid.SetRow(SLIKA, 0);
            Grid.SetRowSpan(SLIKA, 2);
            Vsebina2.Children.Add(SLIKA);

            Grid.SetColumn(DATUM, 1);
            Grid.SetRow(DATUM, 0);
            Grid.SetColumnSpan(DATUM, 2);
            Vsebina2.Children.Add(DATUM);

            Grid.SetColumn(TEMPERATURA, 1);
            Grid.SetRow(TEMPERATURA, 1);
            Vsebina2.Children.Add(TEMPERATURA);

            Grid.SetColumn(TEMPERATURA2, 2);
            Grid.SetRow(TEMPERATURA2, 1);
            Vsebina2.Children.Add(TEMPERATURA2);

            Grid.SetColumn(NAPOVED, 1);
            Grid.SetRow(NAPOVED, 2);
            Grid.SetColumnSpan(NAPOVED, 2);
            Vsebina2.Children.Add(NAPOVED);

            Grid.SetColumn(SOURCE, 0);
            Grid.SetRow(SOURCE, 2);
            Grid.SetColumnSpan(SOURCE, 3);
            Vsebina2.Children.Add(SOURCE);

            Thickness mGRID = Vsebina2.Margin;
            mGRID.Top = 20;
            mGRID.Bottom = 20;
            Vsebina2.Margin = mGRID;

            switch(DAN+1)
            {
                case 1: D1.Children.Add(Vsebina2); break;
                case 2: D2.Children.Add(Vsebina2); break;
                case 3: D3.Children.Add(Vsebina2); break;
                case 4: D4.Children.Add(Vsebina2); break;
                case 5: D5.Children.Add(Vsebina2); break;
            }
            
        }


        public void OnButtonDoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (LLINK == "SLIKE/ACC.png")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri("http://www.vremesi.com");
                webBrowserTask.Show();
            }
            else if (LLINK == "SLIKE/V15.png")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri("http://www.vreme.us");
                webBrowserTask.Show();
            }
            else if (LLINK == "SLIKE/VTK.png")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri("http://www.siol.net/Vreme.aspx?");
                webBrowserTask.Show();
            }
        }


        private void NAREDI_PANEL_FAIL(int DAN)
        {
            Grid Vsebina2 = new Grid();
            Vsebina2.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 220, 220, 220));
            ColumnDefinition CD1 = new ColumnDefinition();
            CD1.Width = new GridLength(480);
            RowDefinition RD1 = new RowDefinition();
            RD1.Height = new GridLength(330);
            Vsebina2.ColumnDefinitions.Add(CD1);
            Vsebina2.RowDefinitions.Add(RD1);

            TextBlock DATUM = new TextBlock();
            DATUM.Text = "TRENUTNO\n NI NA VOLJO";
            DATUM.HorizontalAlignment = HorizontalAlignment.Center;
            DATUM.VerticalAlignment = VerticalAlignment.Center;
            DATUM.FontSize = 40;
            DATUM.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));

            Vsebina2.Children.Add(DATUM);
            switch (DAN + 1)
            {
                case 1: D1.Children.Add(Vsebina2); break;
                case 2: D2.Children.Add(Vsebina2); break;
                case 3: D3.Children.Add(Vsebina2); break;
                case 4: D4.Children.Add(Vsebina2); break;
                case 5: D5.Children.Add(Vsebina2); break;
            }

        }

    }
}