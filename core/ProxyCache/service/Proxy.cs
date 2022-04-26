
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;


namespace ProxyCache
{

    public class Proxy : IProxy
    {
        private readonly ProxyCache<JCDecauxItem> cache = new ProxyCache<JCDecauxItem>();
        public JCDecauxItem GetStation(string stationKey)
        {
            Console.WriteLine("Requête GetStation key: "+stationKey);
            return cache.Get(stationKey);
        }

        public JCDecauxItem TimeGetStation(string stationKey, double time)
        {
            Console.WriteLine("Requête TimeGetStation key: " + stationKey+" time: "+time);
            return cache.Get(stationKey, time);
        }

    }
}
