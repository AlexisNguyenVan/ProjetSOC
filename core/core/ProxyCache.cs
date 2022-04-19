using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace core
{
    internal class ProxyCache<T>
    {
        ObjectCache cache = MemoryCache.Default;
        public DateTimeOffset dt_default;

        public T Get(string CacheItemName)
        {
            
            if (!cache.Contains(CacheItemName))
            {
                var value = Activator.CreateInstance(
                    typeof(T),
                    new object[] { CacheItemName });
                cache.Add(CacheItemName,value , dt_default);
            }
            return (T)cache.Get(CacheItemName);
        }

        public T Get(string CacheItemName, double dt_seconds)
        {

            if (!cache.Contains(CacheItemName))
            {
                DateTimeOffset now = DateTimeOffset.Now;
                
                var value = Activator.CreateInstance(
                    typeof(T),
                    new object[] { CacheItemName });
                cache.Add(CacheItemName, value, now.AddSeconds(dt_seconds));
            }
            return (T)cache.Get(CacheItemName);
        }

        public T Get(string CacheItemName, DateTimeOffset dt)
        {
            if (!cache.Contains(CacheItemName))
            {
                var value = Activator.CreateInstance(
                    typeof(T),
                    new object[] { CacheItemName });
                cache.Add(CacheItemName, value, dt);
            }
            return (T)cache.Get(CacheItemName);
        }

        public void PrintAllCache()
        {

            DateTime dt = DateTime.Now;

            Console.WriteLine("All key-values at " + dt.Second);
            //loop through all key-value pairs and print them
            foreach (var item in this.cache)
            {
                Console.WriteLine("cache object key-value: " + item.Key + "-" + item.Value);
            }
        }
    }
}
