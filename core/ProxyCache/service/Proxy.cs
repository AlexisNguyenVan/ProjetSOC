
using System.ServiceModel;


namespace ProxyCache
{

    public class Proxy : ProxyInterface
    {
        private readonly ProxyCache<JCDecauxItem> cache = new ProxyCache<JCDecauxItem>();
        public JCDecauxItem GetStation(string stationKey)
        {
            return cache.Get(stationKey);
        }

        public JCDecauxItem TimeGetStation(string stationKey, double time)
        {
            return cache.Get(stationKey, time);
        }
    }
}
