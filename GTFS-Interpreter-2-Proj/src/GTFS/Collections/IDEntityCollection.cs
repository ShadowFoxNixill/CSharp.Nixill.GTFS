using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  /// <summary>
  ///   A collection of <see cref="GTFSIdentifiedEntity" />s, accessible
  ///   by their IDs.
  /// </summary>
  public class IDEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSIdentifiedEntity
  {
    private Dictionary<string, T> Dict;
    private List<GTFSUnparsedEntity> Unparsed;

    /// <summary>
    ///   The <see cref="GTFSFeed"> from which this collection was made.
    /// </summary>
    public readonly GTFSFeed Feed;

    /// <summary>
    ///   The number of items within this collection.
    /// </summary>
    public int Count => Dict.Count;

    /// <summary>
    ///   Creates a new IDEntityCollection from the given table in the
    ///   given feed.
    /// </summary>
    public IDEntityCollection(GTFSFeed feed, string tableName, GTFSEntityFactory<T> factory)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;

      foreach (T item in feed.DataSource.GetObjects(Feed, tableName, factory, Unparsed))
      {
        Dict.Add(item.ID, item);
      }
    }

    /// <summary>
    ///   Creates a new IDEntityCollection from the given collection of
    ///   existing objects.
    /// </summary>
    public IDEntityCollection(GTFSFeed feed, ICollection<T> objects)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;

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