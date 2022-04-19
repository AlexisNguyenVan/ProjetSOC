using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }
    }

    public class TotalStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class MainStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class JCDecauxItem

    {
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public DateTime lastUpdate { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public object shape { get; set; }
        public TotalStands totalStands { get; set; }
        public MainStands mainStands { get; set; }
        public object overflowStands { get; set; }


        public JCDecauxItem(string key)
        {
            string[] keys=key.Split('_');
            this.BuildItem(keys[0], Int32.Parse(keys[1]));
        }


        async public void BuildItem(string contract,int id)
        {
            try
            {
                string apiKey = "1c7f71a6f2090235c69e3a96e7fc85feb1fae21b";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?contract=" + contract + "&apiKey=" + apiKey);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                Console.Write(responseBody);
                List<JCDecauxItem> items = JsonConvert.DeserializeObject<List<JCDecauxItem>>(responseBody);
                foreach (JCDecauxItem item in items)
                {
                    if (item.number == id)
                    {
                        this.name=item.name;
                        this.contractName=item.contractName;
                        this.mainStands = item.mainStands;
                        this.number=item.number;
                        this.address = item.address;
                        this.position=item.position;
                    }
                }
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            Console.ReadLine();
        }


      
    }


}