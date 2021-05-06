using System.Collections.Generic;
using System.Linq;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A single entity from a GTFS feed.
  /// </summary>
  public abstract class GTFSEntity
  {
    /// <summary>
    ///   The <see cref="GTFSFeed" /> from which this entity was generated.
    /// </summary>
    public readonly GTFSFeed Feed;

    /// <summary>
    ///   The raw view of the properties of this entity.
    /// </summary>
    protected GTFSPropertyCollection Properties;

    /// <summary>
    ///   Creates a new <c>GTFSEntity</c>.
    /// </summary>
    /// <param name="feed">The feed containing this entity.</param>
    /// <param name="properties">
    ///   The entity's collection of properties.
    /// </param>
    protected GTFSEntity(GTFSFeed feed, GTFSPropertyCollection properties)
    {
      Feed = feed;
      Properties = properties;
    }

    /// <summary>
    ///   Returns the given property of this entity, or null if no such
    ///   property exists.
    /// </summary>
    public string this[string key] => Properties[key];
  }
}