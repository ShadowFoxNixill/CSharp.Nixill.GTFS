using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Frequency : GTFSTwoPartEntity<string, Duration>
  {
    public string TripID => Properties["trip_id"];
    public Duration StartTime => Properties.GetTime("start_time");
    public Duration EndTime => Properties.GetTime("end_time");
    public Duration Headway => Properties.GetDuration("headway_secs");
    public bool ExactTimes => Properties.GetBool("exact_times");

    public Frequency(GTFSPropertyCollection properties) : base(properties, properties["trip_id"], properties.GetTime("start_time")) { }

    public static Frequency Factory(IEnumerable<(string, string)> properties)
      => new Frequency(new GTFSPropertyCollection(properties));
  }
}