using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class FareLegRule : GTFSEntity
  {
    public FareLegRule(GTFSPropertyCollection properties) : base(properties) { }

    public string LegGroupID => Properties["leg_group_id"];
    public string NetworkID => Properties["network_id"];
    public string FromAreaID => Properties["from_area_id"];
    public string ToAreaID => Properties["to_area_id"];
    public string FareProductID => Properties["fare_product_id"];
  }
}