using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using Nixill.GTFS.Enumerations;
using NodaTime;
using System.Linq;
using System.Collections.Generic;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   Represents a single instance of a trip serving a stop.
  /// </summary>
  public class StopTime : GTFSTwoPartEntity<string, int>
  {
    /// <summary>
    ///   The ID of the trip of which this stop is a part.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>trip_id</c> property of the entity,
    ///   and is its <c>FirstKey</c>.
    public string TripID => Properties["trip_id"];

    /// <summary>
    ///   The order in which this stop is served along the trip.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>stop_sequence</c> property of the
    ///   entity, and is its <c>SecondKey</c>. The values must increase
    ///   along the trip but do not need to be consecutive.
    /// </remarks>
    /// <example>
    ///   The first location on the trip could have a <c>StopSequence</c>
    ///   of <c>1</c>, the second location <c>23</c>, the third location
    ///   <c>40</c>, and so on.
    /// </example>
    public int StopSequence => Properties.GetInt("stop_sequence");

    /// <summary>
    ///   Arrival time at this stop.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This is the value of the <c>arrival_time</c> property of the
    ///     entity.
    ///   </para>
    ///   <para>
    ///     For times occurring after midnight on the service day, the
    ///     time is a value greater than 24:00:00 in HH:MM:SS local time
    ///     for the day on which the trip schedule begins.
    ///   </para>
    ///   <para>
    ///     Scheduled stops where the vehicle strictly adheres to the
    ///     specified arrival and departure times are timepoints. If this
    ///     stop is not a timepoint, it is recommended for feed producers 
    ///     to provide an estimated or interpolated time. If this is not
    ///     available, <c>ArrivalTime</c> may be left empty.
    ///     Further, feed producers should indicate that interpolated
    ///     times are provided with <C>Timepoint == false</c>. If
    ///     interpolated times are indicated with
    ///     <c>Timepoint == false</c>, then time points must be indicated
    ///     with <c>Timepoint == true</c>. Feed producers should provide
    ///     arrival times for all stops that are time points. An arrival
    ///     time must be specified for the first and the last stop in a trip.
    ///   </para>
    /// </remarks>
    public Duration? ArrivalTime => Properties.GetTime("arrival_time");

    /// <summary>
    ///   Departure time from this stop.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>departure_time</c> property of the
    ///   entity. All other remarks applicable to
    ///   <see cref="ArrivalTime" /> also apply here.
    /// </remarks>
    public Duration? DepartureTime => Properties.GetTime("departure_time");

    /// <summary>
    ///   The stop that is served at this point along the route.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This is the value of the <c>stop_id</c> property of the
    ///     entity.
    ///   </para>
    ///   <para>
    ///     All stops serviced during a trip must have a <c>StopTime</c>
    ///     record. Referenced locations must be stops
    ///     (<see cref="Stop.LocationType" /> == <see cref="StopLocationType.StopPlatform" />),
    ///     not stations or station entrances. A stop may be serviced
    ///     multiple times in the same trip, and multiple trips and routes
    ///     may service the same stop.
    ///   </para>
    /// </remarks>
    public string StopID => Properties["stop_id"];

    /// <summary>
    ///   Text that appears on signage identifying the trip's destination
    ///   to riders. 
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This is the value of the <c>stop_headsign</c> property of the
    ///     entity.
    ///   </para>
    ///   <para>
    ///     This field overrides the default <see cref="Trip.Headsign" />
    ///     when the headsign changes between stops. If the headsign
    ///     is displayed for an entire trip, use <c>Trip.Headsign</c> instead.
    ///   </para>
    ///   <para>
    ///     A <c>Headsign</c> value specified for one <c>StopTime</c>
    ///     does not apply to subsequent <c>StopTime</c>s in the same
    ///     trip. If you want to override the <c>Trip.Headsign</c> for
    ///     multiple <c>StopTime</c>s in the same trip, the
    ///     <c>Headsign</c> value must be repeated in each <c>StopTime</c>.
    ///   </para>
    /// </remarks>
    public string Headsign => Properties["stop_headsign"];

    /// <summary>Indicates pickup method.</summary>
    /// <remarks>
    ///   This is the value of the <c>pickup_type</c> property of the
    ///   entity, and defaults to <see cref="PickupDropoffType.Available" />.
    /// </remarks>
    public PickupDropoffType PickupType => (PickupDropoffType)Properties.GetInt("pickup_type", 0);

    /// <summary>Indicates drop off method.</summary>
    /// <remarks>
    ///   This is the value of the <c>drop_off_type</c> property of the
    ///   entity, and defaults to <see cref="PickupDropoffType.Available" />.
    /// </remarks>
    public PickupDropoffType DropoffType => (PickupDropoffType)Properties.GetInt("drop_off_type", 0);

    /// <summary>
    ///   Indicates whether a rider can board the transit vehicle at any
    ///   point along the vehicle’s travel path.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>continuous_pickup</c> property of
    ///   the entity, and defaults to
    ///   <see cref="PickupDropoffType.Unavailable" />. The path is
    ///   described by <c>Shapes</c>, from this <c>StopTime</c> to the
    ///   next <c>StopTime</c> in the trip’s <c>StopSequence</c>. The
    ///   continuous pickup behavior indicated in <c>StopTime</c>s
    ///   overrides any behavior defined in <see cref="Route" />s.
    /// </remarks>
    public PickupDropoffType ContinuousPickup => (PickupDropoffType)Properties.GetInt("continuous_pickup", 0);

    /// <summary>
    ///   Indicates whether a rider can alight from the transit vehicle
    ///   at any point along the vehicle's travel path.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>continuous_drop_off</c> property of
    ///   the entity, and defaults to
    ///   <see cref="PickupDropoffType.Unavalable" />. The path is
    ///   described by <c>Shapes</c>, from this <c>StopTime</c> to the
    ///   next <c>StopTime</c> in the trip’s <c>StopSequence</c>. The
    ///   continuous pickup behavior indicated in <c>StopTime</c>s
    ///   overrides any behavior defined in <see cref="Route" />s.
    /// </remarks>
    public PickupDropoffType ContinuousDropoff => (PickupDropoffType)Properties.GetInt("continuous_drop_off", 0);

    /// <summary>
    ///   Actual distance traveled along the associated shape, from the
    ///   start of the shape to the stop specified in this record.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>shape_dist_traveled</c> property of
    ///   the entity. This field specifies how much of the shape to draw
    ///   between any two stops during a trip. Must be in the same units
    ///   used in <c>Shapes</c>. Values used for <c>ShapeDistTraveled</c>
    ///   must increase along with <c>StopSequence</c>; they cannot be
    ///   used to show reverse travel along a route.
    /// </remarks>
    /// <example>
    ///   If this <c>StopTime</c> is 5.25 km from the start of the shape,
    ///   and the shape's distances are in kilometers, this property would
    ///   have a value of <c>5.25</c>.
    /// </example>
    public decimal? ShapeDistTraveled => Properties.GetNullableNonNegativeDecimal("shape_dist_traveled");

    /// <summary>
    ///   Indicates if arrival and departure times for a stop are strictly
    ///   adhered to by the vehicle or if they are instead approximate
    ///   and/or interpolated times.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>timepoint</c> field of the entity.
    ///   This field allows a GTFS producer to provide interpolated
    ///   stop-times, while indicating that the times are approximate. 
    /// </remarks>
    public bool Timepoint => Properties.GetBool("timepoint");

    private StopTime(GTFSPropertyCollection properties) : base(properties, properties["trip_id"], properties.GetInt("stop_sequence")) { }

    /// <summary>Create a new <c>StopTime</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static StopTime Factory(IEnumerable<(string, string)> properties) => new StopTime(new GTFSPropertyCollection(properties));
  }
}