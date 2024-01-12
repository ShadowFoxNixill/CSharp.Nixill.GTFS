using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Timeframe : GTFSEntity
  {
    public Timeframe(GTFSPropertyCollection properties) : base(properties) { }

    public string TimeframeGroupID => Properties["timeframe_group_id"];
    public Duration StartTime => Properties.GetTime("start_time", Duration.Zero);
    public Duration EndTime => Properties.GetTime("end_time", Duration.FromHours(24));
    public string ServiceID => Properties["service_id"];
  }
}