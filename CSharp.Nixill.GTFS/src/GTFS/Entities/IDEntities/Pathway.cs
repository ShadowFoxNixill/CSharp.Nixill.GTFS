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
    public decimal? Length => Properties.GetNullableNonNegativeDecimal("length");
    public Duration? TraversalTime => Properties.GetNullableDuration("traversal_time");
    public int? StairCount => Properties.GetNullableInt("stair_count");
    public decimal? MaxSlope => Properties.GetNullableDecimal("max_slope");
    public decimal? MinWidth => Properties.GetNullableNonNegativeDecimal("min_width");
    public string SignpostedAs => Properties["signposted_as"];
    public string RevesredSignpostedAs => Properties["reversed_signposted_as"];

    public Pathway(GTFSPropertyCollection properties) : base(properties, "pathway_id") { }
  }
}