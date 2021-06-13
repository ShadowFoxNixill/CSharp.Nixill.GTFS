using System.IO.Compression;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using System.Collections.Generic;
using System;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS
{
  /// <summary>
  ///   Represents a single GTFS feed. This class provides access to all
  ///   of the data within the feed.
  /// </summary>
  public class GTFSFeed
  {
    /// <summary>
    ///   The data source and parser of the GTFS feed. It can be used to
    ///   parse custom tables that this parser may not be expecting to see.
    /// </summary>
    public readonly IGTFSDataSource DataSource;

    /// <summary>
    ///   The feed's default <c>agency_id</c>. If there are multiple
    ///   agencies, this default shouldn't be used, but will return an
    ///   arbitrarily selected ID.
    /// </summary>
    public string DefaultAgencyID { get; internal set; }

    /// <summary>
    ///   The collection of <see cref="Agency" />s within the feed.
    /// </summary>
    public readonly IDEntityCollection<Agency> Agencies;

    /// <summary>
    ///   The collection of <see cref="Route" />s within the feed.
    /// </summary>
    public readonly IDEntityCollection<Route> Routes;

    /// <summary>
    ///   The collection of <see cref="Calendar" />s and
    ///   <see cref="CalendarDate" />s within the feed.
    /// </summary>
    public readonly GTFSCalendarCollection Calendars;

    /// <summary>
    ///   The collection of <see cref="Stop" />s within the feed.
    /// </summary>
    public readonly IDEntityCollection<Stop> Stops;

    /// <summary>
    ///   The collection of <see cref="Trip" />s within the feed.
    /// </summary>
    public readonly IDEntityCollection<Trip> Trips;

    /// <summary>
    ///   The collection of <see cref="StopTime" />s within the feed.
    /// </summary>
    public readonly GTFSOrderedEntityCollection<StopTime> StopTimes;

    /// <summary>
    ///   Creates a GTFS feed with a given <see cref="IGTFSDataSource" />.
    /// </summary>
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