using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class CalendarDate : GTFSTwoPartEntity<string, LocalDate>
  {
    public string ServiceId => Properties["service_id"];
    public LocalDate Date => Properties.GetDate("date");
    public ExceptionType ExceptionType => (ExceptionType)Properties.GetInt("exception_type");

    public bool IsAdded => ExceptionType == ExceptionType.Added;
    public bool IsRemoved => ExceptionType == ExceptionType.Removed;

    private CalendarDate(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, properties["service_id"], properties.GetDate("date"))
    {
      if (!properties.IsInt("exception_type")) throw new InvalidDataException("Calendar dates must have exception types.");
    }

    public static CalendarDate Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new CalendarDate(feed, new GTFSPropertyCollection(properties));
    }
  }

  namespace Extensions
  {
    public static class CalendarDateExtensions
    {
      public static IEnumerable<CalendarDate> Exceptions(this Calendar cal) =>
        cal.Feed.Calendars.CalendarDates.WithFirstKey(cal.ID);

      public static bool TotalServiceOn(this Calendar cal, LocalDate date)
      {
        TwoKeyEntityCollection<string, LocalDate, CalendarDate> calendarDates = cal.Feed.Calendars.CalendarDates;
        if (calendarDates.Contains((cal.ID, date))) return calendarDates[cal.ID, date].IsAdded;
        return cal.ServiceOn(date);
      }
    }
  }
}