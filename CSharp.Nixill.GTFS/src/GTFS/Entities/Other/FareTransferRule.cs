using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class FareTransferRule : GTFSEntity
  {
    public FareTransferRule(GTFSPropertyCollection properties) : base(properties) { }

    public string FromLegGroupID => Properties["from_leg_group_id"];
    public string ToLegGroupID => Properties["to_leg_group_id"];
    public int TransferCount => Properties.GetInt("transfer_count");
    public Duration? DurationLimit => Properties.GetNullableDuration("duration_limit");
    public DurationLimitType DurationLimitType => (DurationLimitType)Properties.GetInt("duration_limit_type");
    public FareTransferType FareTransferType => (FareTransferType)Properties.GetInt("fare_transfer_type");
    public string FareProductID => Properties["fare_product_id"];
  }
}