using System.Collections.Generic;
using System.Linq;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   Defines a single trip of a transit route within a GTFS feed.
  /// </summary>
  /// <remarks>
  ///   The ID for this class is <c>trip_id</c>.
  /// </remarks>
  public class Trip : GTFSIdentifiedEntity
  {
    /// <summary>
    ///   Identifies the route to which this trip belongs.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>route_id</c> property of the entity.
    /// </remarks>
    public string RouteId => Properties["route_id"];

    /// <summary>
    ///   Identifies the set of days on which this trip runs.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>service_id</c> property of the entity.
    /// </remarks>
    public string ServiceId => Properties["service_id"];

    /// <summary>
    ///   Text that appears on signage identifying the trip's destination
    ///   to riders.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>trip_headsign</c> property of the
    ///   entity. Use this field to distinguish between different patterns
    ///   of service on the same route. If the headsign changes during a
    ///   trip, <c>Headsign</c> can be overridden by specifying values for
    ///   <c>StopTimes.Headsign</c>.
    /// </remarks>
    public string Headsign => Properties["trip_headsign"];

    /// <summary>
    ///   Public facing text used to identify the trip to riders, for
    ///   instance, to identify train numbers for commuter rail trips.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>trip_short_name</c> property of the
    ///   entity. If riders do not commonly rely on trip names, this field
    ///   should be empty. A <c>ShortName</c> value, if provided, should
    ///   uniquely identify a trip within a service day; it should not be
    ///   used for destination names or limited/express designations.
    /// </remarks>
    public string ShortName => Properties["trip_short_name"];

    /// <summary>
    ///   Indicates the direction of travel for a trip.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>direction_id</c> property of the
    ///   entity. This field is not used in routing; it provides a way to
    ///   separate trips by direction when publishing time tables.
    /// </remarks>
    /// <example>
    ///   Example: The trip_headsign and direction_id fields could be used
    ///   together to assign a name to travel in each direction for a set
    ///   of trips. A trips.txt file could contain these records for use
    ///   in time tables:
    ///   <code>
    ///     trip_id,...,trip_headsign,direction_id
    ///     1234,...,Airport,0
    ///     1505,...,Downtown,1
    ///   </code>
    /// </example>
    public DirectionId? DirectionId => (DirectionId?)Properties.GetInt("direction_id");

    /// <summary>
    ///   Identifies the block to which the trip belongs.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>block_id</c> property of the entity.
    ///   A block consists of a single trip or many sequential trips made
    ///   using the same vehicle, defined by shared service days and
    ///   <c>BlockId</c>. A <c>BlockId</c> can have trips with different
    ///   service days, making distinct blocks.
    /// </remarks>
    public string BlockId => Properties["block_id"];

    /// <summary>
    ///   Identifies a geospatial shape that describes the vehicle travel
    ///   path for a trip.
    /// </summary>
    /// <remarks>
    ///   This field is required if the trip has continuous behavior
    ///   defined, either at the route level or at the stop time level.
    /// </remarks>
    public string ShapeId => Properties["shape_id"];

    /// <summary>
    ///   Indicates whether or not the trip is wheelchair accessible.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>wheelchair_accessible</c> property
    ///   of the entity, which defaults to <see cref="Tristate.Unknown" />.
    /// </remarks>
    public Tristate WheelchairAccessible => (Tristate)Properties.GetInt("wheelchair_accessible", 0);

    /// <summary>
    ///   Indicates whether or not bikes are allowed on the trip.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>bikes_allowed</c> property of the
    ///   entity, which defaults to <see cref="Tristate.Unknown" />.
    /// </remarks>
    public Tristate BikesAllowed => (Tristate)Properties.GetInt("bikes_allowed", 0);

    private Trip(GTFSPropertyCollection properties) : base(properties, "trip_id")
    {
    }

    /// <summary>Creates a new <c>Trip</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static Trip Factory(IEnumerable<(string, string)> properties)
    {
      return new Trip(new GTFSPropertyCollection(properties));
    }
  }
}