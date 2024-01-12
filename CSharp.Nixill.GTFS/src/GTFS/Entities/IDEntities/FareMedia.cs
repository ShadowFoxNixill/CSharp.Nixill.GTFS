using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class FareMedia : GTFSIdentifiedEntity
  {
    public FareMedia(GTFSPropertyCollection properties) : base(properties, "fare_media_id") { }

    public string Name => Properties["fare_media_name"];
    public FareMediaType Type => (FareMediaType)Properties.GetInt("fare_media_type");
  }
}