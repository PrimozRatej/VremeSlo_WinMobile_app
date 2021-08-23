using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
 
        public MainPage()
        {
            InitializeComponent();

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Uri URI = new Uri("http://www.accuweather.com/sl/si/ljubljana/299198/month/299198?view=table", UriKind.Absolute);
            WebClient web = new WebClient();
            web.DownloadStringAsync(URI);
            web.DownloadStringCompleted += PRENESI_HTML;


        }

       
        private void PRENESI_HTML(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] HTML_VRSTICE = Regex.Split(e.Result,"\n");

            for(int i = 0; i<HTML_VRSTICE.Length; i++ )
            {
                if(HTML_VRSTICE[i].Contains("<tr class=\"lo calendar-list-cl-tr cl hv")
                    && HTML_VRSTICE[i+1].Contains("<th scope=\"row\" style=\"background-color:#f6f7f8\">")
                    && HTML_VRSTICE[i+2].Contains("<td style=\"font-weight:bold;\">"))
                {
                    char[] datum_array = HTML_VRSTICE[i + 1].ToCharArray();
                    char[] temp_visoka_array = HTML_VRSTICE[i+2].ToCharArray();
                    char[] temp_nizka_array = HTML_VRSTICE[i + 3].ToCharArray();
                    char[] slika_in_napoved_array = HTML_VRSTICE[i + 7].ToCharArray();

                    string datum = POGLEJ_DATUM(ref datum_array);
                    string temp_visoka = POGLEJ_TEMP_VISOKA(HTML_VRSTICE[i + 2]);
                    string temp_nizka = POGLEJ_TEMP_NIZKA(HTML_VRSTICE[i + 3]);
                    string slika = POGLEJ_SLIKA(ref slika_in_napoved_array);
                    string napoved = POGLEJ_NAPOVED(ref slika_in_napoved_array);

                    BitmapImage ii = new BitmapImage();
                    ii.UriSource = new Uri(slika);
                    SLIKA.Source = ii;
                    TEXT.Text = "";
                    TEXT.Text += datum + "\n\n\n" + temp_nizka + "\n\n\n" + temp_visoka + "\n\n\n" + napoved;
                    break;
                }

            }

        }
       

        public string POGLEJ_DATUM(ref char[] datum_array)
        {
            string vrni = "";            //<th scope="row" style="background-color:#f6f7f8"><a href="http://www.accuweather.com/sl/si/ljubljana/299198/daily-weather-forecast/299198?day=1">čet<br />12.3.2015</a></th>            for(int i = 0; i < datum_array.Length; i++)
            {
                if(datum_array[i]=='<' && datum_array[i+1]=='b' &&datum_array[i+2]=='r' && datum_array[i+3]==' ' && datum_array[i+4]=='/'&&datum_array[i+5]=='>')
                {
                    i+=6;
                    for(;i<datum_array.Length;i++)
                    {
                        if (datum_array[i] == '<') break;
                        else vrni += datum_array[i];
                    }
                    break;
                }
            }
            return vrni;
        }

        public string POGLEJ_TEMP_VISOKA(string temp_visoka_array)
        {
            //<td style="font-weight:bold;">11&#176;</td>
            temp_visoka_array = temp_visoka_array.Replace("<td style=\"font-weight:bold;\">", "");
            temp_visoka_array = temp_visoka_array.Replace("</td>", "");
            temp_visoka_array = temp_visoka_array.Replace("&#176;", "°C");
            return temp_visoka_array;
        }

        public string POGLEJ_TEMP_NIZKA(string temp_nizka_array)
        {
            //td>-3&#176;</td>
            temp_nizka_array = temp_nizka_array.Replace("<td>", "");
            temp_nizka_array = temp_nizka_array.Replace("</td>", "");
            temp_nizka_array = temp_nizka_array.Replace("&#176;", "°C");
            return temp_nizka_array;

        }

        public string POGLEJ_SLIKA(ref char[] slika_in_napoved_array)
        {
            //<img class="lt" style="margin-right:7px;" src="http://vortex.accuweather.com/adc2010/images/icons-numbered/04-h.png" width="45" height="30" />Pretrgana oblačnost
            string vrni = "";
            for(int i = 0; i < slika_in_napoved_array.Length; i++)
            {
                if(slika_in_napoved_array[i]=='s' && slika_in_napoved_array[i+1] == 'r' && slika_in_napoved_array[i+2] == 'c')
                {
                    i += 5;
                    for(;i<slika_in_napoved_array.Length; i++)
                    {
                        if (slika_in_napoved_array[i] == '"') break;
                        else vrni += slika_in_napoved_array[i];
                    }
                    break;
                }
            }
            return vrni;

        }

        public string POGLEJ_NAPOVED(ref char[] slika_in_napoved_array)
        {
            //<img class="lt" style="margin-right:7px;" src="http://vortex.accuweather.com/adc2010/images/icons-numbered/04-h.png" width="45" height="30" />Pretrgana oblačnost
            string vrni = "";
            for(int i = slika_in_napoved_array.Length-1; i>=0; i--)
            {
                if (slika_in_napoved_array[i] == '>') break;
                else vrni += slika_in_napoved_array[i];
            }
            vrni = OBRNI(vrni);
            return vrni;

        }

        public static string OBRNI(string s) //OBRNE STRING
        {
            char[] poljeznakov = s.ToCharArray();
            Array.Reverse(poljeznakov);
            return new string(poljeznakov);
        }


    }
}