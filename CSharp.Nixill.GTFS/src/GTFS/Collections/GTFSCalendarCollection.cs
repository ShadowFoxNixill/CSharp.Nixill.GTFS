using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Collections
{
  /// <summary>
  ///   A collection of <see cref="Calendar" />s and
  ///   <see cref="CalendarDate" />s.
  /// </summary>
  public class GTFSCalendarCollection : IReadOnlyCollection<(Calendar, IEnumerable<CalendarDate>)>
  {
    /// <summary>
    ///   The collection of <see cref="Calendar">s within in the feed data.
    /// </summary>
    /// <remarks>
    ///   This collection doesn't include virtual calendars that are only
    ///   present within <c>calendar_dates</c>.
    /// </remarks>
    public readonly IDEntityCollection<Calendar> Calendars;

    /// <summary>
    ///   The collection of <see cref="CalendarDate">s within the feed data.
    /// </summary>
    public readonly TwoKeyEntityCollection<string, LocalDate, CalendarDate> CalendarDates;

    /// <summary>
    ///   The collection of <c>service_id</c>s across both the
    ///   <c>calendar</c> and <c>calendar_dates</c> tables.
    /// </summary>
    public readonly IReadOnlyList<string> ServiceIds;

    /// <summary>
    ///   Returns the count of <see cref="ServiceIds" />.
    /// </summary>
    public int Count => ServiceIds.Count;

    /// <summary>
    ///   Gets both a <see cref="Calendar" /> (which may be <c>null</c>)
    ///   and the list of <see cref="CalendarDate" />s corresponding to
    ///   the given <c>service_id</c>.
    /// </summary>
    public (Calendar, IEnumerable<CalendarDate>) this[string id] => (Calendars[id], CalendarDates.WithFirstKey(id));

    /// <summary>
    ///   Creates a <c>GTFSCalendarCollection</c> from a given
    ///   <see cref="GTFSFeed" />, using the default <c>calendar</c> and
    ///   <c>calendar_dates</c> tables.
    /// </summary>
    public GTFSCalendarCollection(IGTFSDataSource source) : this(source, "calendar", "calendar_dates") { }

    /// <summary>
    ///   Creates a <c>GTFSCalendarCollection</c> from a given
    ///   <see cref="GTFSFeed" />, using user-defined table names.
    /// </summary>
    public GTFSCalendarCollection(IGTFSDataSource source, string calendarTable, string calendarDateTable)
    {
      Calendars = new IDEntityCollection<Calendar>(source, calendarTable, Calendar.Factory);
      CalendarDates = new TwoKeyEntityCollection<string, LocalDate, CalendarDate>(source, calendarDateTable, CalendarDate.Factory);

      ServiceIds = Calendars.Select(x => x.ID).Union(CalendarDates.FirstKeys).ToList().AsReadOnly();
    }

    public IEnumerator<(Calendar, IEnumerable<CalendarDate>)> GetEnumerator() => ServiceIds.Select(x => this[x]).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)(GetEnumerator());
  }
}