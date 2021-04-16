using System.IO.Compression;
using NodaTime;

namespace Nixill.GTFS
{
  public class GTFSFeed
  {
    internal ZipArchive File;
    public DateTimeZone TimeZone { get; internal set; }

    internal GTFSFeed(ZipArchive file, bool preload)
    {
      File = file;

      if (preload)
      {
        Agencies.Preload();
      }
    }
  }
}