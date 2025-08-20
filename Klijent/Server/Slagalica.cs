using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Slagalica
    {

        public string PonuđenaSlova { get; set; } 
        public string SastavljenaReč { get; set; }

        private static Random rng = new Random();
        public Slagalica()
        {
           PonuđenaSlova = "";
          SastavljenaReč = "";
        }

       

        public void GenerišiSlova()
        {
            string slova = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] svaSlova = slova.ToCharArray();
            List<char> izabrana = new List<char>();

            char isto = svaSlova[rng.Next(svaSlova.Length)];
            izabrana.AddRange(Enumerable.Repeat(isto, 3));

            for (int i = 0; i < 9; i++)
            {
                izabrana.Add(svaSlova[rng.Next(svaSlova.Length)]);
            }

            izabrana = izabrana.OrderBy(c => rng.Next()).ToList();

            
            PonuđenaSlova = new string(izabrana.ToArray());
        }

        public int ProveriReč()
        {
            List<char> slovaPonudjena = PonuđenaSlova.ToUpper().ToList();
            SastavljenaReč = SastavljenaReč.ToUpper();
            foreach (char c in SastavljenaReč)
            {
                if (slovaPonudjena.Contains(c))
                    slovaPonudjena.Remove(c);
                else
                    return 0;
            }

            return 5 * SastavljenaReč.Length;
        }
    }
}
