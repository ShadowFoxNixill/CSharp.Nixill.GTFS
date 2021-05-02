using System;
using System.IO.Compression;
using Nixill.GTFS;
using Nixill.GTFS.Entities;
using NodaTime.Text;

namespace Nixill.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      GTFSFeed feed = new GTFSFeed(ZipFile.OpenRead("gtfs/smart_gtfs.zip"));

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
