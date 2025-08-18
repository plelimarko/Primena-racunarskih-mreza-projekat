using System;
using System.Text;
using System.Net.Sockets;

namespace Services.PisanjePoruka
{

    public class TcpPisanje
    {
        
            public bool PosaljiPoruku(Socket socket, string poruka)
            {
                bool isSent;
                byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

                try
                {
                    socket.Send(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None);
                    isSent = true;
                }
                catch (Exception)
                {
                    isSent = false;
                }

                return isSent;
            }
        }
    }


