using System;
using System.Collections.Generic;
using System.Drawing;
using Nixill.Utils;
using NodaTime;

namespace Nixill.GTFS.Parsing
{
  public static class GTFSObjectParser
  {
    private static IDateTimeZoneProvider TimezoneProvider = DateTimeZoneProviders.Tzdb;

    internal static LocalDate GetDate(string v)
    {
      throw new NotImplementedException();
    }

    public static string DefaultProperty(this Dictionary<string, string> dict, string property, string def)
    {
      if (dict.ContainsKey(property)) return dict[property];
      else return def;
    }

    public static string AgencyId(this Dictionary<string, string> dict, GTFSFeed feed)
    {
      if (dict.ContainsKey("agency_id")) return dict["agency_id"];
      else return feed.DefaultAgencyId;
    }

    public static string GetOrNull(this Dictionary<string, string> dict, string property)
    {
      if (dict.ContainsKey(property)) return dict[property];
      else return null;
    }

    public static DateTimeZone GetTimeZone(string input) => TimezoneProvider.GetZoneOrNull(input);

    public static Color? GetColorQM(string input)
    {
      if (input == null || input == "") return null;
      if (input.Length != 6) return null;
      int col = NumberUtils.StringToInt(input, 16);
      return Color.FromArgb(255, Color.FromArgb(col));
    }

    public static int? GetNullableInt(string input)
    {
      if (input == null || input == "") return null;
      return int.Parse(input);
    }
  }
}