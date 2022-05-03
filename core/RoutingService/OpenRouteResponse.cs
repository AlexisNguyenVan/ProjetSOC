using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingService
{
    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public int type { get; set; }
        public string instruction { get; set; }
        public string name { get; set; }
        public List<int> way_points { get; set; }
    }

    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }
    }

    public class Summary
    {
        public double distance { get; set; }
        public double duration { get; set; }
    }

    public class Properties
    {
        public List<Segment> segments { get; set; }
        public Summary summary { get; set; }
        public List<int> way_points { get; set; }
    }

    public class Geometry
    {
        public List<List<double>> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Feature
    {
        public List<double> bbox { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Query
    {
        public List<List<double>> coordinates { get; set; }
        public string profile { get; set; }
        public string format { get; set; }
    }

    public class Engine
    {
        public string version { get; set; }
        public DateTime build_date { get; set; }
        public DateTime graph_date { get; set; }
    }

    public class Metadata
    {
        public string attribution { get; set; }
        public string service { get; set; }
        public long timestamp { get; set; }
        public Query query { get; set; }
        public Engine engine { get; set; }
    }

    public class Route
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
        public List<double> bbox { get; set; }
        public Metadata metadata { get; set; }
    }


    public class Lang
    {
        public string name { get; set; }
        public string iso6391 { get; set; }
        public string iso6393 { get; set; }
        public string via { get; set; }
        public bool defaulted { get; set; }
    }

    public class ParsedText
    {
        public string city { get; set; }
        public string country { get; set; }
    }

    public class QuerySearch
    {
        public string text { get; set; }
        public int size { get; set; }
        public List<string> layers { get; set; }
        public bool @private { get; set; }
        public Lang lang { get; set; }
        public int querySize { get; set; }
        public string parser { get; set; }
        public ParsedText parsed_text { get; set; }
    }

    public class EngineSearch
    {
        public string name { get; set; }
        public string author { get; set; }
        public string version { get; set; }
    }

    public class Geocoding
    {
        public string version { get; set; }
        public string attribution { get; set; }
        public QuerySearch query { get; set; }
        public List<string> warnings { get; set; }
        public EngineSearch engine { get; set; }
        public long timestamp { get; set; }
    }

    public class GeometrySearch
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }

        public override string ToString()
        {
            return $"{nameof(coordinates)}: {coordinates}";
        }
    }

    public class PropertiesSearch
    {
        public string id { get; set; }
        public string gid { get; set; }
        public string layer { get; set; }
        public string source { get; set; }
        public string source_id { get; set; }
        public string name { get; set; }
        public double confidence { get; set; }
        public string match_type { get; set; }
        public string accuracy { get; set; }
        public string country { get; set; }
        public string country_gid { get; set; }
        public string country_a { get; set; }
        public string macroregion { get; set; }
        public string macroregion_gid { get; set; }
        public string macroregion_a { get; set; }
        public string region { get; set; }
        public string region_gid { get; set; }
        public string region_a { get; set; }
        public string macrocounty { get; set; }
        public string macrocounty_gid { get; set; }
        public string county { get; set; }
        public string county_gid { get; set; }
        public string localadmin { get; set; }
        public string localadmin_gid { get; set; }
        public string locality { get; set; }
        public string locality_gid { get; set; }
        public string continent { get; set; }
        public string continent_gid { get; set; }
        public string label { get; set; }
    }

    public class FeatureSearch
    {
        public string type { get; set; }
        public GeometrySearch geometry { get; set; }
        public PropertiesSearch properties { get; set; }
        public List<double> bbox { get; set; }
    }

    public class SearchResponse
    {
        public Geocoding geocoding { get; set; }
        public string type { get; set; }
        public List<FeatureSearch> features { get; set; }
        public List<double> bbox { get; set; }
    }

    public class DestinationT
    {
        public List<double> location { get; set; }
        public double snapped_distance { get; set; }
    }

    public class SourceT
    {
        public List<double> location { get; set; }
        public double snapped_distance { get; set; }
    }

    public class QueryT
    {
        public List<List<double>> locations { get; set; }
        public string profile { get; set; }
        public string responseType { get; set; }
    }

    public class EngineT
    {
        public string version { get; set; }
        public DateTime build_date { get; set; }
        public DateTime graph_date { get; set; }
    }

    public class MetadataT
    {
        public string attribution { get; set; }
        public string service { get; set; }
        public long timestamp { get; set; }
        public Query query { get; set; }
        public Engine engine { get; set; }
    }

    public class TimeResponse
    {
        public List<List<double>> durations { get; set; }
        public List<DestinationT> destinations { get; set; }
        public List<SourceT> sources { get; set; }
        public MetadataT metadata { get; set; }
    }


}
