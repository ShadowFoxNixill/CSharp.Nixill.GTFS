using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A key location in a GTFS feed, such as a transit stop, station,
  ///   or station entrance/exit.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     The ID for a <c>Stop</c> is its <c>stop_id</c> property.
  ///   </para>
  ///   <para>
  ///     The term "station entrance" refers to both station entrances and
  ///     station exits. Stops, stations or station entrances are
  ///     collectively referred to as locations. Multiple routes may use the same stop.
  ///   </para>
  /// </remarks>
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