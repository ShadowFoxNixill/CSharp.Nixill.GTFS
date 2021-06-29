using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A single entity from a GTFS feed.
  /// </summary>
  public class GTFSEntity
  {
    /// <summary>
    ///   The raw view of the properties of this entity.
    /// </summary>
    protected GTFSPropertyCollection Properties;

    /// <summary>
    ///   Creates a new <c>GTFSEntity</c>.
    /// </summary>
    /// <param name="properties">
    ///   The entity's collection of properties.
    /// </param>
    public GTFSEntity(GTFSPropertyCollection properties)
    {
      Properties = properties;
    }

    /// <summary>
    ///   Returns the given property of this entity, or null if no such
    ///   property exists.
    /// </summary>
    public string this[string key] => Properties[key];
  }
}