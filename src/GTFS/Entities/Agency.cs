using System.Collections.Generic;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Agency : GTFSIdentifiedEntity
  {
    public readonly string Name;
    public readonly string Url;
    public readonly string Language;
    public readonly string PhoneNumber;
    public readonly string FareUrl;
    public readonly string Email;
    public readonly DateTimeZone TimeZone;

    public Agency(GTFSFeed feed, Dictionary<string, string> properties) : base(feed, properties, "agency_id")
    {
      Name = properties["agency_name"];
      properties.TryGetValue("agency_url", out Url);
      Language = properties["agency_lang"];
      TimeZone = GTFSObjectParser.GetTimeZone(properties["agency_timezone"]);
      properties.TryGetValue("agency_phone", out PhoneNumber);
      properties.TryGetValue("agency_fare_url", out FareUrl);
      properties.TryGetValue("agency_email", out Email);
    }

    public static Agency Factory(GTFSFeed feed, Dictionary<string, string> properties) => new Agency(feed, properties);
  }
}