using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Trip : GTFSIdentifiedEntity
  {
    public string RouteID => Properties["route_id"];
    public string ServiceID => Properties["service_id"];
    public string Headsign => Properties["trip_headsign"];
    public string ShortName => Properties["trip_short_name"];
    public DirectionId? DirectionID => (DirectionId?)Properties.GetInt("direction_id");
    public string BlockID => Properties["block_id"];
    public string ShapeID => Properties["shape_id"];
    public Tristate WheelchairAccessible => (Tristate)Properties.GetInt("wheelchair_accessible", 0);
    public Tristate BikesAllowed => (Tristate)Properties.GetInt("bikes_allowed", 0);

    public Trip(GTFSPropertyCollection properties) : base(properties, "trip_id") { }
  }
}