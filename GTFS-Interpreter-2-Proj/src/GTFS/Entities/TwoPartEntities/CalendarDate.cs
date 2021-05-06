using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   Represents a single exception to a <c>Calendar</c>'s defined
  ///   service days.
  /// </summary>
  public class CalendarDate : GTFSTwoPartEntity<string, LocalDate>
  {
    /// <summary>
    ///   Identifies the set of dates to which this record adds an exception.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>service_id</c> property of the
    ///   entity, and the <c>FirstKey</c> of the object.
    /// </remarks>
    public string ServiceId => Properties["service_id"];

    /// <summary>
    ///   Identifies the date on which this exception occurs.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>date</c> property of the entity, and
    ///   the <c>SecondKey</c> of the object.
    /// </remarks>
    public LocalDate Date => Properties.GetDate("date");

    /// <summary>
    ///   Identifies whether service is available on <c>Date</c>.
    /// </summary>
    /// <remarks>
    ///   This is the value of the <c>exception_type</c> property of the
    ///   entity.
    /// </remarks>
    public ExceptionType ExceptionType => (ExceptionType)Properties.GetInt("exception_type");

    /// <summary>
    ///   Whether or not this record adds service on <c>Date</c>.
    /// </summary>
    /// <remarks>
    ///   This is whether or not the <c>exception_type</c> property equals
    ///   <c>1</c>.
    /// </remarks>
    public bool IsAdded => ExceptionType == ExceptionType.Added;

    /// <summary>
    ///   Whether or not this record removes service on <c>Date</c>.
    /// </summary>
    /// <remarks>
    ///   This is whether or not the <c>exception_type</c> property equals
    ///   <c>2</c>.
    /// </remarks>
    public bool IsRemoved => ExceptionType == ExceptionType.Removed;

    private CalendarDate(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, properties["service_id"], properties.GetDate("date"))
    {
      if (!properties.IsInt("exception_type")) throw new InvalidDataException("Calendar dates must have exception types.");
    }

    /// <summary>Creates a new <c>CalendarDate</c>.</summary>
    /// <param name="feed">The parent GTFS feed.</param>
    /// <param name="properties">The property collection.</param>
    public static CalendarDate Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new CalendarDate(feed, new GTFSPropertyCollection(properties));
    }
  }

  namespace Extensions
  {
    public static class CalendarDateExtensions
    {
      /// <summary>
      ///   The list of exceptions to this <c>Calendar</c>.
      /// </summary>
      public static IEnumerable<CalendarDate> Exceptions(this Calendar cal) =>
        cal.Feed.Calendars.CalendarDates.WithFirstKey(cal.ID);

      /// <summary>
      ///   Whether or not service is available on this date.
      /// </summary>
      /// <remarks>
      ///   This method considers both this <c>Calendar</c> object as well
      ///   as the <c>CalendarDate</c> exceptions in the feed.
      /// </remarks>
      public static bool TotalServiceOn(this Calendar cal, LocalDate date)
      {
        CalendarDate dateException = cal.Feed.Calendars.CalendarDates[cal.ID, date];
        if (dateException != null) return dateException.IsAdded;
        return cal.ServiceOn(date);
      }
    }
  }
}