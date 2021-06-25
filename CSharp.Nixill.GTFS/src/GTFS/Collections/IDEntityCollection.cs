using System.Collections;
using System.Collections.Generic;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  public class IDEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSIdentifiedEntity
  {
    private Dictionary<string, T> Dict;
    private List<GTFSUnparsedEntity> Unparsed;

    public int Count => Dict.Count;

    /// <summary>
    ///   Creates a new IDEntityCollection from the given table in the
    ///   given feed.
    /// </summary>
    public IDEntityCollection(IGTFSDataSource source, string tableName, GTFSEntityFactory<T> factory)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();

      foreach (T item in source.GetObjects(tableName, factory, Unparsed))
      {
        Dict.Add(item.ID, item);
      }
    }

    public IDEntityCollection(IEnumerable<T> objects)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();

      foreach (T item in objects)
      {
        Dict.Add(item.ID, item);
      }
    }

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

    IEnumerator IEnumerable.GetEnumerator() => Dict.Values.GetEnumerator();
  }

  public delegate T GTFSEntityFactory<T>(IEnumerable<(string, string)> props) where T : GTFSEntity;
}