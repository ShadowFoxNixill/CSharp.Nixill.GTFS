using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Area : GTFSIdentifiedEntity
  {
    public string Name => Properties["area_name"];

    public Area(GTFSPropertyCollection properties) : base(properties, "area_id") { }
  }
}