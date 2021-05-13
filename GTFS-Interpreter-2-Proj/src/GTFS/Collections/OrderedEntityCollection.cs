using System.Collections;
using System.Collections.Generic;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Collections
{
  /// <summary>
  ///   A collection of <see cref="GTFSIdentifiedEntity" />s, accessible
  ///   by their IDs and indexes.
  /// </summary>
  /// <typeparam name="T">
  ///   The type of entity that this collection contains.
  /// </typeparam>
  public class OrderedEntityCollection<T> : ICollection<T> where T : GTFSOrderedEntity
  {
    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(T item)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(T item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(T item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}