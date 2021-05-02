using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Nixill.Collections.Grid.CSV;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using Nixill.Utils;

namespace Nixill.GTFS.Parsing
{
  public static class GTFSFileEnumerator
  {
    public static IEnumerable<T> Enumerate<T>(GTFSFeed feed, ZipArchiveEntry file, GTFSEntityFactory<T> factory, List<GTFSUnparsedEntity> unparsed) where T : GTFSEntity
    {
      // For null files, return empty collections
      if (file == null) yield break;

      using var stream = new StreamReader(file.Open());
      var rows = CSVParser.EnumerableToRows(FileUtils.StreamCharEnumerator(stream));

      List<string> header = new List<string>();
      bool first = true;

      foreach (List<string> row in rows)
      {
        if (first)
        {
          header = row;
          first = false;
          continue;
        }

        var props = header.Zip(row);

        T obj;

        try
        {
          obj = factory(feed, props);
        }
        catch (Exception ex)
        {
          GTFSUnparsedEntity ent = new GTFSUnparsedEntity(feed, props, ex);
          unparsed.Add(ent);
          continue;
        }

        yield return obj;
      }
    }
  }
}