using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSOrderedEntity : GTFSTwoPartEntity<string, int>
  {
    /// <summary>
    ///   The ID of this entity, corresponding to a specific property.
    /// </summary>
    /// <remarks>
    ///   See the class definition for which property is the value of this key.
    /// </remarks>
    public readonly string ID;

    /// <summary>
    ///   The index of this entity, corresponding to a specific property.
    /// </summary>
    /// <remarks>
    ///   See the class definition for which property is the value of this key.
    /// </remarks>
    public readonly int Index;

    /// <summary>
    ///   Creates a <c>GTFSIdentifiedEntity</c>.
    /// </summary>
    /// <param name="properties">
    ///   The entity's collection of properties.
    /// </param>
    /// <param name="idKey">
    ///   Which property is the value of the ID of the entity.
    /// </param>
    /// <param name="indexKey">
    ///   Which property is the value of the index of the entity.
    /// </param>
    public GTFSOrderedEntity(GTFSPropertyCollection properties, string idKey, string indexKey) : base(properties, properties[idKey], properties.GetInt(indexKey))
    {
      ID = FirstKey;
      Index = SecondKey;
    }
  }
}