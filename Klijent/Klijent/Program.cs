using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Services.CitanjePoruka;
using Services.PisanjePoruka;
using Services.SlanjePoruka;
namespace Klijent
{
    public class Program
    {

        static void Main(string[] args)
        {
            #region inicijalizacija
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 50250);
            EndPoint serverEndPoint = (EndPoint)serverEP;
            
            TcpCitanje tcpCitanje = new TcpCitanje();
            TcpPisanje tcpPisanje = new TcpPisanje();
            UdpPisanje udpPisanje = new UdpPisanje();
            UdpCitanje udpCitanje = new UdpCitanje();
            #endregion


            #region prijava
            string poruka;
            string poruka2;
            Console.WriteLine("Prijavite se na server:[PRIJAVA:ime, niz igara koje zelite da igrate]");
            while (true)
            {
                

                poruka = Console.ReadLine();
                udpPisanje.PosaljiPoruku(udpSocket, poruka, serverEP);
                
                poruka2 = udpCitanje.ProcitajPoruku(udpSocket, ref serverEndPoint); 
                Console.WriteLine(poruka2);
                if (poruka2.Contains("GRESKA"))
                {
                    continue;
                }
                else
                {
                    break;
                }
                
            }
            poruka=poruka.Substring("PRIJAVA:".Length).Trim();
            string [] delovi2 = poruka.Split(',');
            string[] igre = delovi2.Skip(1).Select(i => i.ToLower().Trim()).ToArray();
            #endregion


            #region povezivanje na TCP server
            string[] delovi=poruka2.Split(':');
            IPEndPoint serverTCPEP = new IPEndPoint(IPAddress.Parse(delovi[1].Trim()), int.Parse(delovi[2]));
            tcpSocket.Connect(serverTCPEP);
            Console.WriteLine($"Uspesno ste se povezali na server TCP na adresi {serverTCPEP.Address} i portu {serverTCPEP.Port}");
            string poruka3= tcpCitanje.ProcitajPoruku(tcpSocket);
            Console.WriteLine(poruka3);
            string poruka4 = tcpCitanje.ProcitajPoruku(tcpSocket);
            Console.WriteLine(poruka4);

            while (true)
            {
                
                string spreman= Console.ReadLine();
                tcpPisanje.PosaljiPoruku(tcpSocket, spreman);
                string odgovor = tcpCitanje.ProcitajPoruku(tcpSocket);
                if (odgovor.Contains("GRESKA"))
                {
                    Console.WriteLine(odgovor);
                    continue;
                }
              
                else
                {
                    Console.WriteLine(odgovor);
                    break;
                }
            }
            #endregion


            #region slagalica

           
            if (igre.Contains("sl"))
            {
                string p1, p3;
                string porukaSlagalica = "Dobrodošli u igru Slagalica! Potrebno je od ponudjenih slova sklopiti najduzu mogucu rec";
                Console.WriteLine(porukaSlagalica);
                
                p1=tcpCitanje.ProcitajPoruku(tcpSocket);
                Console.WriteLine(p1);
                string res= Console.ReadLine();
                tcpPisanje.PosaljiPoruku(tcpSocket, res);

                p3= tcpCitanje.ProcitajPoruku(tcpSocket);
                Console.WriteLine(p3);
            }
            #endregion


            #region skocko
            if (igre.Contains("sk"))
            {
                string p1, p2;
                string porukaSkocko = "Dobrodošli u igru Skocko! Potrebno je pogoditi kombinaciju 4 simbola(H, T, P, K, S, Z) imate 6 pokusaja";
                Console.WriteLine(porukaSkocko);
                for (int i = 1; i <= 6; i++)
                {
                    
                    Console.Write($"pokusaj {i}:");
                    p1 = Console.ReadLine();
                
                        
                    tcpPisanje.PosaljiPoruku(tcpSocket, p1);
                    p2 = tcpCitanje.ProcitajPoruku(tcpSocket);
                    Console.WriteLine(p2);
                    if (p2.Contains("GRESKA"))
                    {
                        i--;
                        continue;
                    }
                    
                    if (p2.Contains("Čestitamo"))
                    {
                        break;
                    }
                }
                string p3 = tcpCitanje.ProcitajPoruku(tcpSocket);
                Console.WriteLine(p3);

            }
            #endregion


            #region koznazna
            if (igre.Contains("kzz"))
            {
                string p1, p2, p3;
                string porukaKoZnaZna = "Dobrodošli u igru Ko zna zna! odgovarate na 5 pitanja upisivanjem rednog broja(TACAN ODGOVOR=10 poena NETACAN=-5 poena";
                Console.WriteLine(porukaKoZnaZna);
                for (int i = 1; i <= 5; i++)
                {
                    p1 = tcpCitanje.ProcitajPoruku(tcpSocket);
                    Console.WriteLine(p1);
                    p2 = Console.ReadLine();
                    tcpPisanje.PosaljiPoruku(tcpSocket, p2);
                    p3 = tcpCitanje.ProcitajPoruku(tcpSocket);
                    Console.WriteLine(p3);
                }
                string p4 = tcpCitanje.ProcitajPoruku(tcpSocket);
                Console.WriteLine(p4);
            }

            #endregion


            #region kraj
            string suma = tcpCitanje.ProcitajPoruku(tcpSocket);
            Console.WriteLine(suma);

            Console.ReadLine();

            tcpSocket.Close();
            udpSocket.Close();
            #endregion

        }
    }
}
