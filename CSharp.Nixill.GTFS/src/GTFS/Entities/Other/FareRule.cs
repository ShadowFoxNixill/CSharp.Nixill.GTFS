using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class FareRule : GTFSEntity
  {
    public FareRule(GTFSPropertyCollection properties) : base(properties) { }

    public string FareID => Properties["fare_id"];
    public string RouteID => Properties["route_id"];
    public string OriginID => Properties["origin_id"];
    public string DestinationID => Properties["destination_id"];
    public string ContainsID => Properties["contains_id"];
  }
}