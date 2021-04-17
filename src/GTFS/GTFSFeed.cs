using System.IO.Compression;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS
{
  public class GTFSFeed
  {
    internal ZipArchive File;

    public IDEntityCollection<Agency> Agencies { get; internal set; }

    internal GTFSFeed(ZipArchive file, bool cache = false)
    {
      File = file;

      Agencies = new IDEntityCollection<Agency>(this, file.GetEntry("agency.txt"), Agency.Factory, cache, true);
    }
  }
}