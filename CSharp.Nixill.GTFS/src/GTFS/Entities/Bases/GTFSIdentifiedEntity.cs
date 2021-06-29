using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class GTFSIdentifiedEntity : GTFSEntity
  {
    public readonly string ID;

    public GTFSIdentifiedEntity(GTFSPropertyCollection properties, string idName) : base(properties)
    {
      ID = properties[idName];
    }
  }
}