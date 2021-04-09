using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Nixill.GTFS
{
  public static class GTFSLoader
  {
    public static GTFSFile Load(string path, bool preload = false)
    {
      // First make sure the path itself exists
      if (!File.Exists(path))
      {
        throw new FileNotFoundException("This file does not exist.", path);
      }

      // Open the zip file
      using ZipArchive file = ZipFile.OpenRead(path);

      // And the list of loaded files
      HashSet<string> files = new HashSet<string>();

      // And the file object
      GTFSFile ret = new GTFSFile(file, preload);

      // And output! :D
      return ret;
    }
  }
}