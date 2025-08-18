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
            #endregion

            #region prijava
            string[]dozvoljeno = { "sl", "sk", "kzz" };
            string odgovor;
            Igrac igrac = null;
            bool kraj = false;
            string poruka;
            string[] delovi;
            string[] igre = null; 

            while (true)
            {
                poruka = udpCitanje.ProcitajPoruku(udpSocket, ref posiljaocEP);
                Console.WriteLine(poruka);
                
                if (!poruka.StartsWith("PRIJAVA:"))
                {
                    odgovor = "GRESKA: Pogresan format poruke. Ocekivano: PRIJAVA:ime, niz igara koje zelite da igrate";
                }
                else
                {
                    string ostatak = poruka.Substring("PRIJAVA:".Length).Trim();
                    delovi = ostatak.Split(',');
                    if (delovi.Length < 2)
                    {
                        odgovor = "GRESKA: Pogresan format poruke. Ocekivano: PRIJAVA:ime, niz igara koje zelite da igrate";
                    }
                    else if (delovi.Length > 4)
                    {
                        odgovor = "GRESKA: Previse igara. Maksimalno 3 igre.";
                    }
                    else
                    {

                        string ime = delovi[0];
                        igre = delovi.Skip(1).Select(i => i.ToLower().Trim()).ToArray();

                        if (!igre.All(i => dozvoljeno.Contains(i)))
                        {
                            odgovor = "GRESKA: Dozvoljene igre su samo sl, sk, kzz";
                        }
                        else if (igre.Count(i => i == "sk") > 2)
                        {
                            odgovor = "GRESKA: 'sk' se može navesti najviše 2 puta";
                        }
                        else
                        {
                            string endpoint= serverTCPEP.ToString();
                            odgovor = $"TCP: {endpoint}";
                            Console.WriteLine($"Igrač {ime}, igre: {string.Join(",", igre)}");
                            var random = new Random();
                            int id = random.Next(1000, 9999);
                            int[] niz = new int[igre.Length];
                            igrac = new Igrac(id, ime, niz);
                            kraj = true;

                        }


                    }

                }
                udpPisanje.PosaljiPoruku(udpSocket, odgovor, posiljaocEP);
                if (kraj)
                {
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
                    Console.WriteLine("Igrač je spreman, započinjemo igru.");
                    break;
                }
                else
                {
                    tcpPisanje.PosaljiPoruku(acceptedsocket, "GRESKA: Niste poslali SPREMAN, pokušajte ponovo.");
                }

            }
            #endregion


            if (igre.Contains("sl"))
            {
                
                

                Slagalica slagalica = new Slagalica();
                slagalica.GenerišiSlova();
                tcpPisanje.PosaljiPoruku(acceptedsocket,$"Ponuđena slova: {slagalica.PonuđenaSlova}");
               

                string resenje = tcpCitanje.ProcitajPoruku(acceptedsocket);
                Console.WriteLine($"Igrač je poslao reč: {resenje}");
                slagalica.SastavljenaReč = resenje;
                int poeni = slagalica.ProveriReč();
                if(poeni > 0)
                {
                    igrac.niz[0] = poeni;
                    tcpPisanje.PosaljiPoruku(acceptedsocket, $"Čestitamo! Osvojili ste {poeni} poena.");
                }
                else
                {
                    igrac.niz[0] = 0;
                    tcpPisanje.PosaljiPoruku(acceptedsocket, "Nažalost, reč nije validna. Niste osvojili poene.");
                }
            }



            Console.ReadLine();
            tcpSocket.Close();
            udpSocket.Close();
            }
        }
    }

