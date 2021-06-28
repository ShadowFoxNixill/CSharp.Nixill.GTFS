using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Sources;
using NodaTime;

namespace Nixill.GTFS.Collections
{
  public class GTFSCalendarCollection : IReadOnlyCollection<(Calendar, IEnumerable<CalendarDate>)>
  {
    public readonly IDEntityCollection<Calendar> Calendars;
    public readonly TwoKeyEntityCollection<CalendarDate, string, LocalDate> CalendarDates;
    public readonly IReadOnlyList<string> ServiceIDs;
    public int Count => ServiceIDs.Count;
    public (Calendar, IEnumerable<CalendarDate>) this[string id] => (Calendars[id], CalendarDates.WithFirstKey(id));

    public GTFSCalendarCollection(IGTFSDataSource source, IDEntityCollection<Calendar> calendars, TwoKeyEntityCollection<CalendarDate, string, LocalDate> calendarDates)
    {
      Calendars = calendars;
      CalendarDates = calendarDates;

      ServiceIDs = Calendars.Select(x => x.ID).Union(CalendarDates.FirstKeys).ToList().AsReadOnly();
    }

    public IEnumerator<(Calendar, IEnumerable<CalendarDate>)> GetEnumerator() => ServiceIDs.Select(x => this[x]).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)(GetEnumerator());
    public bool Contains(string serviceID) => ServiceIDs.Contains(serviceID);
  }
}