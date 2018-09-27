using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DBLayer;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Authorization Service";
            ServiceHost host = new ServiceHost(typeof(AuthorizationService));
            host.Open();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Authorization service started");
            Console.WriteLine("-----------------------------------");
            Console.ReadKey();
            host.Close();
        }
    }
}
