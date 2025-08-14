using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Services.PisanjePoruka
{
    public class TcpPisanje
    {
        public bool PosaljiPoruku(Socket socket, string poruka)
        {
            bool poslato;
            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {
                socket.Send(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None);
                poslato = true;
            }
            catch (Exception)
            {
                poslato= false;
            }

            return poslato;
        }
    }
}
