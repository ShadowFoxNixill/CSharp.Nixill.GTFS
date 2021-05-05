using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  public class IDEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSIdentifiedEntity
  {
    private Dictionary<string, T> Dict;
    private List<GTFSUnparsedEntity> Unparsed;
    public readonly GTFSFeed Feed;
    private GTFSEntityFactory<T> ObjectFactory;

    public int Count => Dict.Count;

    public IDEntityCollection(GTFSFeed feed, IGTFSDataSource source, string tableName, GTFSEntityFactory<T> factory)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;
      ObjectFactory = factory;

      foreach (T item in source.GetObjects(Feed, tableName, ObjectFactory, Unparsed))
      {
        Dict.Add(item.ID, item);
      }
    }

    public IDEntityCollection(GTFSFeed feed, ICollection<T> objects)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;
      ObjectFactory = null;

      foreach (T item in objects)
      {
        Dict.Add(item.ID, item);
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => Dict.Values.GetEnumerator();

    public bool Contains(T item) => Dict.ContainsKey(item.ID);
    public bool Contains(string key) => Dict.ContainsKey(key);

    public IEnumerator<T> GetEnumerator()
      => Dict.Values.GetEnumerator();

    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();

    public T this[string index]
    {
      get
      {
        if (Dict.ContainsKey(index)) return Dict[index];
        return null;
      }
    }
  }

  public delegate T GTFSEntityFactory<T>(GTFSFeed feed, IEnumerable<(string, string)> props) where T : GTFSEntity;
}