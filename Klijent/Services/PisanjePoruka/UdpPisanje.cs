using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Services.SlanjePoruka
{
    public class UdpPisanje
    {
        public bool PosaljiPoruku(Socket socket, string poruka, EndPoint primaocEP)
        {
            bool poslato;
            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {
                socket.SendTo(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None, primaocEP);
                poslato = true;
            }
            catch (Exception e)
            {
                poslato = false;
                Console.WriteLine($"Doslo je do greske: {e.Message}");
            }

            return poslato;
        }
    }
}
