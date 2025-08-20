using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Services.CitanjePoruka;
using Services.PisanjePoruka;
using Services.SlanjePoruka;
using Server.servisi;
using static System.Net.Mime.MediaTypeNames;

namespace Server {

    public class Program
    {
        static void Main(string[] args)
        {
            #region inicijalizacija
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint posiljaocEP = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 50250);
            IPEndPoint serverTCPEP = new IPEndPoint(IPAddress.Loopback, 50150);
            udpSocket.Bind(serverEP);

            TcpCitanje tcpCitanje = new TcpCitanje();
            UdpCitanje udpCitanje = new UdpCitanje();
            TcpPisanje tcpPisanje = new TcpPisanje();
            UdpPisanje udpPisanje = new UdpPisanje();
            
            Igrac igrac = null;
            Provera provera = new Provera();
            string poruka;
            string[] igre = null;
            int slot = -1;
            #endregion

            #region prijava
            while (true)
            {
                poruka = udpCitanje.ProcitajPoruku(udpSocket, ref posiljaocEP);
                Console.WriteLine(poruka);
                var (isValid, odgovor) = provera.ProveriPrijavu(poruka, serverTCPEP, ref igrac, ref igre);
                if(!isValid)
                {
                    udpPisanje.PosaljiPoruku(udpSocket, odgovor, posiljaocEP);
                    continue;
                }
                else
                {
                    udpPisanje.PosaljiPoruku(udpSocket, odgovor, posiljaocEP);
                    Console.WriteLine($"Igrač {igrac.Name} je uspešno prijavljen.");
                    break;
                }
            }
            #endregion

            #region povezivanje na TCP
            tcpSocket.Bind(serverTCPEP);
            tcpSocket.Listen(1);
            Socket acceptedsocket = tcpSocket.Accept();
            Console.WriteLine("TCP konekcija uspostavljena.");
            tcpPisanje.PosaljiPoruku(acceptedsocket, $"Dobrodošli u trening igru kviza TV Slagalica, današnji takmičar je {igrac.Name}");
            tcpPisanje.PosaljiPoruku(acceptedsocket, "Posaljite poruku SPREMAN da zapocnete kviz");

            while (true)
            {
                string spreman = tcpCitanje.ProcitajPoruku(acceptedsocket);
                if (spreman.ToUpper() == "SPREMAN")
                {
                    tcpPisanje.PosaljiPoruku(acceptedsocket, "U redu, spremni ste za igru.");
                    Console.WriteLine("Igrač je spreman, započinjemo igru.");
                    break;
                }
                else
                {
                    tcpPisanje.PosaljiPoruku(acceptedsocket, "GRESKA: Niste poslali SPREMAN, pokušajte ponovo.");
                }

            }
            #endregion

            #region slagalica
            if (igre.Contains("sl"))
            {


                Console.WriteLine("Igra Slagalica je pokrenuta.");
                Slagalica slagalica = new Slagalica();
                slagalica.GenerišiSlova();
                Console.WriteLine($"Ponuđena slova: {slagalica.PonuđenaSlova}");
                tcpPisanje.PosaljiPoruku(acceptedsocket, $"Ponuđena slova: {slagalica.PonuđenaSlova}");


                string resenje = tcpCitanje.ProcitajPoruku(acceptedsocket);
                Console.WriteLine($"Igrač je poslao reč: {resenje}");
                slagalica.SastavljenaReč = resenje;
                int poeni = slagalica.ProveriReč();
                if (poeni > 0)
                {
                    igrac.niz[++slot] = poeni;
                    tcpPisanje.PosaljiPoruku(acceptedsocket, $"Čestitamo! Osvojili ste {poeni} poena.");
                    Console.WriteLine($"Igrač je sastavio reč i osvojio {poeni} poena.");
                }
                else
                {
                    igrac.niz[++slot] = 0;
                    tcpPisanje.PosaljiPoruku(acceptedsocket, "Nažalost, reč nije validna. Niste osvojili poene.");
                    Console.WriteLine("Igrač nije sastavio validnu reč.");
                }
            }
            #endregion

