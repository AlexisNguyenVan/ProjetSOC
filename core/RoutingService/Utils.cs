using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingService
{
    public class Utils
    {
        public static double GetDistanceFrom2GpsCoordinates(double lat1, double lon1, double lat2, double lon2)
        {
            // Radius of the earth in km
            var earthRadius = 6371;
            var dLat = Deg2Rad(lat2 - lat1);
            var dLon = Deg2Rad(lon2 - lon1);
            var a =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = earthRadius * c; // Distance in km
            return d;
        }

        public static double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
