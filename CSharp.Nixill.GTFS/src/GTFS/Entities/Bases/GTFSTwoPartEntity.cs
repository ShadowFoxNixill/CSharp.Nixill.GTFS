using System;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A <see cref="GTFSEntity" /> with a two-part composite key.
  /// </summary>
  /// <typeparam name="TKey1">
  ///   The type of the first key of this entity.
  /// </typeparam>
  /// <typeparam name="TKey2">
  ///   The type of the second key of this entity.
  /// </typeparam>
  public abstract class GTFSTwoPartEntity<TKey1, TKey2> : GTFSEntity
  {
    /// <summary>
    ///   The first key of the entity.
    /// </summary>
    /// <remarks>
    ///   See the class definition for which property is the value of this key.
    /// </remarks>
    public readonly TKey1 FirstKey;

    /// <summary>
    ///   The second key of this entity.
    /// </summary>
    /// <remarks>
    ///   See the class definition for which property is the value of this key.
    /// </remarks>
    public readonly TKey2 SecondKey;

    /// <summary>
    ///   Creates a new <c>GTFSTwoPartEntity</c>.
    /// </summary>
    protected GTFSTwoPartEntity(GTFSPropertyCollection properties, TKey1 firstKey, TKey2 secondKey)
      : base(properties)
    {
      FirstKey = firstKey;
      SecondKey = secondKey;
    }
  }
}