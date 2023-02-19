using System.IO;
using System.Drawing;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public class Route : GTFSIdentifiedEntity
  {
    public string AgencyID => Properties["agency_id"];
    public string ShortName => Properties["route_short_name"];
    public string LongName => Properties["route_long_name"];
    public string Description => Properties["route_desc"];
    public RouteType Type => (RouteType)Properties.GetInt("route_type");
    public string Url => Properties["route_url"];
    public Color RouteColor => Properties.GetColor("route_color", Color.White);
    public Color TextColor => Properties.GetColor("route_text_color", Color.Black);
    public int? SortOrder => Properties.GetNullableInt("route_sort_order");
    public PickupDropoffType ContinuousPickup => (PickupDropoffType)Properties.GetInt("continuous_pickup", 1);
    public PickupDropoffType ContinuousDropoff => (PickupDropoffType)Properties.GetInt("continuous_drop_off", 1);
    public string NetworkID => Properties["network_id"];

    public Route(GTFSPropertyCollection properties) : base(properties, "route_id")
    {
      if (!properties.ContainsKey("route_short_name") && !properties.ContainsKey("route_long_name"))
        throw new InvalidDataException("Routes must have either a long name or a short name.");
      if (!properties.IsInt("route_type")) throw new InvalidDataException("Routes must have a type.");
    }
  }
}