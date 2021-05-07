using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

namespace Nixill.GTFS.Parsing
{
  /// <summary>
  ///   Interface for GTFS data sources.
  /// </summary>
  public interface IGTFSDataSource
  {
    /// <summary>
    ///   Returns an enumerable collection of objects parsed from the
    ///   given table in the data source.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Objects that throw an exception upon their instantiation
    ///     should become unparsed entities in <c>unparsed</c>, if a list
    ///     is provided (otherwise, the exception can be propogated
    ///     up the call stack).
    ///   </para>
    /// </remarks>
    public IEnumerable<T> GetObjects<T>(string table, GTFSEntityFactory<T> factory, List<GTFSUnparsedEntity> unparsed = null) where T : GTFSEntity;
  }
}
