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


            UdpPisanje udpPisanje = new UdpPisanje();
            UdpCitanje udpCitanje = new UdpCitanje();
            #endregion

            #region prijava
            string poruka;
            Console.WriteLine("Prijavite se na server:[PRIJAVA:ime, niz igara koje zelite da igrate]");
            while (true)
            {
                

                poruka = Console.ReadLine();
                udpPisanje.PosaljiPoruku(udpSocket, poruka, serverEP);
                string poruka2 = udpCitanje.ProcitajPoruku(udpSocket, ref serverEndPoint); 
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
            #endregion

            tcpSocket.Close();
            udpSocket.Close();

        }
    }
}
