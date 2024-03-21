using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using Nixill.GTFS.Enumerations;
using NodaTime;
using System.Collections.Generic;

namespace Nixill.GTFS.Entities
{
  public class StopTime : GTFSOrderedEntity
  {
    public string TripID => Properties["trip_id"];
    public int StopSequence => Properties.GetInt("stop_sequence");
    public Duration? ArrivalTime => Properties.GetNullableTime("arrival_time");
    public Duration? DepartureTime => Properties.GetNullableTime("departure_time");
    public string StopID => Properties["stop_id"];
    public string Headsign => Properties["stop_headsign"];
    public PickupDropoffType PickupType => (PickupDropoffType)Properties.GetInt("pickup_type", 0);
    public PickupDropoffType DropoffType => (PickupDropoffType)Properties.GetInt("drop_off_type", 0);
    public PickupDropoffType ContinuousPickup => (PickupDropoffType)Properties.GetInt("continuous_pickup", 0);
    public PickupDropoffType ContinuousDropoff => (PickupDropoffType)Properties.GetInt("continuous_drop_off", 0);
    public decimal? ShapeDistTraveled => Properties.GetNullableNonNegativeDecimal("shape_dist_traveled");
    public bool Timepoint => Properties.GetBool("timepoint");

    public StopTime(GTFSPropertyCollection properties) : base(properties, "trip_id", "stop_sequence") { }
  }
}