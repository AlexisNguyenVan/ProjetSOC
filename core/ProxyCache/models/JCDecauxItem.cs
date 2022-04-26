using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Device.Location;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProxyCache;

namespace ProxyCache
{

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

    public class StaticStation
    {
        public int number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public object shape { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public int available_bikes { get; set; }
        public string status { get; set; }
        public DateTime last_update { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public int overflow_bike_stands { get; set; }
        public int overflow_bikes { get; set; }

        public override string ToString()
        {
            return $"{nameof(number)}: {number}\n {nameof(contract_name)}: {contract_name}\n {nameof(name)}: {name}\n {nameof(address)}: {address}\n {nameof(position)}: {position}\n {nameof(shape)}: {shape}\n {nameof(banking)}: {banking}\n {nameof(bonus)}: {bonus}\n {nameof(bike_stands)}: {bike_stands}\n {nameof(available_bike_stands)}: {available_bike_stands}\n {nameof(available_bikes)}: {available_bikes}\n {nameof(status)}: {status}\n {nameof(last_update)}: {last_update}\n {nameof(connected)}: {connected}\n {nameof(overflow)}: {overflow}\n {nameof(overflow_bike_stands)}: {overflow_bike_stands}\n {nameof(overflow_bikes)}: {overflow_bikes}";
        }
    }

    public class DynamicStation
    {
        public override string ToString()
        {
            return $"{nameof(number)}: {number}\n {nameof(contractName)}: {contractName}\n {nameof(name)}: {name}\n {nameof(address)}: {address}\n {nameof(position)}: {position}\n {nameof(banking)}: {banking}\n {nameof(bonus)}: {bonus}\n {nameof(status)}: {status}\n {nameof(lastUpdate)}: {lastUpdate}\n {nameof(connected)}: {connected}\n {nameof(overflow)}: {overflow}\n {nameof(shape)}: {shape}\n {nameof(totalStands)}: {totalStands}\n {nameof(mainStands)}: {mainStands}\n {nameof(overflowStands)}: {overflowStands}\n";
        }

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
    }


    [DataContract]
    public class JCDecauxItem
    {
        [DataMember]
        public DynamicStation station { get; set; }

        public JCDecauxItem(string item)
        {
            this.station = JCDecauxHandler.getItem(item);
        }

        public override string ToString()
        {
            return $"{nameof(DynamicStation)}\n{station}";
        }
    }

}

