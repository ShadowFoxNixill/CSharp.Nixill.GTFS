using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Level : GTFSIdentifiedEntity
  {
    public decimal LevelIndex => Properties.GetDecimal("level_index");
    public string LevelName => Properties["level_name"];

    public Level(GTFSPropertyCollection properties) : base(properties, "level_id") { }
  }
}