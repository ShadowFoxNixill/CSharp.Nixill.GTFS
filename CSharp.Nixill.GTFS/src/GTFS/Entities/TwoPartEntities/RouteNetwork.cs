using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

public class RouteNetwork : GTFSTwoPartEntity<string, string>
{
  public RouteNetwork(GTFSPropertyCollection properties) : base(properties, properties["network_id"], properties["route_id"])
  { }

  public string NetworkID => FirstKey;
  public string RouteID => SecondKey;
}