﻿using Newtonsoft.Json;
using ProxyCache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RoutingService
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Routing : IRouting
    {
        private string openRoutekey ="5b3ce3597851110001cf6248218c274e87a246dabba13d3c95945026";
        private List<StaticStation> stations = JCDecauxHandler.GetAllStations().Result;
        static HttpClient client = new HttpClient();
       
        private DynamicStation GetClosestStation(double lat, double lon,bool isStart)
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

        public List<List<List<double>>> GetPath(string startName,string endName)
        {

            List<List<List<double>>> path = new List<List<List<double>>>();
            
            double[] start = GetPoint(startName).Result;
            double[] startStation = GetClosestStation(start[1], start[0], true).position.toArray().Reverse().ToArray();
            double[] end = GetPoint(endName).Result;
            double[] endStation = GetClosestStation(end[1], end[0], false).position.toArray().Reverse().ToArray();
            if (IsWalkable(start, startStation, end, endStation))
            {
                path.Add(this.getRoute(start, end, true).Result);
            }
            else
            {
                path.Add(this.getRoute(start, startStation, true).Result);
                path.Add(this.getRoute(startStation, endStation, false).Result);
                path.Add(this.getRoute(endStation, end, true).Result);
            }
            return path;
        }

        private bool IsWalkable(double[] start, double[] startStation, double[] end, double[] endStation)
        {
            double timeWalking = 0;
            double timeCycling = 0;
            try
            {
                timeWalking = this.getTime(start, end, true).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Calcul temps à pied raté.");
                return false;
            }
            timeCycling = this.getTime(start, startStation, false).Result +
                          this.getTime(startStation, endStation, false).Result +
                          this.getTime(end, endStation, false).Result;
            if (timeWalking >-1 && timeWalking < timeCycling)
            {
                return true;
            }
            else return false;

        }

        private async Task<double> getTime(double[] start, double[] end,bool walking)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                string mode = "cycling-road";
                if (walking)
                {
                    mode = "foot-walking";
                }
                HttpClient client = new HttpClient();
                string json = buildContentTime(start, end);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", openRoutekey);
                Console.Write("Calcul durée du trajet: start= " + start+" end="+end);
                HttpResponseMessage response = await client.PostAsync("https://api.openrouteservice.org/v2/matrix/"+mode,httpContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                TimeResponse time = JsonConvert.DeserializeObject<TimeResponse>(responseBody,settings);
                return time.durations[0][1];
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return -1;
        }

        private async Task<DynamicStation> GetStationFromProxy(string key)
        {
            try
            {
                HttpClient pathClient = new HttpClient();
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

        private async Task<double[]> GetPoint(string name)
        {
            try
            {

                Console.Write("Recherche pour: "+name);
                HttpResponseMessage response = await client.GetAsync("https://api.openrouteservice.org/geocode/search?api_key="+openRoutekey+"&text="+name);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                SearchResponse search = JsonConvert.DeserializeObject<SearchResponse>(responseBody);
                Console.WriteLine(", coordonnees renvoyées: " + search.features[0].geometry.coordinates[0]+" et "+search.features[0].geometry.coordinates[1]);
                return search.features[0].geometry.coordinates.ToArray();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        private async Task<List<List<double>>> getRoute(double[] start, double[] end,bool walking)
        {
            try
            {
                HttpClient pathClient = new HttpClient();
                string mode = "cycling-road";
                
                if (walking)
                {
                    mode = "foot-walking";
                }

                string json = buildContent(start, end);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                pathClient.DefaultRequestHeaders.Add("Authorization", openRoutekey);
                HttpResponseMessage response = await pathClient.PostAsync("https://api.openrouteservice.org/v2/directions/" + mode+"/geojson",httpContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Route route = JsonConvert.DeserializeObject<Route>(responseBody);
                return route.features[0].geometry.coordinates;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
        private string buildContent(double[] start, double[] end)
        {
            string content = "{\"coordinates\":[";
            content += "[" + start[0].ToString().Replace(",",".") + "," + start[1].ToString().Replace(",", ".") + "],";
            content += "[" + end[0].ToString().Replace(",", ".") + "," + end[1].ToString().Replace(",", ".") + "]"+"]}";
            return content;
        }

        private string buildContentTime(double[] start, double[] end)
        {
            string content = "{\"locations\":[";
            content += "[" + start[0].ToString().Replace(",", ".") + "," + start[1].ToString().Replace(",", ".") + "],";
            content += "[" + end[0].ToString().Replace(",", ".") + "," + end[1].ToString().Replace(",", ".") + "]" + "]}";
            return content;
        }

        public double GetTimeProxy(string key)
        {
            Console.WriteLine("Test du cache en passant par le routing.");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            this.GetStationFromProxy(key).Wait();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

    }



   
}
