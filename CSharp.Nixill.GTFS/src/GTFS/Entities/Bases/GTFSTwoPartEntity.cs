using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSTwoPartEntity<TKey1, TKey2> : GTFSEntity
  {
    public readonly TKey1 FirstKey;

    public readonly TKey2 SecondKey;

    protected GTFSTwoPartEntity(GTFSPropertyCollection properties, TKey1 firstKey, TKey2 secondKey)
      : base(properties)
    {
      FirstKey = firstKey;
      SecondKey = secondKey;
    }
  }
}