using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VremenskaNapoved
{
    public class BRANJE_PODATKOV
    {
        //-------------------------------------------------------------------------------------------------METODE ZA BRANJE PODATKOV IZ VREME15
        public static string V15_GLEJ_DATUM(char[] znaki, string vrstica)
        {
            string vrni = "";
            if (vrstica.Contains("<font color=blue>"))
            {
                for (int i = 0; i < znaki.Length; i++)
                {
                    if (znaki[i] == 'f' && znaki[i + 1] == 'o' && znaki[i + 2] == 'n' && znaki[i + 3] == 't' && znaki[i + 4] == ' ' && znaki[i + 5] == 'c' &&
                        znaki[i + 6] == 'o' && znaki[i + 7] == 'l' && znaki[i + 8] == 'o' && znaki[i + 9] == 'r' && znaki[i + 10] == '=' &&
                        znaki[i + 11] == 'b' && znaki[i + 12] == 'l' && znaki[i + 13] == 'u' && znaki[i + 14] == 'e' && znaki[i + 15] == '>')
                    {
                        i += 16;
                        for (; i < znaki.Length; i++)
                        {
                            if (znaki[i] == '<')
                            {
                                i += 7;
                                for (; i < znaki.Length; i++)
                                {
                                    if (znaki[i] == '<') break;
                                    else vrni += znaki[i];
                                }
                                break;
                            }
                            else vrni += znaki[i];
                        }
                        break;
                    }
                }
            }



            else if (vrstica.Contains("<font color=red>"))
            {
                for (int i = 0; i < znaki.Length; i++)
                {
                    if (znaki[i] == 'r' && znaki[i + 1] == 'e' && znaki[i + 2] == 'd' && znaki[i + 3] == '>')
                    {
                        i += 4;
                        for (; i < znaki.Length; i++)
                        {
                            if (znaki[i] == '<')
                            {
                                i += 7;
                                for (; i < znaki.Length; i++)
                                {
                                    if (znaki[i] == '<') break;
                                    else vrni += znaki[i];
                                }
                                break;
                            }
                            else vrni += znaki[i];
                        }
                        break;
                    }
                }
            }




            else
            {
                for (int i = 0; i < znaki.Length; i++)
                {
                    if (znaki[i] == '<' && znaki[i + 1] == 'b' && znaki[i + 2] == '>')
                    {
                        i += 3;
                        for (; i < znaki.Length; i++)
                        {
                            if (znaki[i] == '<') break;
                            else vrni += znaki[i];
                        }
                        break;
                    }
                }
            }

            return vrni;

        }

        public static string V15_GLEJ_NAPOVED(char[] znaki)
        {
            string vrni = "";
            //colspan="2" align
            for (int i = znaki.Length - 2; i >= 0; i--)
            {
                if (znaki[i] == '>') break;
                else vrni += znaki[i];
            }
            vrni = OBRNI(vrni);
            return vrni;
        }

        public static int V15_GLEJ_TEMPERATURO_NAJNIZJO(char[] znaki2)
        {
            string vrni2 = "";

            for (int i = 61; i < znaki2.Length; i++)
            {
                if (znaki2[i] == '<') break;
                else vrni2 += znaki2[i];
            }

            char[] nova = vrni2.ToCharArray();
            vrni2 = "";

            for (int i = 0; i < nova.Length; i++)
            {
                if (nova[i] == '&') break;
                else vrni2 += nova[i];
            }

            return (int.Parse(vrni2));


        }

        public static int V15_GLEJ_TEMPERATURO_NAJVISJA(char[] znaki)
        {
            string vrni2 = "";

            for (int i = 0; i < znaki.Length; i++)
            {
                if (znaki[i] == 'c' && znaki[i + 1] == 'o' && znaki[i + 2] == 'l' && znaki[i + 3] == 'o' && znaki[i + 4] == 'r' && znaki[i + 5] == '=' && znaki[i + 6] == 'r' && znaki[i + 7] == 'e' && znaki[i + 8] == 'd' && znaki[i + 9] == '>' && znaki[i + 10] == '<' && znaki[i + 11] == 'b' && znaki[i + 12] == '>')
                {
                    i += 13;
                    for (; i < znaki.Length; i++)
                    {
                        if (znaki[i] == '<') break;
                        else vrni2 += znaki[i];
                    }
                    break;
                }
            }

            char[] nova = vrni2.ToCharArray();
            vrni2 = "";

            for (int i = 0; i < nova.Length; i++)
            {
                if (nova[i] == '&') break;
                else vrni2 += nova[i];
            }

            return (int.Parse(vrni2));

        }

        public static Uri V15_GLEJ_SLIKO(char[] znaki, int ii)
        {
            string vrni = "";
            for (int i = 0; i < znaki.Length; i++)
            {
                if (znaki[i] == 's' && znaki[i + 1] == 'r' && znaki[i + 2] == 'c' && znaki[i + 3] == '=' && znaki[i + 4] == '"')
                {
                    i += 5;
                    for (; i < znaki.Length; i++)
                    {
                        if (znaki[i] == '"') break;
                        else vrni += znaki[i];
                    }
                    break;
                }

            }

            Uri naslov_do_slike = SPREMENI_NASLOV_SLIKE(vrni);

            return naslov_do_slike;
        }








        //-------------------------------------------------------------------------------------------------METODE ZA BRANJE PODATKOV IZ ACCUWEATHER
        public static string ACC_BRANJE_PODATKOV(string[] string_array)
        {

            string_array[0] = string_array[0].Replace("<div class=\"fc\"><span class=\"first\">", "");
            string_array[0] = string_array[0].Replace("<sup>&deg;C</sup>", "");
            string_array[0] = string_array[0].Replace("<p class=\"d\">", "");
            string_array[0] = string_array[0].Replace("width=\"100\" height=\"60\" alt=\"\" /><p>", "");
            string_array[0] = string_array[0].Replace("</p><p", "");
            string_array[0] = string_array[0].Replace("km/h</p></span>", "");
            string_array[0] = string_array[0].Replace("<img src=\"", "");
            string_array[0] = string_array[0].Replace("</p><p", "");
            string_array[0] = string_array[0].Replace("</p", "");
            string_array[0] = string_array[0].Replace("</div><div class=\"fch\"><a id=\"pswfch\" class=\"arr\" href=\"#\" title=\"Klikni za prikaz urne napovedi\">", "");
            string_array[0] = string_array[0].Replace("\t\t", "");
            string_array[0] = string_array[0].Replace("<span class=\"\">", "×××");
            return string_array[0];
        }










        //-------------------------------------------------------------------------------------------------METODE ZA BRANJE PODATKOV IZ ???
        public static string VTK_BERI_DATUM(string X)
        {
            char[] XX = X.ToCharArray();
            X = "";
            for(int i = 0; i < XX.Length; i++)
            {
                if(XX[i] == '>')
                {
                    i++;
                    for(;i<XX.Length;i++)
                    {
                        if (XX[i] == '<') break;
                        else X += XX[i];
                    }
                    break;
                }
            }
            return X;

        }

        public static int VTK_BERI_TEMP_MIN(string X)
        {
            char[] XX = X.ToCharArray();
            X = "";
            for (int i = 0; i < XX.Length; i++)
            {
                if (XX[i] == '>')
                {
                    i++;
                    for (; i < XX.Length; i++)
                    {
                        if (XX[i] == '<') break;
                        else X += XX[i];
                    }
                    break;
                }
            }
            X = X.Replace("&deg;C", "");
            return int.Parse(X);
        }

        public static int VTK_BERI_TEMP_MAX(string X)
        {
            char[] XX = X.ToCharArray();
            X = "";
            for (int i = 0; i < XX.Length; i++)
            {
                if (XX[i] == '>')
                {
                    i++;
                    for (; i < XX.Length; i++)
                    {
                        if (XX[i] == '<') break;
                        else X += XX[i];
                    }
                    break;
                }
            }
            X = X.Replace("&deg;C", "");
            return int.Parse(X);
        }

        public static string VTK_BERI_NAPOVED(string X)
        {
            char[] XX = X.ToCharArray();
            X = "";
            for (int i = 0; i < XX.Length; i++)
            {
                if (XX[i] == '>')
                {
                    i++;
                    for (; i < XX.Length; i++)
                    {
                        if (XX[i] == '<') break;
                        else X += XX[i];
                    }
                    break;
                }
            }
            return X;
        }

        public static string VTK_BERI_SLIKA(string X)
        {
            X= X.Replace("<p><img class='wp-forecast-fc-left' src='", "");
            X= X.Replace("<div class='wp-forecast-curr-left'><img class='wp-forecast-curr-left' src='", "");
            char[] XX = X.ToCharArray();
            X = "";
            for (int i = 0; i < XX.Length; i++ )
            {
                if (XX[i] == '\'') break;
                else X += XX[i];
            }

                return X;
        }









        //-------------------------------------------------------------------------------------------------OSTALE METODE
        public static string OBRNI(string s) //OBRNE STRING
        {
            char[] poljeznakov = s.ToCharArray();
            Array.Reverse(poljeznakov);
            return new string(poljeznakov);
        }

        public static Uri SPREMENI_NASLOV_SLIKE(string vrni)
        {
            char[] vmesna = vrni.ToCharArray();
            string vmesna2 = "SLIKE/SLIKICE/";

            for (int i = 32; i < vmesna.Length; i++)
            {
                vmesna2 += vmesna[i];
            }
            return new Uri(vmesna2, UriKind.Relative);
        }





    }
}
