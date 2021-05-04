using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Trip : GTFSIdentifiedEntity
  {
    public string RouteId => Properties["route_id"];
    public string ServiceId => Properties["service_id"];
    public string Headsign => Properties["trip_headsign"];
    public string ShortName => Properties["trip_short_name"];
    public DirectionId? DirectionId => (DirectionId?)Properties.GetInt("direction_id");
    public string BlockId => Properties["block_id"];
    public string ShapeId => Properties["shape_id"];
    public Tristate WheelchairAccessible => (Tristate)Properties.GetInt("wheelchair_accessible", 0);
    public Tristate BikesAllowed => (Tristate)Properties.GetInt("bikes_allowed", 0);

    public Route Route => Feed.Routes[RouteId];
    public Calendar ServiceCalendar => Feed.Calendars[ServiceId];

    private Trip(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "trip_id")
    {
    }

    public static Trip Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new Trip(feed, new GTFSPropertyCollection(properties));
    }
  }

  namespace Extensions
  {
    public static class TripFeedExtensions
    {

    }
  }
}