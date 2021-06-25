using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSIdentifiedEntity : GTFSEntity
  {
    public readonly string ID;

    protected GTFSIdentifiedEntity(GTFSPropertyCollection properties, string idName) : base(properties)
    {
      ID = properties[idName];
    }
  }
}