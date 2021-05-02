using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class Calendar : GTFSIdentifiedEntity
  {
    public LocalDate StartDate => Properties.GetDate("start_date");
    public LocalDate EndDate => Properties.GetDate("end_date");
    public bool Sunday => Properties["sunday"] == "1";
    public bool Monday => Properties["monday"] == "1";
    public bool Tuesday => Properties["tuesday"] == "1";
    public bool Wednesday => Properties["wednesday"] == "1";
    public bool Thursday => Properties["thursday"] == "1";
    public bool Friday => Properties["friday"] == "1";
    public bool Saturday => Properties["saturday"] == "1";

    public int Mask =>
      (Monday ? 1 : 0) +
      (Tuesday ? 2 : 0) +
      (Wednesday ? 4 : 0) +
      (Thursday ? 8 : 0) +
      (Friday ? 16 : 0) +
      (Saturday ? 32 : 0) +
      (Sunday ? 64 : 0);

    public bool ServiceOnDayOfWeek(IsoDayOfWeek day) => day switch
    {
      IsoDayOfWeek.Sunday => Sunday,
      IsoDayOfWeek.Monday => Monday,
      IsoDayOfWeek.Tuesday => Tuesday,
      IsoDayOfWeek.Wednesday => Wednesday,
      IsoDayOfWeek.Thursday => Thursday,
      IsoDayOfWeek.Friday => Friday,
      IsoDayOfWeek.Saturday => Saturday,
      _ => false
    };

    public bool DateInRange(LocalDate date) => date >= StartDate && date <= EndDate;

    public bool ServiceOn(LocalDate date) => DateInRange(date) && ServiceOnDayOfWeek(date.DayOfWeek);

    private Calendar(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "service_id")
    {
      if (!properties.IsDate("start_date") || !properties.IsDate("end_date")) throw new InvalidDataException("Calendars must have a date range.");
    }

    public static Calendar Factory(GTFSFeed feed, IEnumerable<(string, string)> properties) => new Calendar(feed, new GTFSPropertyCollection(properties));
  }
}