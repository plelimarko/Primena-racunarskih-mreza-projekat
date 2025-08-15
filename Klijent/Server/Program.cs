using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Services.PisanjePoruka;
using Services.CitanjePoruka;

using Services.SlanjePoruka;

namespace Server { 

    public class Program
    {
        static void Main(string[] args)
        {

            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint posiljaocEP = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 50250);
            tcpSocket.Bind(serverEP);
            udpSocket.Bind(serverEP);

            Console.WriteLine($"PRISTUPNA UTICNICA: {serverEP}");
            Console.WriteLine($"INFO UTICNICA: {serverEP}");
            Console.WriteLine();
          
            TcpCitanje tcpCitanje = new TcpCitanje();
            UdpCitanje udpCitanje = new UdpCitanje();
            TcpPisanje tcpPisanje = new TcpPisanje();
            UdpPisanje udpPisanje = new UdpPisanje();

            string poruka=udpCitanje.ProcitajPoruku(udpSocket, ref posiljaocEP);
            Console.WriteLine(poruka);
            Console.ReadLine();



            tcpSocket.Close();
            udpSocket.Close();
            }
        }
    }

