using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using HeavyClient.Routing;

namespace HeavyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Entrez clé getStation: ");
                string key =Console.ReadLine();
                RoutingClient client = new RoutingClient();
                Console.WriteLine("Temps de la requête sans cache: "+client.GetTimeProxy(key)+"ms");
                Console.WriteLine("Temps de la requête avec cache: " + client.GetTimeProxy(key) + "ms");
                Console.ReadLine();
            }
        }
    }
}
