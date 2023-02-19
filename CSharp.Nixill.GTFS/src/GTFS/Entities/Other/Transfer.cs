using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Transfer : GTFSEntity
  {
    public Transfer(GTFSPropertyCollection properties) : base(properties) { }

    public string FromStopID => Properties["from_stop_id"];
    public string ToStopID => Properties["to_stop_id"];
    public string FromRouteID => Properties["from_route_id"];
    public string ToRouteID => Properties["to_route_id"];
    public string FromTripID => Properties["from_trip_id"];
    public TransferType TransferType => (TransferType)Properties.GetInt("transfer_type", 0);
  }
}