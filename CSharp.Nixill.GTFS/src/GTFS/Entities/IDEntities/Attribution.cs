using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Attribution : GTFSEntity
  {
    public string AttributionID => Properties["attribution_id"];
    public string AgencyID => Properties["agency_id"];
    public string RouteID => Properties["route_id"];
    public string TripID => Properties["trip_id"];
    public string OrganizationName => Properties["organization_name"];
    public bool IsProducer => Properties.GetBool("is_producer");
    public bool IsOperator => Properties.GetBool("is_operator");
    public bool IsAuthority => Properties.GetBool("is_authority");
    public string AttributionURL => Properties["attribution_url"];
    public string AttributionEmail => Properties["attribution_email"];
    public string AttributionPhone => Properties["attribution_phone"];

    public Attribution(GTFSPropertyCollection properties) : base(properties) { }
  }
}