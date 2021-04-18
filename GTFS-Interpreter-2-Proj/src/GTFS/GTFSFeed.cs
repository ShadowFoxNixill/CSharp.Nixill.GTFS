using System.IO.Compression;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;

namespace Nixill.GTFS
{
  public class GTFSFeed
  {
    internal ZipArchive File;

    public string DefaultAgencyId => Agencies.First().ID;

    public IDEntityCollection<Agency> Agencies { get; internal set; }
    public IDEntityCollection<Route> Routes { get; internal set; }

    public GTFSFeed(ZipArchive file)
    {
      File = file;

      Agencies = new IDEntityCollection<Agency>(this, file.GetEntry("agency.txt"), Agency.Factory, true);
      Routes = new IDEntityCollection<Route>(this, file.GetEntry("routes.txt"), Route.Factory, true);
    }
  }
}