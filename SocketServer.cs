using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{

    internal class SocketServer
    {
        public SocketServer()
        {
            IPEndPoint endpoint = GetServerIp();
            StartServer(endpoint);

        }
        private void StartServer (IPEndPoint endpoint)
        {
            Socket listener = new(
                endpoint.AddressFamily, 
                SocketType.Stream,
                ProtocolType.Tcp);
            listener.Bind(endpoint);
            listener.Listen(10);

            Socket handler = listener.Accept();

            string msg = null;
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRec = handler.Receive(buffer);
                msg += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                if (msg.IndexOf("<EOM>") > -1) break;
            }
            Console.WriteLine($"Message: {msg}");
        }

        private IPEndPoint GetServerIp()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(strHostName);

            List<IPAddress> addrlist = new();
            
            int counter = 0;

            foreach (var item in host.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine($"{counter++}{item.ToString()}");
                    addrlist.Add(item);

                }
            }
            int temp = 0;
            do {Console.Write("Select server IP: ");}
            while (!int.TryParse(Console.ReadLine(), out temp));
            
            IPAddress addr = addrlist[temp];
            IPEndPoint localEndPoint = new IPEndPoint(addr, 11000);
            return localEndPoint;
        }
    }
}
