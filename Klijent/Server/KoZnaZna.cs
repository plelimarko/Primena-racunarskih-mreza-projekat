using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class KoZnaZna
    {

        
        
            public string TekucePitanje { get; set; }
            public int TacanOdgovor { get; set; }
            public Dictionary<string, int> Pitanja { get; set; }
            
            private Random rnd = new Random();
            
            private int poslednjiOdgovor = -1; // čuva poslednji tačan odgovor da se ne ponavlja
        public KoZnaZna()
            {
                UcitajPitanja();
            }

            // Učitavanje pitanja u rečnik
            private void UcitajPitanja()
            {
                Pitanja = new Dictionary<string, int>
            {
            { "Koje godine je pao Berlinski zid?\n1 - 1961\n2 - 1989\n3 - kad god da je objavljen film 'Atomic Blonde'", 2 },
            { "Ko je napisao 'Na Drini ćuprija'?\n1 - Ivo Andrić\n2 - Meša Selimović\n3 - Miloš Crnjanski", 1 },
            { "Najveći okean na svetu je?\n1 - Atlantski\n2 - Tihi\n3 - Indijski", 2 },
            { "Prvi čovek u svemiru bio je?\n1 - Neil Armstrong\n2 - Jurij Gagarin\n3 - Buzz Aldrin", 2 },
            { "Glavni grad Australije je?\n1 - Sidnej\n2 - Kanbera\n3 - Melburn", 2 },
            { "Najduža reka na svetu je?\n1 - Nil\n2 - Amazona\n3 - Misisipi", 2 },
            { "Ko je autor romana 'Ana Karenjina'?\n1 - Fjodor Dostojevski\n2 - Lav Tolstoj\n3 - Anton Čehov", 2 },
            { "Koja planeta je najbliža Suncu?\n1 - Merkur\n2 - Venera\n3 - Mars", 1 },
            { "Koja životinja je simbol Australije?\n1 - Klokan\n2 - Koala\n3 - Emu", 1 },
            { "Koliko hromozoma ima čovek?\n1 - 23\n2 - 46\n3 - 48", 2 },
            { "Ko je naslikao 'Mona Lizu'?\n1 - Leonardo da Vinči\n2 - Mikelanđelo\n3 - Rafael", 1 },
            { "Koja država je osvojila Svetsko prvenstvo u fudbalu 2018?\n1 - Brazil\n2 - Nemačka\n3 - Francuska", 3 },
            { "Najviši vrh na svetu je?\n1 - Mont Everest\n2 - K2\n3 - Kangčendženga", 1 },
            { "Koja hemijska oznaka pripada zlatu?\n1 - Ag\n2 - Au\n3 - Zn", 2 },
            { "Koliko kontinenata postoji na Zemlji?\n1 - 5\n2 - 6\n3 - 7", 3 },
            { "Prvi predsednik SAD bio je?\n1 - Abraham Linkoln\n2 - Džordž Vašington\n3 - Tomas Džeferson", 2 },
            { "Ko je autor dela 'Prokleta avlija'?\n1 - Ivo Andrić\n2 - Meša Selimović\n3 - Branko Ćopić", 1 },
            { "Koji metal je u tečnom stanju na sobnoj temperaturi?\n1 - Živa\n2 - Natrijum\n3 - Kalaj", 1 },
            { "Koja država ima najviše stanovnika?\n1 - SAD\n2 - Indija\n3 - Kina", 3 },
            { "Koja krvna grupa se smatra univerzalnim davaocem?\n1 - 0-\n2 - AB+\n3 - A+", 1 },
            { "Koja je najveća pustinja na svetu?\n1 - Sahara\n2 - Antarktik\n3 - Gobi", 2 },
            { "Glavni grad Kanade je?\n1 - Otava\n2 - Toronto\n3 - Montreal", 1 },
            { "Koja ptica ne može da leti?\n1 - Noj\n2 - Sova\n3 - Orao", 1 },
            { "Ko je izumeo sijalicu?\n1 - Nikola Tesla\n2 - Tomas Edison\n3 - Aleksandar Grejem Bel", 2 },
            { "Koja je najmanja država na svetu?\n1 - Vatikan\n2 - Monako\n3 - San Marino", 1 },
            { "Koja je hemijska oznaka za kiseonik?\n1 - O\n2 - K\n3 - Ox", 1 },
            { "Koja je najnaseljenija evropska država?\n1 - Nemačka\n2 - Rusija\n3 - Francuska", 2 },
            { "Koja ptica je simbol mira?\n1 - Golub\n2 - Orao\n3 - Labud", 1 },
            { "Ko je napisao tragediju 'Hamlet'?\n1 - Vilijam Šekspir\n2 - Čarls Dikens\n3 - Džojs", 1 },
            { "Koji kontinent se naziva 'crni kontinent'?\n1 - Azija\n2 - Afrika\n3 - Australija", 2 },
            { "Prvi čovek na Mesecu bio je?\n1 - Jurij Gagarin\n2 - Neil Armstrong\n3 - Majkl Kolins", 2 },
            { "Koja država je poznata po piramidama?\n1 - Egipat\n2 - Meksiko\n3 - Peru", 1 },
            { "Ko je komponovao 'Odu radosti'?\n1 - Mocart\n2 - Betoven\n3 - Bah", 2 },
            { "Glavni grad Japana je?\n1 - Tokio\n2 - Kjoto\n3 - Osaka", 1 },
            { "Koji gas udišemo da bismo preživeli?\n1 - Ugljen-dioksid\n2 - Azot\n3 - Kiseonik", 3 },
            { "Koja je najveća životinja na svetu?\n1 - Plavi kit\n2 - Slon\n3 - Žirafa", 1 },
            { "Koja država se prostire na dva kontinenta?\n1 - Turska\n2 - Egipat\n3 - Rusija", 1 },
            { "Koji sport je poznat kao 'kralj sportova'?\n1 - Fudbal\n2 - Košarka\n3 - Tenis", 1 },
            { "Koja je valuta Japana?\n1 - Jen\n2 - Juan\n3 - Won", 1 },
            { "Najpoznatiji grčki filozof koji je bio učitelj Aleksandra Velikog je?\n1 - Platon\n2 - Aristotel\n3 - Sokrat", 2 },
            { "Koja je najviša zgrada na svetu (2023)?\n1 - Burdž Kalifa\n2 - Šangaj Tauer\n3 - Abraj Al Bait", 1 }
        };
            }

            // Biranje nasumičnog pitanja
            public string IzaberiPitanje()
            {

            KeyValuePair<string, int> element;
            do
            {
                int index = rnd.Next(Pitanja.Count);
                element = Pitanja.ElementAt(index);
            }
            while (element.Value == poslednjiOdgovor); // ponavlja dok ne dobije drugi tačan odgovor

            TekucePitanje = element.Key;
            TacanOdgovor = element.Value;
            poslednjiOdgovor = TacanOdgovor;

            return TekucePitanje;

        }

            // Provera odgovora igrača
            public bool ProveriOdgovor(int odgovor)
            {
                if (odgovor == TacanOdgovor)
                    return true;   // tačan = +10
                else
                    return false;   // netačan = -5
            }
        }

    }

