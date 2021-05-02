using System;
using System.Collections.Generic;

namespace Nixill.GTFS.Entities
{
  public class GTFSUnparsedEntity : GTFSEntity
  {
    public readonly Exception Exception;

    public GTFSUnparsedEntity(GTFSFeed feed, IEnumerable<(string, string)> properties, Exception ex) : base(feed, properties)
    {
      Exception = ex;
    }
  }
}