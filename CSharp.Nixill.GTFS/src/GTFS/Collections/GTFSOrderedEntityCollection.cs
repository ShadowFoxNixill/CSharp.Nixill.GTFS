using System.Collections;
using System.Collections.Generic;
using Nixill.Collections;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Sources;

namespace Nixill.GTFS.Collections
{
  public class GTFSOrderedEntityCollection<T> : IReadOnlyCollection<T> where T : GTFSOrderedEntity
  {
    private Dictionary<string, AVLTreeDictionary<int, T>> Backing;
    private List<GTFSUnparsedEntity> Unparsed;

    public int Count { get; }

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

    public IReadOnlyNavigableDictionary<int, T> this[string id]
    {
      get
      {
        if (Backing.TryGetValue(id, out var second))
          return second.AsReadOnly();

        return null;
      }
    }

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

    public GTFSOrderedEntityCollection(IEnumerable<T> objects)
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

    public bool Contains(T item) => Backing.TryGetValue(item.ID, out var middle) && middle.ContainsKey(item.Index);
    public bool Contains((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsKey(key.Index);
    public bool Contains(string key) => Backing.ContainsKey(key);

    public bool ContainsLower((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsLower(key.Index);
    public bool ContainsFloor((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsFloor(key.Index);
    public bool ContainsCeiling((string ID, int Index) key) =>
      Backing.TryGetValue(key.ID, out var middle) && middle.ContainsCeiling(key.Index);
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
    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();
  }
}