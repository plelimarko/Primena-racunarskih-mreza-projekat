using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Services.CitanjePoruka
{
    public class TcpCitanje
    {
        public string ProcitajPoruku(Socket socket)
        {
            string poruka;
            byte[] buffer = new byte[1024];

            try
            {
                int bytesReceived = socket.Receive(buffer);
                poruka = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            }
            catch (Exception e)
            {
                poruka = String.Empty;
            }

            return poruka;
        }
    }
}
