using System;
using System.IO.Compression;
using Nixill.GTFS;
using Nixill.GTFS.Entities;

namespace Nixill.Testing
{
  class Program
  {
    static void Main(string[] args)
    {
      GTFSFeed feed = new GTFSFeed(ZipFile.OpenRead("gtfs/ddot_gtfs.zip"));

      foreach (Agency ag in feed.Agencies)
      {
        Console.WriteLine(ag.Name);
        foreach (Route rt in ag.Routes())
        {
          Console.WriteLine((rt.ShortName ?? "") + " " + (rt.LongName ?? ""));
        }
      }
    }
  }
}
