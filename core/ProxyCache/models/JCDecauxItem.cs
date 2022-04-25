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

        public GeoCoordinate toGeoCoord()
        {
            return new GeoCoordinate(latitude, longitude);
        }

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

    public class Station
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

        public GeoCoordinate getCoord()
        {
            return position.toGeoCoord();
        }
    }

    [DataContract]
    public class JCDecauxItem
    {
        [DataMember]
        public Station station;

        public JCDecauxItem(string item)
        {
            JCDecauxHandler handler = new JCDecauxHandler();
            this.station = handler.getItem(item);
        }

        public override string ToString()
        {
            return $"{nameof(station)}\n{station}";
        }
    }

}

