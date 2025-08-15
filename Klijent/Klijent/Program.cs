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
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 50250);

           string poruka=Console.ReadLine();
           UdpPisanje udpPisanje = new UdpPisanje();
           bool poslato = udpPisanje.PosaljiPoruku(udpSocket, poruka, serverEP);
            if (poslato)
            {
                Console.WriteLine("Poruka je poslata preko UDP protokola.");
            }
            else
            {
                Console.WriteLine("Doslo je do greske prilikom slanja poruke preko UDP protokola.");
            }

                tcpSocket.Close();
                udpSocket.Close();




        }
    }
}
