using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   A <c>Calendar</c> is a set of dates when service is available for
  ///   one or more routes.
  /// </summary>
  /// <remarks>
  ///   The ID for a <c>Calendar</c> is its <c>service_id</c> property.
  /// </remarks>
  public class Calendar : GTFSIdentifiedEntity
  {
    /// <summary>Start service day for the service interval.</summary>
    /// <remarks>
    ///   This is the value of the <c>start_date</c> property of the entity.
    /// </remarks>
    public LocalDate StartDate => Properties.GetDate("start_date");

    /// <summary>End service day for the service interval.</summary>
    /// <remarks>
    ///   This is the value of the <c>end_date</c> property of the entity.
    ///   This service day is included in the interval.
    /// </remarks>
    public LocalDate EndDate => Properties.GetDate("end_date");

    /// <summary>
    ///   Indicates whether the service operates on all Mondays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>monday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Monday => Properties["monday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Tuesdays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>tuesday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Tuesday => Properties["tuesday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Wednesdays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>wednesday</c> property of the
    ///   entity, specifically returning whether or not that property has
    ///   a value of <c>1</c>. Exceptions on individual dates may be
    ///   listed in the <c>calendar_dates</c> table.
    /// </remarks>
    public bool Wednesday => Properties["wednesday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Thursdays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>thursday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Thursday => Properties["thursday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Fridays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>friday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Friday => Properties["friday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Saturdays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>saturday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Saturday => Properties["saturday"] == "1";

    /// <summary>
    ///   Indicates whether the service operates on all Sundays in the
    ///   date range specified by the <c>start_date</c> and
    ///   <c>end_date</c> fields.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>sunday</c> property of the entity,
    ///   specifically returning whether or not that property has a value
    ///   of <c>1</c>. Exceptions on individual dates may be listed in the
    ///   <c>calendar_dates</c> table.
    /// </remarks>
    public bool Sunday => Properties["sunday"] == "1";

    /// <summary>
    ///   Returns the days-of-week of service as a bitfield.
    /// </summary>
    /// <remarks>
    ///   0x1 is the value of Monday, 0x2 to Tuesday, etc. to 0x40 for Sunday.
    /// </remarks>
    public int Mask =>
      (Monday ? 1 : 0) +
      (Tuesday ? 2 : 0) +
      (Wednesday ? 4 : 0) +
      (Thursday ? 8 : 0) +
      (Friday ? 16 : 0) +
      (Saturday ? 32 : 0) +
      (Sunday ? 64 : 0);

    /// <summary>
    ///   Whether or not there exists service on a given day of the week.
    /// </summary>
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

    /// <summary>
    ///   Whether or not a given date is between the
    ///   <see cref="StartDate" /> and <see cref="EndDate" /> of the Calendar.
    /// </summary>
    public bool DateInRange(LocalDate date) => date >= StartDate && date <= EndDate;

    /// <summary>
    ///   Whether or not there is service on the given date
    ///   (<see cref="ServiceOnDayOfWeek" /> &amp;&amp;
    ///   <see cref="DateInRange" />).
    /// </summary>
    /// <remarks>
    ///   This method only considers this <c>Calendar</c> entity without
    ///   regard to any <c>CalendarDate</c>s that may change the result.
    ///   To consider the <c>CalendarDate</c>s as well, use the extension
    ///   method
    ///   <see cref="Extensions.CalendarDateExtensions.TotalServiceOn(Calendar, LocalDate)">TotalServiceOn()</see>.
    /// </remarks>
    public bool ServiceOn(LocalDate date) => DateInRange(date) && ServiceOnDayOfWeek(date.DayOfWeek);

    private Calendar(GTFSPropertyCollection properties) : base(properties, "service_id")
    {
      if (!properties.IsDate("start_date") || !properties.IsDate("end_date")) throw new InvalidDataException("Calendars must have a date range.");
    }

    /// <summary>Creates a new <c>Calendar</c>.</summary>
    /// <param name="properties">The property collection.</param>
    public static Calendar Factory(IEnumerable<(string, string)> properties) => new Calendar(new GTFSPropertyCollection(properties));
  }
}