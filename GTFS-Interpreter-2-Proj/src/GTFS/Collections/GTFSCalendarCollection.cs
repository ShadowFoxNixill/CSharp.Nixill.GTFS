using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Collections
{
  public class GTFSCalendarCollection : IReadOnlyCollection<Calendar>
  {
    public readonly IDEntityCollection<Calendar> Calendars;
    public readonly TwoKeyEntityCollection<string, LocalDate, CalendarDate> CalendarDates;

    private List<GTFSUnparsedEntity> UnparsedCalendars = new List<GTFSUnparsedEntity>();

    public readonly GTFSFeed Feed;

    public int Count => Calendars.Count;

    public GTFSCalendarCollection(GTFSFeed feed, ZipArchiveEntry calendars, ZipArchiveEntry calendarDates)
    {
      CalendarDates = new TwoKeyEntityCollection<string, LocalDate, CalendarDate>(feed, calendarDates, CalendarDate.Factory);

      string today = GTFSObjectParser.DatePattern.Format(SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZone.Utc).LocalDateTime.Date);

      GTFSPropertyCollection defaultCalendar = new GTFSPropertyCollection(new Dictionary<string, string>{
        {"start_date", today},
        {"end_date", today},
        {"sunday", "0"},
        {"monday", "0"},
        {"tuesday", "0"},
        {"wednesday", "0"},
        {"thursday", "0"},
        {"friday", "0"},
        {"saturday", "0"}
      });

      List<string> serviceIds = CalendarDates.FirstKeys.ToList();

      List<Calendar> cals = new List<Calendar>();
      foreach (Calendar cal in GTFSFileEnumerator.Enumerate(feed, calendars, Calendar.Factory, UnparsedCalendars))
      {
        serviceIds.Remove(cal.ID);
        cals.Add(cal);
      }

      foreach (string id in serviceIds)
      {
        Calendar cal = Calendar.Factory(feed, defaultCalendar.Select(x => (x.Key, x.Value)));
      }

      Calendars = new IDEntityCollection<Calendar>(feed, cals);
    }

    public IEnumerator<Calendar> GetEnumerator() => Calendars.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Calendars.GetEnumerator();

    public IReadOnlyCollection<GTFSUnparsedEntity> GetUnparsedCalendars() =>
      UnparsedCalendars.AsReadOnly();
  }
}