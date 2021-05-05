using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Parsing
{
  public interface IGTFSDataSource
  {
    public IEnumerable<T> GetObjects<T>(GTFSFeed feed, string table, GTFSEntityFactory<T> factory, List<GTFSUnparsedEntity> unparsed) where T : GTFSEntity;
  }
}
