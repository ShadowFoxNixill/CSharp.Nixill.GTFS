using System;
using System.IO.Compression;
using Nixill.GTFS;
using Nixill.GTFS.Entities;
using Nixill.GTFS.Entities.Extensions;
using NodaTime.Text;
using System.Linq;

namespace Nixill.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      GTFSFeed feed = new GTFSFeed(ZipFile.OpenRead("gtfs/ddot_gtfs.zip"));
      StopTest(feed);
    }

    private static void StopTest(GTFSFeed feed)
    {
      var stops = feed.Stops.GroupBy(x => x.LocationType);

      foreach (var x in stops)
      {
        Console.WriteLine($"Location type {x.Key}: {x.Count()}");
      }
    }

    static void CalendarTest(GTFSFeed feed)
    {
      LocalDatePattern ptn = LocalDatePattern.CreateWithInvariantCulture("ddd uuuu-MM-dd");

      foreach (Calendar cal in feed.Calendars)
      {
        Console.WriteLine($"Calendar: {cal.ID}");
        Console.WriteLine($"Provides service on: {DayMasks.Get(cal.Mask)}");
        Console.WriteLine($"Active {ptn.Format(cal.StartDate)} through {ptn.Format(cal.EndDate)}");
        Console.WriteLine("Exceptions:");
        foreach (CalendarDate date in cal.Exceptions())
        {
          Console.WriteLine($"  {ptn.Format(date.Date)}: {date.ExceptionType}");
        }
        Console.WriteLine();
      }
    }
  }
}
