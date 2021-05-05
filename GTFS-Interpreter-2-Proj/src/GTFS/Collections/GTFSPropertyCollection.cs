using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nixill.GTFS.Collections
{
  /// <summary>
  ///   A collection of keys and values specified for a given GTFS Entity,
  ///   representing a single row from a table.
  /// </summary>
  /// <remarks>
  ///   This class is similar to a read-only
  ///   <see cref="Dictionary{string, string}" />, except that values of
  ///   <c>null</c> or <c>""</c> are ignored and their keys discarded.
  ///   Additionally, attempting to read the value of a key that doesn't
  ///   exist simply returns <c>null</c> rather than throwing an exception.
  /// </remarks>
  public class GTFSPropertyCollection : IReadOnlyDictionary<string, string>
  {
    private Dictionary<string, string> Backing;

    /// <summary>
    ///   Creates a new <c>GTFSPropertyCollection</c> from a dictionary of
    ///   keys and values.
    /// </summary>
    /// <param name="input">The keys and values to use.</param>
    /// <param name="agencyId">
    ///   The default <c>agency_id</c> to use. If specified, and there's
    ///   no <c>agency_id</c> in the input, this value will be used for
    ///   that key.
    /// </param>
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

    /// <summary>
    ///   Creates a new <c>GTFSPropertyCollection</c> from an enumerable
    ///   set of two-string tuples.
    /// </summary>
    /// <param name="enumerable">The keys and values to use.</param>
    /// <param name="agencyId">
    ///   The default <c>agency_id</c> to use. If specified, and there's
    ///   no <c>agency_id</c> in the input, this value will be used for
    ///   that key.
    /// </param>
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

    /// <summary>
    ///   Returns the property with the specified key.
    /// </summary>
    /// <remarks>
    ///   If the key wasn't in the input data, or its value was
    ///   <c>null</c> or empty string, <c>null</c> is returned.
    /// </remarks>
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

    /// <summary>
    ///   Returns an enumerable set of the keys of this collection.
    /// </summary>
    /// <remarks>
    ///   Keys whose values were <c>null</c> or empty string are not included.
    /// </remarks>
    public IEnumerable<string> Keys => Backing.Keys;

    /// <summary>
    ///   Returns an enumerable set of the values of this collection.
    /// </summary>
    /// <remarks>
    ///   Values that were <c>null</c> or empty string are not included.
    /// </remarks>
    public IEnumerable<string> Values => Backing.Values;

    /// <summary>
    ///   Returns the number of key-value pairs in this collection.
    /// </summary>
    /// <remarks>
    ///   This may be lower than the <c>Count</c> of the input collection,
    ///   as values that are <c>null</c> or empty string are not included.
    /// </remarks>
    public int Count => Backing.Count;

    /// <summary>
    ///   Whether or not this collection contains a key with the given name.
    /// </summary>
    /// <remarks>
    ///   This method may return <c>false</c> for keys that had been in
    ///   the input collection, iff their values were <c>null</c> or empty
    ///   string.
    /// </remarks>
    public bool ContainsKey(string key) => Backing.ContainsKey(key);

    /// <summary>
    ///   Returns an iterator over the key-value pairs in this collection.
    /// </summary>
    /// <remarks>
    ///   Key-value pairs where the value is <c>null</c> or empty string
    ///   are not included.
    /// </remarks>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Backing.GetEnumerator();

    /// <summary>
    ///   Attempts to get the value associated with the specified key.
    /// </summary>
    /// <remarks>
    ///   This method may return <c>false</c> for keys that had been in
    ///   the input collection, iff their values were <c>null</c> or empty
    ///   string.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="value">
    ///   When this method returns, if the key was found, this variable
    ///   contains the value for that key. Otherwise, it contains <c>null</c>.
    /// </param>
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) => Backing.TryGetValue(key, out value);

    /// override
    IEnumerator IEnumerable.GetEnumerator() => Backing.GetEnumerator();
  }
}