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
  /// <typeparam name="T">
  ///   The type of entity that this collection contains.
  /// </typeparam>
  public class IDEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSIdentifiedEntity
  {
    private Dictionary<string, T> Dict;
    private List<GTFSUnparsedEntity> Unparsed;

    /// <summary>
    ///   The number of entities within this collection.
    /// </summary>
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

    /// <summary>
    ///   Creates a new IDEntityCollection from the given collection of
    ///   existing objects.
    /// </summary>
    public IDEntityCollection(ICollection<T> objects)
    {
      Dict = new Dictionary<string, T>();
      Unparsed = new List<GTFSUnparsedEntity>();

      foreach (T item in objects)
      {
        Dict.Add(item.ID, item);
      }
    }

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains the given entity.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given entity's ID.
    /// </remarks>
    public bool Contains(T item) => Dict.ContainsKey(item.ID);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a given key.
    /// </summary>
    public bool Contains(string key) => Dict.ContainsKey(key);

    /// <summary>
    ///   Returns an enumerator over the objects within the collection.
    /// </summary>
    public IEnumerator<T> GetEnumerator()
      => Dict.Values.GetEnumerator();

    /// <summary>
    ///   Returns a list of <see cref="GTFSUnparsedEntity" />s created
    ///   from the data in this table.
    /// </summary>
    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();

    /// <summary>
    ///    Returns the entity with this key, if one is stored. Otherwise,
    ///    returns <c>null</c>.
    /// </summary>
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

  /// <summary>
  ///   A method that takes the properties of an entity and outputs the
  ///   entity with those properties.
  /// </summary>
  public delegate T GTFSEntityFactory<T>(IEnumerable<(string, string)> props) where T : GTFSEntity;
}