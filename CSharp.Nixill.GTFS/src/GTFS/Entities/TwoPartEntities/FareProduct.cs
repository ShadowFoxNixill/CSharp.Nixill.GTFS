using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class FareProduct : GTFSTwoPartEntity<string, string>
  {
    public FareProduct(GTFSPropertyCollection properties) : base(properties, properties["fare_product_id"], properties.GetValueOrDefault("fare_media_id", "")) { }

    public string ID => Properties["fare_product_id"];
    public string FareMediaID => Properties["fare_media_id"];
    public string Name => Properties["fare_product_name"];
    public decimal Amount => Properties.GetDecimal("amount");
    public string Currency => Properties["currency"];
  }
}
