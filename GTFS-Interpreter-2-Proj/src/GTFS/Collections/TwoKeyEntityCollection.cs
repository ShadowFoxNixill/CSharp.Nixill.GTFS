using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Collections
{
  public class TwoKeyEntityCollection<TKey1, TKey2, TEntity> : IReadOnlyCollection<TEntity> where TEntity : GTFSTwoPartEntity<TKey1, TKey2>
  {
    private Dictionary<(TKey1, TKey2), TEntity> Dict;
    private List<GTFSUnparsedEntity> Unparsed;
    public readonly GTFSFeed Feed;
    private GTFSEntityFactory<TEntity> ObjectFactory;

    public int Count => Dict.Count;

    public TwoKeyEntityCollection(GTFSFeed feed, IGTFSDataSource source, string tableName, GTFSEntityFactory<TEntity> factory)
    {
      Dict = new Dictionary<(TKey1, TKey2), TEntity>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;
      ObjectFactory = factory;

      foreach (TEntity item in source.GetObjects(Feed, tableName, ObjectFactory, Unparsed))
      {
        Dict.Add((item.FirstKey, item.SecondKey), item);
      }
    }

    public TwoKeyEntityCollection(GTFSFeed feed, ICollection<TEntity> objects)
    {
      Dict = new Dictionary<(TKey1, TKey2), TEntity>();
      Unparsed = new List<GTFSUnparsedEntity>();
      Feed = feed;
      ObjectFactory = null;

      foreach (TEntity item in objects)
      {
        Dict.Add((item.FirstKey, item.SecondKey), item);
      }
    }

    public IEnumerator<TEntity> GetEnumerator() =>
      Dict.Values.GetEnumerator();

    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();

    IEnumerator IEnumerable.GetEnumerator() =>
      Dict.Values.GetEnumerator();

    public bool Contains(TEntity item) => Dict.ContainsKey((item.FirstKey, item.SecondKey));
    public bool Contains((TKey1, TKey2) key) => Dict.ContainsKey(key);

    public IEnumerable<TEntity> WithFirstKey(TKey1 key) =>
      Dict.Where(x => object.Equals(x.Key.Item1, key))
        .Select(x => x.Value);

    public IEnumerable<TEntity> WithSecondKey(TKey2 key) =>
      Dict.Where(x => object.Equals(x.Key.Item2, key))
        .Select(x => x.Value);

    public IEnumerable<TKey1> FirstKeys =>
      Dict.Keys.Select(x => x.Item1).Distinct();

    public IEnumerable<TKey2> SecondKeys =>
      Dict.Keys.Select(x => x.Item2).Distinct();

    public TEntity this[TKey1 firstKey, TKey2 secondKey] =>
      Dict[(firstKey, secondKey)];
  }
}