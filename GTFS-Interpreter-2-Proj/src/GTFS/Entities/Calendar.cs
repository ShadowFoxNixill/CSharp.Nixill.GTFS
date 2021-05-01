using System.Collections.Generic;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Calendar : GTFSIdentifiedEntity
  {
    public LocalDate StartDate => GTFSObjectParser.GetDate(Properties["start_date"]);

    private Calendar(GTFSFeed feed, Dictionary<string, string> properties) : base(feed, properties, "service_id")
    {
    }


  }
}