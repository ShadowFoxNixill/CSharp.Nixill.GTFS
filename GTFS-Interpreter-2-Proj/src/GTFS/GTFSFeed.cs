using System.IO.Compression;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using System.Collections.Generic;
using System;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS
{
  public class GTFSFeed
  {
    public readonly IGTFSDataSource DataSource;

    public string DefaultAgencyId => Agencies.First().ID;

    public readonly IDEntityCollection<Agency> Agencies;
    public readonly IDEntityCollection<Route> Routes;
    public readonly GTFSCalendarCollection Calendars;
    public readonly IDEntityCollection<Stop> Stops;

    public GTFSFeed(IGTFSDataSource source)
    {
      DataSource = source;

      Agencies = new IDEntityCollection<Agency>(this, source, "agency", Agency.Factory);
      Routes = new IDEntityCollection<Route>(this, source, "routes", Route.Factory);
      Calendars = new GTFSCalendarCollection(this, source);
      Stops = new IDEntityCollection<Stop>(this, source, "stops", Stop.Factory);
    }
  }
}