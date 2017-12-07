using System;
using System.ServiceModel;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Authorization Service";
            try
            {
                ServiceHost host = new ServiceHost(typeof(AuthenticationService));
                host.Open();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Authentication service started");
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
