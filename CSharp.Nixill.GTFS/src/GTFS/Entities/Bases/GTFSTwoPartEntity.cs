using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class GTFSTwoPartEntity<TKey1, TKey2> : GTFSEntity
  {
    public readonly TKey1 FirstKey;

    public readonly TKey2 SecondKey;

    public GTFSTwoPartEntity(GTFSPropertyCollection properties, TKey1 firstKey, TKey2 secondKey)
      : base(properties)
    {
      FirstKey = firstKey;
      SecondKey = secondKey;
    }
  }
}