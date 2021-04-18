using System.IO;
using System.Collections.Generic;
using System.Drawing;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using System.Linq;
using Nixill.Utils;

namespace Nixill.GTFS.Entities
{
  public class Route : GTFSIdentifiedEntity
  {
    public string AgencyID => Properties.AgencyId(Feed);
    public string ShortName => Properties.GetOrNull("route_short_name");
    public string LongName => Properties.GetOrNull("route_long_name");
    public string Description => Properties.GetOrNull("route_desc");
    public RouteType Type => (RouteType)int.Parse(Properties["route_type"]);
    public string Url => Properties.GetOrNull("route_url");
    public Color? RouteColor => GTFSObjectParser.GetColorQM(Properties.GetOrNull("route_color"));
    public Color? TextColor => GTFSObjectParser.GetColorQM(Properties.GetOrNull("route_text_color"));
    public int? SortOrder => GTFSObjectParser.GetNullableInt(Properties.GetOrNull("route_sort_order"));
    public PickupDropoffType ContinuousPickup => (PickupDropoffType)int.Parse(Properties.DefaultProperty("continuous_pickup", "1"));
    public PickupDropoffType ContinuousDropoff => (PickupDropoffType)int.Parse(Properties.DefaultProperty("continuous_drop_off", "1"));

    public Agency Agency => Feed.Agencies[AgencyID];

    private Route(GTFSFeed feed, Dictionary<string, string> properties) : base(feed, properties, "route_id")
    {
      if ((!properties.ContainsKey("route_short_name") || (properties["route_short_name"] == ""))
      && (!properties.ContainsKey("route_long_name") || (properties["route_long_name"] == "")))
        throw new InvalidDataException("Routes must have either a long name or a short name.");
    }

    public static Route Factory(GTFSFeed feed, Dictionary<string, string> properties) => new Route(feed, properties);
  }

  public static class RouteAgencyExtensions
  {
    public static IEnumerable<Route> Routes(this Agency agency) => agency.Feed.Routes.Where(x => x.AgencyID == agency.ID);
  }
}