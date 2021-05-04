using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Agency : GTFSIdentifiedEntity
  {
    public string Name => Properties["agency_name"];
    public string Url => Properties["agency_url"];
    public string Language => Properties["agency_lang"];
    public string PhoneNumber => Properties["agency_phone"];
    public string FareUrl => Properties["agency_fare_url"];
    public string Email => Properties["agency_email"];
    public DateTimeZone TimeZone => Properties.GetTimeZone("agency_timezone");

    private Agency(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "agency_id")
    {
    }

    public static Agency Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new Agency(feed, new GTFSPropertyCollection(properties, ""));
    }
  }
}