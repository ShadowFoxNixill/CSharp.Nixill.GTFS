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
    private bool HasAgencyId;
    private GTFSEntityFactory<T> ObjectFactory;

    public int Count => Dict.Count;

    public IDEntityCollection(GTFSFeed feed, ZipArchiveEntry file, GTFSEntityFactory<T> factory, bool hasAgency = false)
    {
      Dict = new Dictionary<string, T>();
      Feed = feed;
      ObjectFactory = factory;
      HasAgencyId = hasAgency;
      foreach (T item in FileEnumerable(file))
      {
        Dict.Add(item.ID, item);
      }
    }

    public IDEntityCollection(GTFSFeed feed, ICollection<T> objects, bool hasAgency = false)
    {
      Dict = new Dictionary<string, T>();
      Feed = feed;
      ObjectFactory = null;
      HasAgencyId = hasAgency;
      foreach (T item in objects)
      {
        Dict.Add(item.ID, item);
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict.Values).GetEnumerator();

    public bool Contains(T item) => Dict.ContainsKey(item.ID);
    public bool Contains(string key) => Dict.ContainsKey(key);

    public IEnumerator<T> GetEnumerator()
      => Dict.Values.GetEnumerator();

    public T this[string index] => Dict[index];

    private IEnumerable<T> FileEnumerable(ZipArchiveEntry file)
    {
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

        var props = header.Zip(row).ToDictionary(x => x.First, x => x.Second);

        T obj = this.ObjectFactory(Feed, props);

        yield return obj;
      }
    }
  }

  public delegate T GTFSEntityFactory<T>(GTFSFeed feed, Dictionary<string, string> props) where T : GTFSEntity;
}