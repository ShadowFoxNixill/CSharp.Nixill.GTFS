using System;
using System.Collections.Generic;
using System.IO;
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
      if (!properties.ContainsKey("agency_name")) throw new InvalidDataException("Agency name cannot be blank.");
      if (!properties.ContainsKey("agency_lang")) throw new InvalidDataException("Agency language cannot be blank.");
      if (!properties.ContainsKey("agency_timezone")) throw new InvalidDataException("Agency timezone cannot be blank.");
    }

    public static Agency Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new Agency(feed, new GTFSPropertyCollection(properties, ""));
    }
  }
}