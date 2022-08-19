using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Oxy.SocketTutorial.Utils
{
    public class SocketClientAsync
    {
        public string host { get; private set; }
        public int port { get; private set; }
        public TextWriter outWriter { get; private set; }
        public bool debug { get; private set; }

        public SocketClientAsync(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.outWriter = Console.Out;
            this.debug = true;
        }

        public SocketClientAsync(string host, int port, TextWriter outWriter)
        {
            this.host = host;
            this.port = port;
            this.outWriter = outWriter;
            this.debug = true;
        }

        public async Task<string> GetTime()
        {
            byte[] buffer = new byte[1024];

            string response;

            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
                IPAddress iPAddress = iPHostEntry.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(iPAddress, port);

                Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    if(debug) outWriter.WriteLine("Connecting to remote host...");
                    socket.Connect(remoteEndPoint);
                    if (debug) outWriter.WriteLine("Client is connected");

                    if (debug) outWriter.WriteLine("Sending request");
                    string request = "GET /time HTTP/1.1\r\n\r\n";
                    socket.Send(Encoding.UTF8.GetBytes(request));

                    StringBuilder responseBuilder = new StringBuilder();

                    if (debug) outWriter.Write("Reading first part of response...\t");
                    int nbytes1 = socket.Receive(buffer);
                    if (debug) outWriter.WriteLine($"Received {nbytes1} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes1));

                    if (debug) outWriter.Write("Reading second part of response...\t");
                    int nbytes2 = socket.Receive(buffer);
                    if (debug) outWriter.WriteLine($"Received {nbytes2} bytes:");
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, nbytes2));

                    response = responseBuilder.ToString();
                    if (debug) outWriter.WriteLine();
                    if (debug) outWriter.WriteLine("Closing Socket");
                    socket.Close();
                }
                catch (ArgumentNullException ex)
                {
                    response = $"ArgumentNullException: {ex.ToString()}";
                }
                catch (SocketException ex)
                {
                    response = $"SocketException: {ex.ToString()}";
                }
                catch (Exception ex)
                {
                    response = $"Unexpected Exception: {ex.ToString()}";
                }

            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

            if (debug) outWriter?.WriteLine();
            if (debug) outWriter?.WriteLine(response);
            return response;
        }

    }
}