            #region skocko
            for (int j = 0; j < igre.Length; j++)
            {
                string igra = igre[j];

                if (igra == "sk")
                {
                    Console.WriteLine("Igra Skocko je pokrenuta.");
                    Skocko skocko = new Skocko();
                    skocko.GenerišiKombinaciju();
                    Console.WriteLine(skocko.TraženaKombinacija);
                    var rezultat = (opis: "", poeni: 0);
                    for (int i = 1; i <= 6; i++)
                    {
                        skocko.TekućaKombinacija = tcpCitanje.ProcitajPoruku(acceptedsocket);
                        Console.WriteLine($"Igrač je poslao pokušaj {i}: {skocko.TekućaKombinacija}");
                        var proveraRezultat = provera.ProveraSkocko(skocko.TekućaKombinacija);
                        if (!proveraRezultat.Item1)
                        {
                            tcpPisanje.PosaljiPoruku(acceptedsocket, proveraRezultat.Item2);
                            Console.WriteLine(proveraRezultat.Item2);
                            i--; // ponovi pokušaj
                            continue;
                        }
                        rezultat = skocko.ProveriKombinaciju(i);
                        tcpPisanje.PosaljiPoruku(acceptedsocket, rezultat.opis);
                        if (rezultat.poeni != 0)
                        {

                            break;
                        }

                    }
                    if (rezultat.poeni > 0)
                    {
                        igrac.niz[++slot] = rezultat.poeni;
                        tcpPisanje.PosaljiPoruku(acceptedsocket, $"Osvojili ste {rezultat.poeni} poena.");
                        Console.WriteLine($"Igrač je pogodio kombinaciju i osvojio je {rezultat.poeni} poena");
                    }
                    else
                    {
                        igrac.niz[++slot] = 0;
                        tcpPisanje.PosaljiPoruku(acceptedsocket, "Nažalost, niste pogodili kombinaciju. Niste osvojili poene.");
                        Console.WriteLine("Igrač nije pogodio kombinaciju.");
                    }


                }
            }

            #endregion

            #region koznazna
            if (igre.Contains("kzz"))
            {
                KoZnaZna koZnaZna = new KoZnaZna();
                Console.WriteLine("Igra Ko zna zna je pokrenuta.");
                int poeni = 0;

                for (int i = 0; i < 5; i++)
                {
                    string pitanje = koZnaZna.IzaberiPitanje();
                    Console.WriteLine($"Pitanje {i + 1}: {pitanje} \nodgovor: {koZnaZna.TacanOdgovor}");
                    tcpPisanje.PosaljiPoruku(acceptedsocket, pitanje);
                    string odgovor1 = tcpCitanje.ProcitajPoruku(acceptedsocket);
                    int brojOdgovora = int.Parse(odgovor1);
                    Console.WriteLine($"Igrač je poslao odgovor: {brojOdgovora}");
                    if (koZnaZna.ProveriOdgovor(brojOdgovora))
                    {
                        poeni += 10;
                        tcpPisanje.PosaljiPoruku(acceptedsocket, "Čestitamo! Tačan odgovor, osvojili ste 10 poena.");
                        Console.WriteLine($"Igrač je tačno odgovorio na pitanje {i + 1} i osvojio 10 poena.");
                    }
                    else
                    {
                        poeni -= 5;
                        tcpPisanje.PosaljiPoruku(acceptedsocket, "Nažalost, netačan odgovor. Izgubili ste 5 poena.");
                        Console.WriteLine($"Igrač je netačno odgovorio na pitanje {i + 1} i izgubio 5 poena.");
                    }
                }
                igrac.niz[++slot] = poeni;
                Console.WriteLine($"Igrač je završio igru Ko zna zna i osvojio ukupno {poeni} poena.");
                tcpPisanje.PosaljiPoruku(acceptedsocket, $"Kraj igre Ko zna zna. Osvojili ste ukupno {poeni} poena.");
            }
            #endregion

            #region kraj
            int suma = 0;
            foreach (int poen in igrac.niz)
            {
                suma += poen;
            }
            Console.WriteLine($"Igrač {igrac.Name} je završio igru sa ukupno {suma} poena.");
            tcpPisanje.PosaljiPoruku(acceptedsocket, $"Kraj igre. Ukupno ste osvojili {suma} poena.");

            Console.ReadLine();
            tcpSocket.Close();
            udpSocket.Close();
            #endregion
        }
    }
}

