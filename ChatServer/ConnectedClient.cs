using ChatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ConnectedClient
    {
        public Client connection;
        public string UserName { get; set; }

    }
}
