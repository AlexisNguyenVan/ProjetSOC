using Routing;

using System.ServiceModel;
using ProxyCache.Proxy;



namespace core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost proxyHost = new ServiceHost(typeof(Proxy));
            

            proxyHost.Open();

            
        }
    }
}
