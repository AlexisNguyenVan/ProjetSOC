using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProxyCache
{
    public class JCDecauxHandler
    {
        string apiKey = "1c7f71a6f2090235c69e3a96e7fc85feb1fae21b";
        HttpClient client = new HttpClient();

        public Station getItem(string key)
        {
            if (key != null)
            {
                string[] keys = key.Split('_');
                return Task.Run(async () => await GetStation(keys[0], Int32.Parse(keys[1]))).Result;

            }
            return null;

        }

        public async Task<Station> GetStation(string contractName, int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?contract=" + contractName + "&apiKey=" + this.apiKey);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Station> stations = JsonConvert.DeserializeObject<List<Station>>(responseBody);
                foreach (Station station in stations)
                {
                    if (station.number == id)
                    {
                        return station;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<List<Station>> GetAllStations()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v2/stations?apiKey=" +this.apiKey);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Station> stations = JsonConvert.DeserializeObject<List<Station>>(responseBody);
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
