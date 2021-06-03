using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A key location in a GTFS feed, such as a transit stop, station,
  ///   or station entrance/exit.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     The ID for a <c>Stop</c> is its <c>stop_id</c> property.
  ///   </para>
  ///   <para>
  ///     The term "station entrance" refers to both station entrances and
  ///     station exits. Stops, stations or station entrances are
  ///     collectively referred to as locations. Multiple routes may use
  ///     the same stop.
  ///   </para>
  /// </remarks>
  public class Stop : GTFSIdentifiedEntity
  {
    /// <summary>
    ///   Short text or a number that identifies the location for riders.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_code</c> property of the 
    ///   entity. These codes are often used in phone-based transit
    ///   information systems or printed on signage to make it easier for
    ///   riders to get information for a particular location. The
    ///   <c>StopCode</c> can be the same as <c>ID</c> if it is public
    ///   facing. This field should be left empty for locations without a
    ///   code presented to riders.
    /// </remarks>
    public string StopCode => Properties["stop_code"];

    /// <summary>Name of the location.</summary>
    /// <remarks>
    ///   <para>
    ///     This is the value of the <c>stop_name</c> property of the entity.
    ///   </para>
    ///   <para>
    ///     It should be a name that people will understand in the local
    ///     and tourist vernacular.
    ///   </para>
    ///   <para>
    ///     When the location is a boarding area
    ///     (<see cref="LocationType" /> ==
    ///     <see cref="StopLocationType.BoardingArea" />), the <c>Name</c>
    ///     should contains the name of the boarding area as displayed by
    ///     the agency. It could be just one letter (like on some European
    ///     intercity railway stations), or text like “Wheelchair boarding
    ///     area” (NYC’s Subway) or “Head of short trains” (Paris’ RER).
    ///   </para>
    /// </remarks>
    public string Name => Properties["stop_name"];

    /// <summary>
    ///   Description of the location that provides useful, quality
    ///   information.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_desc</c> property of the
    ///   entity. It should not simply duplicate the name of the location.
    /// </remarks>
    public string Description => Properties["stop_desc"];

    /// <summary>
    ///   The latitude of the location.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_lat</c> property of the entity.
    ///   It is not required on
    ///   <see cref="StopLocationType.GenericNode" />s or
    ///   <see cref="StopLocationType.BoardingArea" />s.
    /// </remarks>
    public double? Latitude => Properties.GetNullableDouble("stop_lat");

    /// <summary>
    ///   The longitude of the location.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_lon</c> property of the entity.
    ///   It is not required on
    ///   <see cref="StopLocationType.GenericNode" />s or
    ///   <see cref="StopLocationType.BoardingArea" />s.
    /// </remarks>
    public double? Longitude => Properties.GetNullableDouble("stop_lon");

    /// <summary>
    ///   Identifies the fare zone for a stop.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>zone_id</c> property of the entity.
    ///   This field is required if providing fare information using
    ///   <c>FareRules</c>, otherwise it is optional. If this record
    ///   represents a station or station entrance, the <c>ZoneId</c>
    ///   is ignored.
    /// </remarks>
    public string ZoneId => Properties["zone_id"];

    /// <summary>
    ///   URL of a web page about the location.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_url</c> property of the entity.
    ///   It should be different from the 
    ///   <see cref="Agency.Url" /> and the <see cref="Route.Url" /> field
    ///   values.
    /// </remarks>
    public string StopUrl => Properties["stop_url"];

    /// <summary>Type of the location.</summary>
    /// <remarks>
    ///   This is the value of the <c>location_type</c> property of the
    ///   entity and defaults to <see cref="StopLocationType.StopPlatform" />.
    /// </remarks>
    public StopLocationType LocationType => (StopLocationType)Properties.GetInt("location_type", 0);

    /// <summary>
    ///   Defines hierarchy between the different locations.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>parent_station</c> property of the
    ///   entity. It contains the ID of the parent location, as follows:
    ///   <list type="bullet">
    ///     <item>
    ///       For <see cref="StopLocationType.StopPlatform" />s, it may
    ///       contain a <see cref="StopLocationType.Station" /> or be empty.
    ///     </item>
    ///     <item>
    ///       For <see cref="StopLocationType.Station" />s, it must be empty.
    ///     </item>
    ///     <item>
    ///       For <see cref="StopLocationType.EntranceExit" />s or
    ///       <see cref="StopLocationType.GenericNode" />s, it must
    ///       contain a <see cref="StopLocationType.Station" />.
    ///     </item>
    ///     <item>
    ///       For <see cref="StopLocationType.BoardingArea" />s, it must
    ///       contain a <see cref="StopLocationType.StopPlatform" />.
    ///     </item>
    ///   </list>
    /// </remarks>
    public string ParentStationId => Properties["parent_station"];

    /// <summary>Timezone of the location.</summary>
    /// <remarks>
    ///   <para>
    ///     This is the value of the <c>stop_timezone</c> property of the
    ///     entity. If the location has a <see cref="ParentStation" />, it
    ///     inherits the parent station's <c>TimeZone</c> instead of
    ///     applying its own. Stations and parentless stops with empty
    ///     <c>TimeZone</c>s inherit the timezone specified by
    ///     <see cref="Agency.TimeZone" />.
    ///   </para>
    ///   <para>
    ///     Regardless of the value of <c>TimeZone</c>, <c>StopTimes</c>
    ///     should have times entered relative to the
    ///     <c>Agency.TimeZone</c>, ensuring that time values always
    ///     increase during a trip regardless of which timezones the trip
    ///     crosses.
    ///   </para>
    /// </remarks>
    public DateTimeZone TimeZone => Properties.GetTimeZone("stop_timezone");

    /// <summary>
    ///   Indicates whether wheelchair boardings are possible from this
    ///   location.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>wheelchair_boarding</c> property of
    ///   the entity, and defaults to <see cref="Tristate.Unknown" />.
    /// </remarks>
    public Tristate WheelchairBoarding => (Tristate)Properties.GetInt("wheelchair_boarding", 0);

    /// <summary>Level of the location.</summary>
    /// <remarks>
    ///   This is the value of the <c>level_id</c> property of the entity.
    ///   The same level can be used by multiple unlinked stations.
    /// </remarks>
    public string LevelId => Properties["level_id"];

    /// <summary>
    ///   Platform identifier for a platform stop (a stop belonging to a
    ///   station).
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>platform_code</c> property of the
    ///   entity. This should be just the platform identifier (eg. G or
    ///   3). Words like "platform" or "track" (or the feed’s
    ///   language-specific equivalent) should not be included. This
    ///   allows feed consumers to more easily internationalize and
    ///   localize the platform identifier into other languages.
    /// </remarks>
    public string PlatformCode => Properties["platform_code"];

    private Stop(GTFSPropertyCollection properties) : base(properties, "stop_id")
    {
    }

    /// <summary>Creates a new <c>Stop</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static Stop Factory(IEnumerable<(string, string)> properties)
    {
      return new Stop(new GTFSPropertyCollection(properties));
    }
  }
}