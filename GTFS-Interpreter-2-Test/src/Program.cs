using System;
using Nixill.GTFS;
using Nixill.GTFS.Entities;
using NodaTime.Text;
using System.Linq;
using Nixill.GTFS.Parsing;
using System.Collections.Generic;
using Nixill.GTFS.Collections;

namespace Nixill.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      var source = new ZipGTFSDataSource("smart_gtfs.zip");
      StopTimeTest(source);
    }

    private static void StopTimeTest(IGTFSDataSource source)
    {
      GTFSFeed feed = new GTFSFeed(source);
      var stopTimes = new TwoKeyEntityCollection<string, int, StopTime>(source, "stop_times", StopTime.Factory);

      
    }

    private static void StopTest(GTFSFeed feed)
    {
      var stops = feed.Stops.GroupBy(x => x.LocationType);

      foreach (var x in stops)
      {
        Console.WriteLine($"Location type {x.Key}: {x.Count()}");
      }
    }

    private static void CalendarTest(GTFSFeed feed)
    {
      LocalDatePattern ptn = LocalDatePattern.CreateWithInvariantCulture("ddd uuuu-MM-dd");

      foreach ((Calendar Cal, IEnumerable<CalendarDate> CalDates) cal in feed.Calendars)
      {
        Console.WriteLine($"Calendar: {cal.Cal.ID}");
        Console.WriteLine($"Provides service on: {DayMasks.Get(cal.Cal.Mask)}");
        Console.WriteLine($"Active {ptn.Format(cal.Cal.StartDate)} through {ptn.Format(cal.Cal.EndDate)}");
        Console.WriteLine("Exceptions:");
        foreach (CalendarDate date in cal.CalDates)
        {
          Console.WriteLine($"  {ptn.Format(date.Date)}: {date.ExceptionType}");
        }
        Console.WriteLine();
      }
    }
  }
}
