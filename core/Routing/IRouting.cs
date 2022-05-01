using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using ProxyCache;

namespace Routing
{
    [ServiceContract]
    public interface IRouting
    {

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "closestStation?lat={lat}&lon={lon}&isStart={isStart}")]
        DynamicStation GetClosestStation(double lat, double lon, bool isStart);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "path?startName={startName}&endName={endName}")]
        List<List<List<double>>> GetPath(string startName,string endName);

        [OperationContract]
        double GetTimeProxy(string key);


    }
}
