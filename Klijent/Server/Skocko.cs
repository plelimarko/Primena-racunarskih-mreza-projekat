using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Skocko
    {
        private static Random rng = new Random();

        public string TraženaKombinacija { get; private set; } 
        public string TekućaKombinacija { get; set; }

        private char[] simboli = { 'H', 'T', 'P', 'K', 'S', 'Z' };

        public Skocko()
        {
            TraženaKombinacija= string.Empty;
            TekućaKombinacija = string.Empty;
        }

        // Generiše 4 simbola
        public void GenerišiKombinaciju()
        {
            char[] kombinacija = new char[4];
            for (int i = 0; i < 4; i++)
                kombinacija[i] = simboli[rng.Next(simboli.Length)];
            TraženaKombinacija = new string(kombinacija);
        }

        // Provera pokušaja → vraća opis i broj poena
        public (string opis, int poeni) ProveriKombinaciju(int redniPokusaj)
        {
            TekućaKombinacija = TekućaKombinacija.ToUpper();

            int naMestu = 0;
            int uKombinaciji = 0;

            bool[] pogodjeniTarget = new bool[4];
            bool[] pogodjeniGuess = new bool[4];

            // prvo: tačno mesto
            for (int i = 0; i < 4; i++)
            {
                if (TekućaKombinacija[i] == TraženaKombinacija[i])
                {
                    naMestu++;
                    pogodjeniTarget[i] = true;
                    pogodjeniGuess[i] = true;
                }
            }

            // drugo: postoji u kombinaciji ali ne na istom mestu
            for (int i = 0; i < 4; i++)
            {
                if (!pogodjeniGuess[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!pogodjeniTarget[j] && TekućaKombinacija[i] == TraženaKombinacija[j])
                        {
                            uKombinaciji++;
                            pogodjeniTarget[j] = true;
                            break;
                        }
                    }
                }
            }



            string opis;
            int poeni = 0;
            if (naMestu == 4)
            {
              opis="Čestitamo! Pogodili ste kombinaciju!";
                switch (redniPokusaj)
                {
                    case 1: poeni = 30; break;
                    case 2: poeni = 25; break;
                    case 3: poeni = 20; break;
                    case 4: poeni = 15; break;
                    case 5: poeni = 10; break;
                    case 6: poeni = 10; break;
                }

            }
            else {
                opis = $"{naMestu} na mestu, {uKombinaciji} u kombinaciji ali nisu na mestu";
            }

                

            return (opis, poeni);
        }
    }
}
