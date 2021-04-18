using System;
using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Agency : GTFSIdentifiedEntity
  {
    public string Name => Properties["agency_name"];
    public string Url => Properties.GetOrNull("agency_url");
    public string Language => Properties["agency_lang"];
    public string PhoneNumber => Properties.GetOrNull("agency_phone");
    public string FareUrl => Properties.GetOrNull("agency_fare_url");
    public string Email => Properties.GetOrNull("agency_email");
    public DateTimeZone TimeZone => GTFSObjectParser.GetTimeZone(Properties["agency_timezone"]);

    private Agency(GTFSFeed feed, Dictionary<string, string> properties) : base(feed, properties, "agency_id")
    {
      if (!properties.ContainsKey("agency_name") || properties["agency_name"] == "") throw new InvalidDataException("Agency name cannot be blank.");
      if (!properties.ContainsKey("agency_lang") || properties["agency_lang"] == "") throw new InvalidDataException("Agency language cannot be blank.");
      if (!properties.ContainsKey("agency_timezone") || properties["agency_timezone"] == "") throw new InvalidDataException("Agency timezone cannot be blank.");
    }

    public static Agency Factory(GTFSFeed feed, Dictionary<string, string> properties)
    {
      if (!properties.ContainsKey("agency_id")) properties.Add("agency_id", "");
      return new Agency(feed, properties);
    }
  }
}