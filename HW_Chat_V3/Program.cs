using System;
using System.ServiceModel;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "Server";
            try
            {
            ServiceHost host = new ServiceHost(typeof(ChatService));
            host.Open();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Server started");
            Console.WriteLine("-----------------------------------");
            Console.ReadKey();
            host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
