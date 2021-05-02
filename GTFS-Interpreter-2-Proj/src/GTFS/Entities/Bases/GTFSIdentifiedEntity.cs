using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSIdentifiedEntity : GTFSEntity
  {
    public readonly string ID;

    protected GTFSIdentifiedEntity(GTFSFeed feed, GTFSPropertyCollection properties, string idName) : base(feed, properties)
    {
      ID = properties[idName];
    }
  }
}