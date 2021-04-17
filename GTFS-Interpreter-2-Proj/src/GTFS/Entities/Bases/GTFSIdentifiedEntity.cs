using System.Collections.Generic;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSIdentifiedEntity : GTFSEntity
  {
    public readonly string ID;

    protected GTFSIdentifiedEntity(GTFSFeed feed, Dictionary<string, string> properties, string idName) : base(feed, properties)
    {
      ID = properties[idName];
    }
  }
}