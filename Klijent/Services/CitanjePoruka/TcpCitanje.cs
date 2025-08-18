using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


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

