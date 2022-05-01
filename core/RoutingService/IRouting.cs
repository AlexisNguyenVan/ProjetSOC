using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using ProxyCache;

namespace RoutingService
{
    [ServiceContract]
    public interface IRouting
    {

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "path?startName={startName}&endName={endName}")]
        List<List<List<double>>> GetPath(string startName,string endName);

        [OperationContract]
        double GetTimeProxy(string key);

    }
}
