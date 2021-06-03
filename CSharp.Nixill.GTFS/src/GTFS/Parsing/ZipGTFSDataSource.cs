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
  /// <summary>
  ///   An IGTFSDataSource using a zip archive as its backend.
  /// </summary>
  public class ZipGTFSDataSource : IGTFSDataSource
  {
    private ZipArchive Archive;

    /// <summary>
    ///   Creates a <c>ZipGTFSDataSource</c> using an existing
    ///   <c>ZipArchive</c> object.
    /// </summary>
    public ZipGTFSDataSource(ZipArchive archive)
    {
      Archive = archive;
    }

    /// <summary>
    ///   Creates a <c>ZipGTFSDataSource</c> using a new
    ///   <see cref="ZipArchive" /> object referring to <c>archiveName</c>.
    /// </summary>
    public ZipGTFSDataSource(string archiveName) : this(ZipFile.OpenRead(archiveName))
    { }

    public IEnumerable<T> GetObjects<T>(string table, GTFSEntityFactory<T> factory, List<GTFSUnparsedEntity> unparsed = null) where T : GTFSEntity
    {
      // Get the file:
      ZipArchiveEntry file = Archive.GetEntry(table);

      // If that's not found, try appending .txt:
      if (file == null) file = Archive.GetEntry($"{table}.txt");

      // If still nout found, return an empty collection.
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
          obj = factory(props);
        }
        catch (Exception ex)
        {
          if (unparsed == null) throw ex;
          GTFSUnparsedEntity ent = new GTFSUnparsedEntity(new GTFSPropertyCollection(props), ex);
          unparsed.Add(ent);
          continue;
        }

        yield return obj;
      }
    }
  }
}