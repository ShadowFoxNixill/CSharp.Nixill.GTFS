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
    public bool Monday => Properties["monday"] == "1";
    public bool Tuesday => Properties["tuesday"] == "1";
    public bool Wednesday => Properties["wednesday"] == "1";
    public bool Thursday => Properties["thursday"] == "1";
    public bool Friday => Properties["friday"] == "1";
    public bool Saturday => Properties["saturday"] == "1";
    public bool Sunday => Properties["sunday"] == "1";

    public byte Mask => (byte)(
      (Monday ? 1 : 0) +
      (Tuesday ? 2 : 0) +
      (Wednesday ? 4 : 0) +
      (Thursday ? 8 : 0) +
      (Friday ? 16 : 0) +
      (Saturday ? 32 : 0) +
      (Sunday ? 64 : 0)
    );

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

    public Calendar(GTFSPropertyCollection properties) : base(properties, "service_id") { }

    /// <summary>Creates a new <c>Calendar</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static Calendar Factory(IEnumerable<(string, string)> properties) => new Calendar(new GTFSPropertyCollection(properties));
  }
}