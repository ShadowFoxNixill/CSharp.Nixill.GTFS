using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  /// <summary>
  ///   A collection of <see cref="GTFSTwoPartEntity" />s, accessible by
  ///   their two keys.
  /// </summary>
  /// <typeparam name="TEntity">
  ///   The type of entity that this collection contains.
  /// </typeparam>
  /// <typeparam name="TKey1">
  ///   The type of <c>TEntity</c>'s first key.
  /// </typeparam>
  /// <typeparam name="TKey2">
  ///   The type of <c>TEntity</c>'s second key.
  /// </typeparam>
  public class TwoKeyEntityCollection<TKey1, TKey2, TEntity> : IReadOnlyCollection<TEntity> where TEntity : GTFSTwoPartEntity<TKey1, TKey2>
  {
    private Dictionary<(TKey1, TKey2), TEntity> Dict;
    private List<GTFSUnparsedEntity> Unparsed;

    /// <summary>
    ///   The <see cref="GTFSFeed" /> from which this collection was made.
    /// </summary>
    public readonly GTFSFeed Feed;

    /// <summary>
    ///   The number of entities within this collection.
    /// </summary>
    public int Count => Dict.Count;

    /// <summary>
    ///   Creates a new TwoKeyEntityCollection from the given table in the
    ///   given feed.
    /// </summary>
    public TwoKeyEntityCollection(GTFSFeed feed, string tableName, GTFSEntityFactory<TEntity> factory)
    {
      Dict = new Dictionary<(TKey1, TKey2), TEntity>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;

      foreach (TEntity item in feed.DataSource.GetObjects(Feed, tableName, factory, Unparsed))
      {
        Dict.Add((item.FirstKey, item.SecondKey), item);
      }
    }

    /// <summary>
    ///   Creates a new TwoKeyEntityCollection from the given collection
    ///   of existing objects.
    /// </summary>
    public TwoKeyEntityCollection(GTFSFeed feed, ICollection<TEntity> objects)
    {
      Dict = new Dictionary<(TKey1, TKey2), TEntity>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;

      foreach (TEntity item in objects)
      {
        Dict.Add((item.FirstKey, item.SecondKey), item);
      }
    }

    /// <summary>
    ///   Returns an enumerator over the objects within the collection.
    /// </summary>
    public IEnumerator<TEntity> GetEnumerator() =>
      Dict.Values.GetEnumerator();

    /// <summary>
    ///   Returns a list of <see cref="GTFSUnparsedEntity" />s created
    ///   from the data in this table.
    /// </summary>
    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();

    /// <summary>
    ///   Returns true <c>iff</c> the collection contains the given entity.
    /// </summary>
    /// <remakrs>
    ///   More specificall, this is <c>true</c> iff the collection has a
    ///   pair of keys that matches the given entity's keys.
    /// </remarks>
    public bool Contains(TEntity item) => Dict.ContainsKey((item.FirstKey, item.SecondKey));

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a given pair of keys.
    /// </summary>
    public bool Contains((TKey1, TKey2) key) => Dict.ContainsKey(key);

    /// <summary>
    ///   Returns the subset of this collection containing entities with
    ///   the given first key.
    /// </summary>
    /// <remarks>
    ///   If no such entities exist, the iterator will yield no results.
    /// </remarks>
    public IEnumerable<TEntity> WithFirstKey(TKey1 key) =>
      Dict.Where(x => object.Equals(x.Key.Item1, key))
        .Select(x => x.Value);

    /// <summary>
    ///   Returns the subset of this collection containing entities with
    ///   the given second key.
    /// </summary>
    /// <remarks>
    ///   If no such entities exist, the iterator will yield no results.
    /// </remarks>
    public IEnumerable<TEntity> WithSecondKey(TKey2 key) =>
      Dict.Where(x => object.Equals(x.Key.Item2, key))
        .Select(x => x.Value);

    /// <summary>
    ///   Returns the collection of distinct first keys.
    /// </summary>
    public IEnumerable<TKey1> FirstKeys =>
      Dict.Keys.Select(x => x.Item1).Distinct();

    /// <summary>
    ///   Returns the collection of distinct second keys.
    /// </summary>
    public IEnumerable<TKey2> SecondKeys =>
      Dict.Keys.Select(x => x.Item2).Distinct();

    /// <summary>
    ///   Returns the entity with the given pair of keys. If no such
    ///   entity exists, returns <c>null</c>.
    /// </summary>
    public TEntity this[TKey1 firstKey, TKey2 secondKey]
    {
      get
      {
        if (Dict.ContainsKey((firstKey, secondKey))) return Dict[(firstKey, secondKey)];
        else return null;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
      Dict.Values.GetEnumerator();
  }
}