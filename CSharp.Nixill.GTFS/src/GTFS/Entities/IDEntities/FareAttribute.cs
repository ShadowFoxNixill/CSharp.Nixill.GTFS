using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class FareAttribute : GTFSIdentifiedEntity
  {
    public FareAttribute(GTFSPropertyCollection properties) : base(properties, "fare_id") { }

    public decimal Price => Properties.GetDecimal("price");
    public string CurrencyType => Properties["currency_type"];
    public FarePaymentMethod PaymentMethod => (FarePaymentMethod)Properties.GetInt("payment_method");
    public int? Transfers => Properties.GetNullableInt("transfers");
    public string AgencyID => Properties["agency_id"];
    public Duration? TransferDuration => Properties.GetNullableDuration("transfer_duration");

    public static GTFSEntityFactory<FareAttribute> GetFactory(string defaultAgencyID) =>
      (properties) => new FareAttribute(new GTFSPropertyCollection(properties, defaultAgencyID));
  }
}