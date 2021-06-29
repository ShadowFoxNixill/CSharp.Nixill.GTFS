using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class GTFSOrderedEntity : GTFSTwoPartEntity<string, int>
  {
    public readonly string ID;

    public readonly int Index;

    public GTFSOrderedEntity(GTFSPropertyCollection properties, string idKey, string indexKey) : base(properties, properties[idKey], properties.GetInt(indexKey))
    {
      ID = FirstKey;
      Index = SecondKey;
    }
  }
}