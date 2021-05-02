using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nixill.GTFS.Collections
{
  public class GTFSPropertyCollection : IReadOnlyDictionary<string, string>
  {
    private Dictionary<string, string> Backing;

    public GTFSPropertyCollection(IDictionary<string, string> input, string agencyId = null)
    {
      Backing = new Dictionary<string, string>();

      foreach (string key in input.Keys)
      {
        if (input[key] != null && input[key] != "") Backing[key] = input[key];
      }

      if ((agencyId != null) && !Backing.ContainsKey("agency_id"))
      {
        Backing["agency_id"] = agencyId;
      }
    }

    public GTFSPropertyCollection(IEnumerable<(string Key, string Value)> enumerable, string agencyId = null)
    {
      Backing = new Dictionary<string, string>();

      foreach ((string Key, string Value) tuple in enumerable)
      {
        if (tuple.Value != null && tuple.Value != "") Backing[tuple.Key] = tuple.Value;
      }

      if ((agencyId != null) && !Backing.ContainsKey("agency_id"))
      {
        Backing["agency_id"] = agencyId;
      }
    }

    public string this[string key]
    {
      get
      {
        if (Backing.ContainsKey(key))
        {
          return Backing[key];
        }
        else return null;
      }
    }

    public IEnumerable<string> Keys => Backing.Keys;

    public IEnumerable<string> Values => Backing.Values;

    public int Count => Backing.Count;

    public bool ContainsKey(string key) => Backing.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Backing.GetEnumerator();

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) => Backing.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => Backing.GetEnumerator();
  }
}