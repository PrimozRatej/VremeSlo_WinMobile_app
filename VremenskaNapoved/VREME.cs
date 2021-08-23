using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VremenskaNapoved
{
    public class VREME
    {
        string datum, napoved;
        Uri slika;
        int temperatura_najnizja, temperatura_najvisja;

        public VREME(string datum, string napoved, Uri slika, int temperatura_najnizja, int temperatura_najvisja)
        {
            this.datum = datum;
            //
        }
    }
}
