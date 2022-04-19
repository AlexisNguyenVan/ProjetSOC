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

        public override string ToString()
        {
            return $"{nameof(latitude)}: {latitude}, {nameof(longitude)}: {longitude}";
        }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }

        public override string ToString()
        {
            return $"\n\t{nameof(bikes)}: {bikes}\n\t {nameof(stands)}: {stands}\n\t {nameof(mechanicalBikes)}: {mechanicalBikes}\n\t {nameof(electricalBikes)}: {electricalBikes}\n\t {nameof(electricalInternalBatteryBikes)}: {electricalInternalBatteryBikes}\n\t {nameof(electricalRemovableBatteryBikes)}: {electricalRemovableBatteryBikes}\n";
        }
    }

    public class Stands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }

        public override string ToString()
        {
            return $"{nameof(availabilities)}: {availabilities}\n {nameof(capacity)}: {capacity}\n";
        }
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
        public Stands totalStands { get; set; }
        public Stands mainStands { get; set; }
        public object overflowStands { get; set; }

        public override string ToString()
        {
            return $"{nameof(number)}: {number}\n {nameof(contractName)}: {contractName}\n {nameof(name)}: {name}\n {nameof(address)}: {address}\n {nameof(position)}: {position}\n {nameof(banking)}: {banking}\n {nameof(bonus)}: {bonus}\n {nameof(status)}: {status}\n {nameof(lastUpdate)}: {lastUpdate}\n {nameof(connected)}: {connected}\n {nameof(overflow)}: {overflow}\n {nameof(shape)}: {shape}\n {nameof(totalStands)}: {totalStands}\n {nameof(mainStands)}: {mainStands}\n {nameof(overflowStands)}: {overflowStands}\n";
        }


        public JCDecauxItem(string key)
        {
            if (key!=null)
            {
                string[] keys = key.Split('_');
                Console.Write(keys[0]);
                Console.Write(keys[1]);
                JCDecauxItem result = Task.Run(async () => await BuildItem(keys[0], Int32.Parse(keys[1]))).Result ;
                this.name = result.name;
                this.number = result.number;
                this.position= result.position;
                this.banking = result.banking;
                this.bonus = result.bonus;
                this.status = result.status;
                this.lastUpdate = result.lastUpdate;
                this.connected = result.connected;
                this.overflow = result.overflow;
                this.shape = result.shape;
                this.totalStands = result.totalStands;
                this.mainStands = result.mainStands;
                this.address= result.address;
                this.contractName = result.contractName;
                this.overflowStands= result.overflowStands;
            }
           
        }


        public static async Task<JCDecauxItem> BuildItem(string contract,int id)
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
                List<JCDecauxItem> items = JsonConvert.DeserializeObject<List<JCDecauxItem>>(responseBody);
                foreach (JCDecauxItem item in items)
                {
                    if (item.number == id)
                    {
                        return item;
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


      
    }


}