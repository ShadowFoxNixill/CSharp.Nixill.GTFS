using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Transfer : GTFSTwoPartEntity<string, string>
  {
    public string FromStopID => Properties["from_stop_id"];
    public string ToStopID => Properties["to_stop_id"];
    public TransferType TransferType => (TransferType)Properties.GetInt("transfer_type", 0);
    public Duration? MinTransferTime => Properties.GetNullableDuration("min_transfer_time");

    public Transfer(GTFSPropertyCollection properties) : base(properties, properties["from_stop_id"], properties["to_stop_id"]) { }

    public static Transfer Factory(IEnumerable<(string, string)> properties)
      => new Transfer(new GTFSPropertyCollection(properties));
  }
}