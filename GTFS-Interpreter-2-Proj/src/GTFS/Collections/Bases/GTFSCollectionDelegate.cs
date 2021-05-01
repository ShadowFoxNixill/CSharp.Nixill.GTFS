using System.Collections.Generic;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Collections
{
  public delegate ICollection<GTFSEntity> CollectionFactory(GTFSFeed feed, Dictionary<string, string> properties);
}