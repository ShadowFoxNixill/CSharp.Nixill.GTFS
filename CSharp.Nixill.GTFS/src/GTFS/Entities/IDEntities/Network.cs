using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;

public class Network : GTFSIdentifiedEntity
{
  public Network(GTFSPropertyCollection props) : base(props, "network_id") { }

  public string Name => Properties["network_name"];
}