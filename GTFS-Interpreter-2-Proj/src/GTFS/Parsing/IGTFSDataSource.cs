using System.Collections.Generic;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Parsing
{
  public interface IGTFSDataSource
  {
    public IEnumerable<GTFSPropertyCollection> GetRowsOfTable(string tableName);
  }
}
