using NodaTime;

namespace Nixill.GTFS.Parsing
{
  public static class GTFSObjectParser
  {
    private static IDateTimeZoneProvider TimezoneProvider = DateTimeZoneProviders.Tzdb;

    public static DateTimeZone GetTimeZone(string input) => TimezoneProvider.GetZoneOrNull(input);
  }
}