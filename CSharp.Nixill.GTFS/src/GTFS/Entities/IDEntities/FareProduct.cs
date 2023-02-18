using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class FareProduct : GTFSIdentifiedEntity
  {
    public FareProduct(GTFSPropertyCollection properties) : base(properties, "fare_product_id") { }

    public string Name => Properties["fare_product_name"];
    public decimal Amount => Properties.GetDecimal("amount");
    public string Currency => Properties["currency"];
  }
}
