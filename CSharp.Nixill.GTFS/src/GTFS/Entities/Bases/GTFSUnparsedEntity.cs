using System;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class GTFSUnparsedEntity : GTFSEntity
  {
    public readonly Exception Exception;

    public GTFSUnparsedEntity(GTFSPropertyCollection properties, Exception ex) : base(properties)
    {
      Exception = ex;
    }
  }
}