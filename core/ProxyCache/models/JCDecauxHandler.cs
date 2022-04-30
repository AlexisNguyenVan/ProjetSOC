using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProxyCache
{
    public static class JCDecauxHandler
    {
        static string apiKey = "1c7f71a6f2090235c69e3a96e7fc85feb1fae21b";
        static HttpClient client = new HttpClient();

        public static DynamicStation getItem(string key)
        {
            if (key != null)
            {
                string[] keys = key.Split('_');
                return Task.Run(async () => await GetStation(keys[0], Int32.Parse(keys[1]))).Result;

            }
            return null;

        }

        public static async Task<DynamicStation> GetStation(string contractName, int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/stations/"+id+"?contract=" + contractName + "&apiKey=" + apiKey);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                DynamicStation dynamicStation = JsonConvert.DeserializeObject<DynamicStation>(responseBody);
                return dynamicStation;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public static async Task<List<StaticStation>> GetAllStations()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v2/stations?apiKey=" +apiKey);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<StaticStation> stations = JsonConvert.DeserializeObject<List<StaticStation>>(responseBody,settings);
                return stations;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }


}
