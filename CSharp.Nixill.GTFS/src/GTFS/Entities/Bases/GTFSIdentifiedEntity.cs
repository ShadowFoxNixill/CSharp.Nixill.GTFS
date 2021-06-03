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
    ///   See the class definition for which property is the value of this key.
    /// </remarks>
    public readonly string ID;

    /// <summary>
    ///   Creates a <c>GTFSIdentifiedEntity</c>.
    /// </summary>
    /// <param name="properties">
    ///   The entity's collection of properties.
    /// </param>
    /// <param name="idName">
    ///   Which property is the value of the ID of the entity.
    /// </param>
    protected GTFSIdentifiedEntity(GTFSPropertyCollection properties, string idName) : base(properties)
    {
      ID = properties[idName];
    }
  }
}