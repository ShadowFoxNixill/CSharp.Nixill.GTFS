using System;
using Nixill.GTFS;
using Nixill.GTFS.Entities;
using NodaTime.Text;
using System.Linq;
using Nixill.GTFS.Parsing;
using System.Collections.Generic;
using Nixill.GTFS.Collections;
using NodaTime;

namespace Nixill.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      var source = new ZipGTFSDataSource("gtfs/ddot_gtfs.zip");
      StopTimeTest(source);
    }

    private static void StopTimeTest(IGTFSDataSource source)
    {
      GTFSFeed feed = new GTFSFeed(source);
      var stopTimes = feed.StopTimes;

      var tripsByRouteService = feed.Trips.GroupBy(tp => (tp.RouteId, tp.ServiceId));
      var timesByTrip = stopTimes.GroupBy(stm => stm.TripID).ToDictionary(stm => stm.Key, stm => new { start = stm.Min(x => x.DepartureTime), end = stm.Max(x => x.ArrivalTime) });

      DurationPattern ptn = DurationPattern.CreateWithInvariantCulture("H:mm:ss");

      foreach (var group in tripsByRouteService)
      {
        var rt = feed.Routes[group.Key.RouteId];
        var cal = feed.Calendars[group.Key.ServiceId].Item1;

        string header = $"{rt.Type} {rt.ShortName} {rt.LongName} - {DayMasks.Get(cal.Mask)}";

        Duration min = Duration.MaxValue;
        Duration max = Duration.MinValue;

        foreach (var trip in group)
        {
          var times = timesByTrip[trip.ID];
          min = Duration.Min(times.start.Value, min);
          max = Duration.Max(times.end.Value, max);
        }

        string timeStr = $"{ptn.Format(min)} - {ptn.Format(max)}";

        Console.WriteLine($"{header}: {timeStr}");
      }
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
