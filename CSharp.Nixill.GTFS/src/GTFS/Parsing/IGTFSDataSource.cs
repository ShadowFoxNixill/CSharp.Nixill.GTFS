using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Parsing
{
  public interface IGTFSDataSource
  {
    public IEnumerable<T> GetObjects<T>(string table, GTFSEntityFactory<T> factory, List<GTFSUnparsedEntity> unparsed = null) where T : GTFSEntity;
  }
}
