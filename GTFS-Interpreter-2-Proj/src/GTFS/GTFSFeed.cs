using System.IO.Compression;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Nixill.GTFS
{
  public class GTFSFeed
  {
    internal ZipArchive File;

    public string DefaultAgencyId => Agencies.First().ID;

    public readonly IDEntityCollection<Agency> Agencies;
    public readonly IDEntityCollection<Route> Routes;
    public readonly GTFSCalendarCollection Calendars;
    public readonly IDEntityCollection<Stop> Stops;

    public GTFSFeed(ZipArchive file)
    {
      File = file;

      Agencies = new IDEntityCollection<Agency>(this, file.GetEntry("agency.txt"), Agency.Factory);
      Routes = new IDEntityCollection<Route>(this, file.GetEntry("routes.txt"), Route.Factory);
      Calendars = new GTFSCalendarCollection(this, file.GetEntry("calendar.txt"), file.GetEntry("calendar_dates.txt"));
      Stops = new IDEntityCollection<Stop>(this, file.GetEntry("stops.txt"), Stop.Factory);
    }
  }
}