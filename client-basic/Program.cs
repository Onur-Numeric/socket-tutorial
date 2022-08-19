using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Oxy.SocketTutorial.Utils;

namespace client_basic
{
    class Program
    {
        public static string HostName = Dns.GetHostName();
        public static int Port = 8080;

        static void Main(string[] args)
        {
            SocketClient client = new SocketClient("127.0.0.1", 8080);
            client.connect();
        }
    }
}