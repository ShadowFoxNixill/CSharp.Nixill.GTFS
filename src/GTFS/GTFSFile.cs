using System.IO.Compression;

namespace Nixill.GTFS
{
  public class GTFSFile
  {
    internal ZipArchive File;

    internal GTFSFile(ZipArchive file, bool preload)
    {
      File = file;

      if (preload)
      {
        Agencies.Preload();
      }
    }
  }
}