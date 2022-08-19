using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Oxy.SocketTutorial.Utils
{
    public class SocketClient
    {
        public string host { get; private set; }
        public int port { get; private set; }
        public TextWriter outWriter { get; private set; }

        public SocketClient(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.outWriter = Console.Out;
        }

        public SocketClient(string host, int port, TextWriter outWriter)
        {
            this.host = host;
            this.port = port;
            this.outWriter = outWriter;
        }

        public void connect()
        {
            byte[] buffer = new byte[1024];

            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
                IPAddress iPAddress = iPHostEntry.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(iPAddress, port);

                Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    outWriter.WriteLine("Connecting to remote host...");
                    socket.Connect(remoteEndPoint);
                    outWriter.WriteLine("Client is connected");

                    outWriter.WriteLine("Sending request");
                    string request = "GET /time HTTP/1.1\r\n\r\n";
                    socket.Send(Encoding.UTF8.GetBytes(request));

                    StringBuilder responseBuilder = new StringBuilder();

                    outWriter.Write("Reading first part of response...\t");
                    int nbytes1 = socket.Receive(buffer);
                    outWriter.WriteLine($"Received {nbytes1} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes1));

                    outWriter.Write("Reading second part of response...\t");
                    int nbytes2 = socket.Receive(buffer);
                    outWriter.WriteLine($"Received {nbytes2} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes2));

                    outWriter.WriteLine();
                    outWriter.WriteLine(responseBuilder.ToString());

                    outWriter.WriteLine();
                    outWriter.WriteLine("Closing Socket");
                    socket.Close();
                }
                catch (ArgumentNullException ex)
                {
                    outWriter.WriteLine("ArgumentNullException: {0}", ex.ToString());
                }
                catch (SocketException ex)
                {
                    outWriter.WriteLine("SocketException: {0}", ex.ToString());
                }
                catch (Exception ex)
                {
                    outWriter.WriteLine("Unexpected Exception: {0}", ex.ToString());
                }

            }
            catch (Exception ex)
            {
                outWriter.WriteLine(ex.ToString());
            }
        }

    }
}