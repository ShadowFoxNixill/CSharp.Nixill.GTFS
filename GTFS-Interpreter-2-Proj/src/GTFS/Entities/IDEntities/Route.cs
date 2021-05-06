using System.IO;
using System.Collections.Generic;
using System.Drawing;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using System.Linq;
using Nixill.Utils;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   Represents a single transit route within a GTFS feed.
  /// </summary>
  /// <remarks>
  ///   The ID for a <c>Route</c> is its <c>route_id</c> property.
  /// </remarks>
  public class Route : GTFSIdentifiedEntity
  {
    /// <summary>
    ///   The ID of the agency for the route.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>agency_id</c> property of the
    ///   entity. If it's is never specified within the feed, this
    ///   property will be equal to the empty string.
    /// </remarks>
    public string AgencyID => Properties["agency_id"];

    /// <summary>
    ///   Short name of a route.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_short_name</c> property of the
    ///   entity. This will often be a short, abstract identifier like
    ///   "32", "100X", or "Green" that riders use to identify a route,
    ///   but which doesn't give any indication of what places the route
    ///   serves. Either <c>ShortName</c> or <c>LongName</c> must be
    ///   specified, or potentially both if appropriate.
    /// </remarks>
    public string ShortName => Properties["route_short_name"];

    /// <summary>
    ///   Full name of a route.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_long_name</c> property of the
    ///   entity. This name is generally more descriptive than the 
    ///   <c>ShortName</c> and often includes the route's destination or
    ///   stop. Either <c>ShortName</c> or <c>LongName</c> must be
    ///   specified, or potentially both if appropriate.
    /// </remarks>
    public string LongName => Properties["route_long_name"];

    /// <summary>
    ///   Description of a route that provides useful, quality information.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_desc</c> property of the entity.
    /// </remarks>
    /// <example>
    ///   "A" trains operate between Inwood-207 St, Manhattan and Far
    ///   Rockaway-Mott Avenue, Queens at all times. Also from about 6AM
    ///   until about midnight, additional "A" trains operate between
    ///   Inwood-207 St and Lefferts Boulevard (trains typically alternate
    ///   between Lefferts Blvd and Far Rockaway).
    /// </example>
    public string Description => Properties["route_desc"];

    /// <summary>
    ///   Indicates the type of transportation used on a route.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_type</c> property of the entity.
    /// </remarks>
    public RouteType Type => (RouteType)Properties.GetInt("route_type");

    /// <summary>
    ///   URL of a web page about the particular route.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_url</c> property of the
    ///   entity. It should be different from the
    ///   <see cref="Agency.Url" /> value.
    /// </remarks>
    public string Url => Properties["route_url"];

    /// <summary>
    ///   Route color designation that matches public facing material.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_color</c> property of the
    ///   entity. It defaults to <see cref="Color.White" /> when empty.
    ///   The color difference between RouteColor and
    ///   <see cref="TextColor" /> should provide sufficient contrast when
    ///   viewed on a black and white screen.
    /// </remarks>
    public Color RouteColor => Properties.GetColor("route_color", Color.White);

    /// <summary>
    ///   Legible color to use for text drawn against a background of
    ///   <see cref="RouteColor" />.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_text_color</c> property of the
    ///   entity. It defaults to <see cref="Color.Black" /> when empty.
    ///   The color difference between <see cref="RouteColor" /> and
    ///   TextColor should provide sufficient contrast when viewed on a
    ///   black and white screen.
    /// </remarks>
    public Color TextColor => Properties.GetColor("route_text_color", Color.Black);

    /// <summary>
    ///   Orders the routes in a way which is ideal for presentation to
    ///   customers.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_sort_order</c> property of the
    ///   entity. Routes with smaller <c>SortOrder</c> values should be
    ///   displayed first.
    /// </remarks>
    public int? SortOrder => Properties.GetNullableInt("route_sort_order");

    /// <summary>
    ///   Indicates whether a rider can board the transit vehicle anywhere
    ///   along the vehicle’s travel path.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>continuous_pickup</c> property of
    ///   the entity. The path is described by shapes.txt on every trip of
    ///   the route. The default continuous pickup behavior is
    ///   <see cref="PickupDropoffType.Unavalable" />, and the behavior
    ///   defined in <c>Route</c>s can be overridden in <c>StopTime</c>s.
    /// </remarks>
    public PickupDropoffType ContinuousPickup => (PickupDropoffType)Properties.GetInt("continuous_pickup", 1);

    /// <summary>
    ///   Indicates whether a rider can alight from the transit vehicle
    ///   anywhere along the vehicle’s travel path.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>continuous_drop_off</c> property of
    ///   the entity. The path is described by shapes.txt on every trip of
    ///   the route. The default continuous drop-off behavior is
    ///   <see cref="PickupDropoffType.Unavalable" />, and the behavior
    ///   defined in <c>Route</c>s can be overridden in <c>StopTime</c>s.
    /// </remarks>
    public PickupDropoffType ContinuousDropoff => (PickupDropoffType)Properties.GetInt("continuous_drop_off", 1);

    /// <summary>
    ///   Agency for the specified route.
    /// </summary>
    /// <remarks>
    ///   This is the <see cref="Agency" /> whose ID is specified by
    ///   <c>agency_id</c>.
    /// </remarks>
    public Agency Agency => Feed.Agencies[AgencyID];

    private Route(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "route_id")
    {
      if (!properties.ContainsKey("route_short_name") && !properties.ContainsKey("route_long_name"))
        throw new InvalidDataException("Routes must have either a long name or a short name.");
      if (!properties.IsInt("route_type")) throw new InvalidDataException("Routes must have a type.");
    }

    /// <summary>Creates a new <c>Route</c>.</summary>
    /// <param name="feed">The parent GTFS feed.</param>
    /// <param name="properties">The property collection.</param>
    public static Route Factory(GTFSFeed feed, IEnumerable<(string, string)> properties) => new Route(feed, new GTFSPropertyCollection(properties, feed.DefaultAgencyId));
  }

  namespace Extensions
  {
    public static class RouteAgencyExtensions
    {
      /// <summary>
      ///   Returns all the <c>Route</c>s under this <c>Agency</c>.
      /// </summary>
      public static IEnumerable<Route> Routes(this Agency agency) => agency.Feed.Routes.Where(x => x.AgencyID == agency.ID);
    }
  }
}