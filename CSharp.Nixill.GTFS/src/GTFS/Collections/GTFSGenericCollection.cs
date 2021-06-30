using System.Collections;
using System.Collections.Generic;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Sources;

namespace Nixill.GTFS.Collections
{
  public class GTFSGenericCollection<T> : IReadOnlyCollection<T> where T : GTFSEntity
  {
    private List<T> Backing;
    private List<GTFSUnparsedEntity> Unparsed;

    public GTFSGenericCollection(IGTFSDataSource source, string tableName, GTFSEntityFactory<T> factory)
    {
      Backing = new();
      Unparsed = new();

      foreach (T item in source.GetObjects(tableName, factory, Unparsed))
      {
        Backing.Add(item);
      }
    }

    public GTFSGenericCollection(IEnumerable<T> objects)
    {
      Backing = new();
      Unparsed = new();

      foreach (T item in objects)
      {
        Backing.Add(item);
      }
    }

    public int Count => Backing.Count;
    public IEnumerator<T> GetEnumerator() => Backing.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Backing.GetEnumerator();

    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsed() =>
      Unparsed.AsReadOnly();
  }
}