using System.Collections;
using System.Collections.Generic;
using Nixill.Collections;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  public class GTFSOrderedEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSOrderedEntity
  {
    private Dictionary<string, AVLTreeDictionary<int, T>> Backing;
    private List<GTFSUnparsedEntity> Unparsed;

    /// <summary>
    ///   The number of entities within this collection.
    /// </summary>
    public int Count { get; }

    /// <summary>
    ///   Creates a new GTFSOrderedEntityCollection from the given table in the
    ///   given feed.
    /// </summary>
    public GTFSOrderedEntityCollection(IGTFSDataSource source, string tableName, GTFSEntityFactory<T> factory)
    {
      Backing = new Dictionary<string, AVLTreeDictionary<int, T>>();
      var backingGen = new DictionaryGenerator<string, AVLTreeDictionary<int, T>>(Backing,
        new FuncGenerator<string, AVLTreeDictionary<int, T>>(x => new AVLTreeDictionary<int, T>()));
      Unparsed = new List<GTFSUnparsedEntity>();

      foreach (T item in source.GetObjects(tableName, factory, Unparsed))
      {
        backingGen[item.ID].Add(item.Index, item);
      }
    }

    /// <summary>
    ///   Creates a new IDEntityCollection from the given collection of
    ///   existing objects.
    /// </summary>
    public GTFSOrderedEntityCollection(ICollection<T> objects)
    {
      Backing = new Dictionary<string, AVLTreeDictionary<int, T>>();
      var backingGen = new DictionaryGenerator<string, AVLTreeDictionary<int, T>>(Backing,
        new FuncGenerator<string, AVLTreeDictionary<int, T>>(x => new AVLTreeDictionary<int, T>()));
      Unparsed = new List<GTFSUnparsedEntity>();

      foreach (T item in objects)
      {
        backingGen[item.ID].Add(item.Index, item);
      }
    }

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains the given entity.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given entity's ID and index.
    /// </remarks>
    public bool Contains(T item) => Backing.TryGetValue(item.ID, out var middle) && middle.ContainsKey(item.Index);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a given key.
    /// </summary>
    public bool Contains((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsKey(key.Index);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a given collection.
    /// </summary>
    /// <remarks>
    ///   More specifically, returns <c>true</c> iff the collection
    ///   contains any entity with the given ID.
    /// </remarks>
    public bool Contains(string key) => Backing.ContainsKey(key);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a key lower than
    ///   a given key.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given ID and has a lower Index.
    /// </remarks>
    public bool ContainsLower((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsLower(key.Index);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a key lower than
    ///   or equal to a given key.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given ID and has an equal or lower Index.
    /// </remarks>
    public bool ContainsFloor((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsFloor(key.Index);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a key higher
    ///   than or equal to a given key.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given ID and has an equal or higher Index.
    /// </remarks>
    public bool ContainsCeiling((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsCeiling(key.Index);

    /// <summary>
    ///   Returns <c>true</c> iff the collection contains a key higher
    ///   than a given key.
    /// </summary>
    /// <remarks>
    ///   More specifically, this is <c>true</c> iff the collection has a
    ///   key that matches the given ID and has a higher Index.
    /// </remarks>
    public bool ContainsHigher((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsHigher(key.Index);

    private IEnumerable<T> Enumerable()
    {
      foreach (AVLTreeDictionary<int, T> dict in Backing.Values)
      {
        foreach (T val in dict.Values)
        {
          yield return val;
        }
      }
    }

    public IEnumerator<T> GetEnumerator() => Enumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Enumerable().GetEnumerator();
  }
}