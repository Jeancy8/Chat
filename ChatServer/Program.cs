using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ChatServer
{
    class Program
    {
        public static ChatService _server;

        static void Main(string[] args)
        {
            _server = new ChatService();
            using (ServiceHost host = new ServiceHost(_server))
            {
                host.Open();
                Console.WriteLine("Le server est lancé !");
                Console.ReadLine();
            }
        }
    }
}
