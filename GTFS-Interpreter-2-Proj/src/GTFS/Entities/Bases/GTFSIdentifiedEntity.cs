using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A <see cref="GTFSEntity"> with a simple, identifier-based primary key.
  /// </summary>
  public abstract class GTFSIdentifiedEntity : GTFSEntity
  {
    /// <summary>
    ///   The ID of this entity, corresponding to a specific property.
    /// </summary>
    /// <remarks>
    ///   See the class definition for which property corresponds to this key.
    /// </remarks>
    public readonly string ID;

    /// <summary>
    ///   Creates a <c>GTFSIdentifiedEntity</c>.
    /// </summary>
    /// <param name="feed">The feed containing this entity.</param>
    /// <param name="properties">
    ///   The entity's collection of properties.
    /// </param>
    /// <param name="idName">
    ///   Which property corresponds to the ID of the entity.
    /// </param>
    protected GTFSIdentifiedEntity(GTFSFeed feed, GTFSPropertyCollection properties, string idName) : base(feed, properties)
    {
      ID = properties[idName];
    }
  }
}