using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nixill.Collections;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS
{
  public class GTFSOrderedEntityCollection<T> : IReadOnlyDictionary<(string, int), T> where T : GTFSOrderedEntity
  {
    private Dictionary<string, AVLTreeDictionary<int, T>> Backing;

    public

    public T this[(string, int) key] => throw new System.NotImplementedException();

    public IEnumerable<(string, int)> Keys => throw new System.NotImplementedException();

    public IEnumerable<T> Values => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public bool ContainsKey((string, int) key)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<KeyValuePair<(string, int), T>> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    public bool TryGetValue((string, int) key, [MaybeNullWhen(false)] out T value)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}