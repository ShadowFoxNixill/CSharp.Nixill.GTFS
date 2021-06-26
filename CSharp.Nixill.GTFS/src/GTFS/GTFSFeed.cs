using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS
{
  /// <summary>
  ///   Represents a single GTFS feed. This class provides access to all
  ///   of the data within the feed.
  /// </summary>
  public class GTFSFeed
  {
    public readonly IGTFSDataSource DataSource;
    public string DefaultAgencyID { get; internal set; }

    public readonly IDEntityCollection<Agency> Agencies;
    public readonly IDEntityCollection<Route> Routes;
    public readonly GTFSCalendarCollection Calendars;
    public readonly IDEntityCollection<Stop> Stops;
    public readonly IDEntityCollection<Trip> Trips;
    public readonly GTFSOrderedEntityCollection<StopTime> StopTimes;

    public GTFSFeed(IGTFSDataSource source)
    {
      DataSource = source;

      Agencies = new IDEntityCollection<Agency>(DataSource, "agency", Agency.Factory);
      DefaultAgencyID = Agencies.First().ID;
      Routes = new IDEntityCollection<Route>(DataSource, "routes", Route.GetFactory(DefaultAgencyID));
      Calendars = new GTFSCalendarCollection(DataSource);
      Stops = new IDEntityCollection<Stop>(DataSource, "stops", Stop.Factory);
      Trips = new IDEntityCollection<Trip>(DataSource, "trips", Trip.Factory);
      StopTimes = new GTFSOrderedEntityCollection<StopTime>(DataSource, "stop_times", StopTime.Factory);
    }
  }
}