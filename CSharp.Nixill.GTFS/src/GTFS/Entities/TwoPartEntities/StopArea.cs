using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class StopArea : GTFSTwoPartEntity<string, string>
  {
    public string AreaID => Properties["area_id"];
    public string StopID => Properties["stop_id"];

    public StopArea(GTFSPropertyCollection properties) : base(properties, properties["area_id"], properties["stop_id"]) { }
  }
}