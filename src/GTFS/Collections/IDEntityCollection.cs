using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Collections
{
  public class IDEntityCollection<T> : ICollection<T> where T : GTFSIdentifiedEntity
  {
    internal Dictionary<string, T> Dict;
    public readonly GTFSFeed File;

    public int Count => Dict.Count;
    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator() => Dict.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict.Values).GetEnumerator();

    public void Add(T entity) => Dict.Add(entity.ID, entity);
    public void Clear() => Dict.Clear();
    public bool Contains(T item) => Dict.ContainsKey(item.ID);
    public bool Contains(string key) => Dict.ContainsKey(key);
    public void CopyTo(T[] array, int arrayIndex) => Dict.Values.CopyTo(array, arrayIndex);
    public bool Remove(T item) => Dict.Remove(item.ID);
  }
}