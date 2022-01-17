using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Pathway : GTFSIdentifiedEntity
  {
    public string FromStopID => Properties["from_stop_id"];
    public string ToStopID => Properties["to_stop_id"];
    public PathwayMode PathwayMode => (PathwayMode)Properties.GetInt("pathway_mode");
    public bool IsBidirectional => Properties.GetBool("is_bidirectional");
    public double? Length => Properties.GetNullableNonNegativeDouble("length");
    public Duration? TraversalTime => Properties.GetNullableDuration("traversal_time");
    public int? StairCount => Properties.GetNullableInt("stair_count");
    public double? MaxSlope => Properties.GetNullableDouble("max_slope");
    public double? MinWidth => Properties.GetNullableNonNegativeDouble("min_width");
    public string SignpostedAs => Properties["signposted_as"];
    public string RevesredSignpostedAs => Properties["reversed_signposted_as"];

    public Pathway(GTFSPropertyCollection properties) : base(properties, "pathway_id") { }
  }
}