using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.Collections.Grid.CSV;
using Nixill.Utils;
using System.IO;

namespace Nixill.GTFS.Collections
{
  public abstract class GTFSFeedFile<T> : IEnumerable<T> where T : GTFSEntity
  {
    private ZipArchiveEntry InnerFile;
    private bool HasAgencyId;
    public readonly ICollection<T> Collection;
    private bool Cached;
    public readonly GTFSFeed ParentFeed;
    private GTFSEntityFactory<T> ObjectFactory;

    public IEnumerator<T> GetEnumerator()
    {
      if (Cached) {
        foreach (T item in Collection) yield return item;
        yield break;
      }

      using var stream = new StreamReader(InnerFile.Open());
      var rows = CSVParser.EnumerableToRows(FileUtils.StreamCharEnumerator(stream));

      List<string> header = new List<string>();
      bool first = false;

      foreach (List<string> row in rows)
      {
        if (first)
        {
          header = row;
          continue;
        }

        var props = header.Zip(row).ToDictionary(x => x.First, x => x.Second);

        T obj = this.ObjectFactory(ParentFeed, props);

        yield return obj;
      }

      Cached = true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }

  public delegate T GTFSEntityFactory<T>(GTFSFeed feed, Dictionary<string, string> props) where T : GTFSEntity;
}