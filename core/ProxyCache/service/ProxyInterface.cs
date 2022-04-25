using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ProxyCache
{

    [ServiceContract]
    internal interface ProxyInterface
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "station?stationKey={stationKey}")]
        JCDecauxItem GetStation(string stationKey);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "timeStation?stationKey={stationKey}&time={time}")]
        JCDecauxItem TimeGetStation(string stationKey,double time);
    }
}
