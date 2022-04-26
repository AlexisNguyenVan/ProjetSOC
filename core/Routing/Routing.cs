using Newtonsoft.Json;
using ProxyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    internal class Routing : IRouting
    {
        private List<StaticStation> stations = JCDecauxHandler.GetAllStations().Result;
        static HttpClient client = new HttpClient();
        public DynamicStation GetClosestStation(double lat, double lon,bool isStart)
        {
            Console.WriteLine("Requête GetClosestStation latitude: "+lat +" longitude: "+lon);
            double distance = Utils.GetDistanceFrom2GpsCoordinates(stations[0].position.latitude, stations[0].position.longitude,lat,lon);
            StaticStation closest = stations[0];
            foreach (StaticStation station in stations)
            {
                if (distance > Utils.GetDistanceFrom2GpsCoordinates(station.position.latitude,
                        station.position.longitude, lat, lon))
                {
                    DynamicStation temp = this.GetStationFromProxy(station.contract_name + "_" + station.number).Result;
                    if (temp.mainStands.availabilities.bikes > 0 && temp.status.Equals("OPEN"))
                    {
                        if (isStart || temp.mainStands.availabilities.stands > 0)
                        {
                            closest = station;
                            distance = Utils.GetDistanceFrom2GpsCoordinates(station.position.latitude,
                                station.position.longitude, lat, lon);
                        }
                    }
                }
            }
            Console.WriteLine("Station la plus proche: "+closest.name);
            return this.GetStationFromProxy(closest.contract_name + "_" + closest.number).Result;

        }

        public DynamicStation GetPath(double startLat, double startLon, double endLat, double endLon)
        {
            throw new NotImplementedException();
        }

        public async Task<DynamicStation> GetStationFromProxy(string key)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:8000/Proxy/station?stationKey="+key);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JCDecauxItem item = JsonConvert.DeserializeObject<JCDecauxItem>(responseBody);
                return item.station;
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
