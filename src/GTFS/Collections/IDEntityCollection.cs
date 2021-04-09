using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nixill.GTFS.Collections
{
  public class IDEntityCollection<T> : IReadOnlyDictionary<string, T> where T : GTFSIdentifiedEntity
  {
    internal Dictionary<string, T> Dict;
    public readonly GTFSFile File;

    // public methods
    public T this[string key] => Dict[key];
    public IEnumerable<string> Keys => Dict.Keys;
    public IEnumerable<T> Values => Dict.Values;
    public int Count => Dict.Count;
    public bool ContainsKey(string key) => Dict.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => Dict.GetEnumerator();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out T value) => Dict.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict).GetEnumerator();

    // internal methods for writing
    internal void Add(T )
  }
}