using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client_basic
{
    class Program
    {
        public static string HostName = Dns.GetHostName();
        public static int Port = 8080;

        static void Main(string[] args)
        {
            byte[] buffer = new byte[1024];

            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(HostName);
                IPAddress iPAddress = iPHostEntry.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(iPAddress, Port);

                Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    Console.WriteLine("Connecting to remote host...");
                    socket.Connect(remoteEndPoint);
                    Console.WriteLine("Client is connected");

                    Console.WriteLine("Sending request");
                    string request = "GET /time HTTP/1.1\r\n\r\n";
                    socket.Send(Encoding.UTF8.GetBytes(request));

                    StringBuilder responseBuilder = new StringBuilder();

                    Console.Write("Reading first part of response...\t");
                    int nbytes1 = socket.Receive(buffer);
                    Console.WriteLine($"Received {nbytes1} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes1));

                    Console.Write("Reading second part of response...\t");
                    int nbytes2 = socket.Receive(buffer);
                    Console.WriteLine($"Received {nbytes2} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes2));

                    Console.WriteLine();
                    Console.WriteLine(responseBuilder.ToString());

                    Console.WriteLine();
                    Console.WriteLine("Closing Socket");
                    socket.Close();
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ex.ToString());
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("SocketException: {0}", ex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected Exception: {0}", ex.ToString());
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}