using System;

namespace Nixill.GTFS.Enumerations
{
  [Flags]
  public enum CalendarDaysOfWeek
  {
    None = 0,

    Mondays = 1,
    Tuesdays = 2,
    Wednesdays = 4,
    Thursdays = 8,
    Fridays = 16,
    Saturdays = 32,
    Sundays = 64,

    Weekdays = 31,
    Weekends = 96,
    Daily = 127
  }
}