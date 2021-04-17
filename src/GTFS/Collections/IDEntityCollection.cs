using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Nixill.Collections.Grid.CSV;
using Nixill.GTFS.Entities;
using Nixill.Utils;

namespace Nixill.GTFS.Collections
{
  public class IDEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSIdentifiedEntity
  {
    internal Dictionary<string, T> Dict;
    public readonly GTFSFeed Feed;
    private ZipArchiveEntry InnerFile;
    private bool HasAgencyId;
    private bool Cached;
    private GTFSEntityFactory<T> ObjectFactory;

    public int Count => Dict.Count;

    public IDEntityCollection(GTFSFeed feed, ZipArchiveEntry file, GTFSEntityFactory<T> factory, bool cache = false, bool hasAgency = false)
    {
      Dict = new Dictionary<string, T>();
      Feed = feed;
      InnerFile = file;
      Cached = cache;
      ObjectFactory = factory;
      HasAgencyId = hasAgency;
      if (cache)
      {
        foreach (T item in FileEnumerable())
        {
          Dict.Add(item.ID, item);
        }
      }
    }

    public IDEntityCollection(GTFSFeed feed, ICollection<T> objects, bool hasAgency = false)
    {
      Dict = new Dictionary<string, T>();
      Feed = feed;
      InnerFile = null;
      Cached = true;
      ObjectFactory = null;
      HasAgencyId = hasAgency;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict.Values).GetEnumerator();

    public bool Contains(T item) => Dict.ContainsKey(item.ID);
    public bool Contains(string key) => Dict.ContainsKey(key);

    public IEnumerator<T> GetEnumerator()
    {
      if (Cached) return Dict.Values.GetEnumerator();
      else return FileEnumerable().GetEnumerator();
    }

    private IEnumerable<T> FileEnumerable()
    {
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

        T obj = this.ObjectFactory(Feed, props);

        yield return obj;
      }
    }
  }
}