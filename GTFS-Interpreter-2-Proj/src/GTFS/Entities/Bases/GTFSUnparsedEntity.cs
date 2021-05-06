using System;
using System.Collections.Generic;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class GTFSUnparsedEntity : GTFSEntity
  {
    public readonly Exception Exception;

    public GTFSUnparsedEntity(GTFSFeed feed, GTFSPropertyCollection properties, Exception ex) : base(feed, properties)
    {
      Exception = ex;
    }
  }
}