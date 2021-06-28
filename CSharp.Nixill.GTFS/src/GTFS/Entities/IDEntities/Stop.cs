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
    public string ZoneID => Properties["zone_id"];
    public string StopUrl => Properties["stop_url"];
    public StopLocationType LocationType => (StopLocationType)Properties.GetInt("location_type", 0);
    public string ParentStationID => Properties["parent_station"];
    public DateTimeZone TimeZone => Properties.GetTimeZone("stop_timezone");
    public Tristate WheelchairBoarding => (Tristate)Properties.GetInt("wheelchair_boarding", 0);
    public string LevelID => Properties["level_id"];
    public string PlatformCode => Properties["platform_code"];

    private Stop(GTFSPropertyCollection properties) : base(properties, "stop_id")
    {
    }

    /// <summary>Creates a new <c>Stop</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static Stop Factory(IEnumerable<(string, string)> properties)
      => new Stop(new GTFSPropertyCollection(properties));
  }
}