using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Sources;

namespace Nixill.GTFS.Feeds
{
  public interface IGTFSFeed
  {
    public IGTFSDataSource DataSource { get; }

    public IDEntityCollection<Agency> Agencies { get; }
    public IDEntityCollection<Route> Routes { get; }
    public GTFSCalendarCollection Calendars { get; }
    public IDEntityCollection<Stop> Stops { get; }
    public IDEntityCollection<Trip> Trips { get; }
    public GTFSOrderedEntityCollection<StopTime> StopTimes { get; }
  }
}