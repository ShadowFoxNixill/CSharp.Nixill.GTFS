using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Collections
{
  public class GTFSCalendarCollection : IReadOnlyCollection<(Calendar, IEnumerable<CalendarDate>)>
  {
    public readonly IDEntityCollection<Calendar> Calendars;
    public readonly TwoKeyEntityCollection<CalendarDate, string, LocalDate> CalendarDates;
    public readonly IReadOnlyList<string> ServiceIds;
    public int Count => ServiceIds.Count;
    public (Calendar, IEnumerable<CalendarDate>) this[string id] => (Calendars[id], CalendarDates.WithFirstKey(id));

    public GTFSCalendarCollection(IGTFSDataSource source, string calendarTable = "calendar", string calendarDateTable = "calendar_dates")
    {
      Calendars = new IDEntityCollection<Calendar>(source, calendarTable, Calendar.Factory);
      CalendarDates = new TwoKeyEntityCollection<CalendarDate, string, LocalDate>(source, calendarDateTable, CalendarDate.Factory);

      ServiceIds = Calendars.Select(x => x.ID).Union(CalendarDates.FirstKeys).ToList().AsReadOnly();
    }

    public IEnumerator<(Calendar, IEnumerable<CalendarDate>)> GetEnumerator() => ServiceIds.Select(x => this[x]).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)(GetEnumerator());
  }
}