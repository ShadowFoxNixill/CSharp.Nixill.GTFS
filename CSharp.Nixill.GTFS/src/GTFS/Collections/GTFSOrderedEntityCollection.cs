using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    ///   The element with the given ID, and the highest equal-or-lower
    ///   index. If no equal or lower index exists, returns null.
    /// </summary>
    public T this[string id, int index]
    {
      get
      {
        if (Backing.TryGetValue(id, out var second))
        {
          if (second.TryGetFloorEntry(index, out var result)) return result.Value;
        }

        return null;
      }
    }

    /// <summary>
    ///   All of the elements with a given ID, if any exist. Otherwise,
    ///   null.
    /// </summary>
    public IReadOnlyNavigableDictionary<int, T> this[string id]
    {
      get
      {
        if (Backing.TryGetValue(id, out var second))
          return new OrderedListView() { Backer = second };

        return null;
      }
    }

    /// <summary>
    ///   Creates a new GTFSOrderedEntityCollection from the given table
    ///   in the given feed.
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

    private class OrderedListView : IReadOnlyNavigableDictionary<int, T>
    {
      public AVLTreeDictionary<int, T> Backer;

      public T this[int key]
      {
        get
        {
          var nodes = Backer.EntriesAround(key);
          if (nodes.HasEqualValue) return nodes.EqualValue.Value;
          else if (nodes.HasLesserValue) return nodes.LesserValue.Value;
          else return null;
        }
      }


      public IEnumerable<int> Keys => ((IDictionary<int, T>)Backer).Keys;

      public IEnumerable<T> Values => ((IDictionary<int, T>)Backer).Values;

      public int Count => ((ICollection<KeyValuePair<int, T>>)Backer).Count;

      public KeyValuePair<int, T> CeilingEntry(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).CeilingEntry(from);
      }

      public int CeilingKey(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).CeilingKey(from);
      }

      public bool ContainsCeiling(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).ContainsCeiling(from);
      }

      public bool ContainsFloor(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).ContainsFloor(from);
      }

      public bool ContainsHigher(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).ContainsHigher(from);
      }

      public bool ContainsKey(int key)
      {
        return ((IDictionary<int, T>)Backer).ContainsKey(key);
      }

      public bool ContainsLower(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).ContainsLower(from);
      }

      public KeyValuePair<int, T> FloorEntry(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).FloorEntry(from);
      }

      public int FloorKey(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).FloorKey(from);
      }

      public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
      {
        return ((IEnumerable<KeyValuePair<int, T>>)Backer).GetEnumerator();
      }

      public KeyValuePair<int, T> HigherEntry(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).HigherEntry(from);
      }

      public int HigherKey(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).HigherKey(from);
      }

      public KeyValuePair<int, T> HighestEntry()
      {
        return ((INavigableDictionary<int, T>)Backer).HighestEntry();
      }

      public int HighestKey()
      {
        return ((INavigableDictionary<int, T>)Backer).HighestKey();
      }

      public KeyValuePair<int, T> LowerEntry(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).LowerEntry(from);
      }

      public int LowerKey(int from)
      {
        return ((INavigableDictionary<int, T>)Backer).LowerKey(from);
      }

      public KeyValuePair<int, T> LowestEntry()
      {
        return ((INavigableDictionary<int, T>)Backer).LowestEntry();
      }

      public int LowestKey()
      {
        return ((INavigableDictionary<int, T>)Backer).LowestKey();
      }

      public bool TryGetCeilingEntry(int from, out KeyValuePair<int, T> value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetCeilingEntry(from, out value);
      }

      public bool TryGetCeilingKey(int from, out int value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetCeilingKey(from, out value);
      }

      public bool TryGetFloorEntry(int from, out KeyValuePair<int, T> value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetFloorEntry(from, out value);
      }

      public bool TryGetFloorKey(int from, out int value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetFloorKey(from, out value);
      }

      public bool TryGetHigherEntry(int from, out KeyValuePair<int, T> value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetHigherEntry(from, out value);
      }

      public bool TryGetHigherKey(int from, out int value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetHigherKey(from, out value);
      }

      public bool TryGetLowerEntry(int from, out KeyValuePair<int, T> value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetLowerEntry(from, out value);
      }

      public bool TryGetLowerKey(int from, out int value)
      {
        return ((INavigableDictionary<int, T>)Backer).TryGetLowerKey(from, out value);
      }

      public bool TryGetValue(int key, [MaybeNullWhen(false)] out T value)
      {
        return ((IDictionary<int, T>)Backer).TryGetValue(key, out value);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return ((IEnumerable)Backer).GetEnumerator();
      }
    }
  }
}