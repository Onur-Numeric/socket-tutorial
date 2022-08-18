using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_basic
{
    public class SocketClient
    {
        public string host { get; private set; }
        public int port { get; private set; }

        public SocketClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void connect()
        {

        }

    }
}
