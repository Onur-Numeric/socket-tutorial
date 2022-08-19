using Oxy.SocketTutorial.Utils;

namespace Oxy.SocketTutorial
{
    public class SocketTutorialClient
    {
        public async static void Main(string[] args)
        {
            SocketClientAsync client = new SocketClientAsync("127.0.0.1", 8080);

            string response = await client.GetTime();
        }
    }
}