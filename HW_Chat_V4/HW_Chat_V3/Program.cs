using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using Server;
using DBLayer;

namespace Server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Title = "Server";
            ServiceHost host = new ServiceHost(typeof(ChatService));
            host.Open();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Server started");
            Console.WriteLine("-----------------------------------");
            Console.ReadKey();
            host.Close();
        }
    }
}
