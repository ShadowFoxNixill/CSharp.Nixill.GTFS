using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Stop : GTFSIdentifiedEntity
  {
    public string StopCode => Properties["stop_code"];
    public string Name => Properties["stop_name"];
    public string Description => Properties["stop_desc"];
    public double? Latitude => Properties.GetNullableDouble("stop_lat");
    public double? Longitude => Properties.GetNullableDouble("stop_lon");
    public string ZoneId => Properties["zone_id"];
    public string StopUrl => Properties["stop_url"];
    public StopLocationType LocationType => (StopLocationType)Properties.GetInt("location_type", 0);
    public string ParentStationId => Properties["parent_station"];
    public DateTimeZone TimeZone => Properties.GetTimeZone("stop_timezone");
    public Tristate WheelchairBoarding => (Tristate)Properties.GetInt("wheelchair_boarding", 0);
    public string LevelId => Properties["level_id"];
    public string PlatformCode => Properties["platform_code"];

    public Stop ParentStation => Feed.Stops[ParentStationId];

    private Stop(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "stop_id")
    {
    }

    public static Stop Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new Stop(feed, new GTFSPropertyCollection(properties));
    }
  }
}