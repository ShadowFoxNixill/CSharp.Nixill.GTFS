using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Nixill.GTFS.Collections;
using Nixill.Utils;
using NodaTime;
using NodaTime.Text;

namespace Nixill.GTFS.Parsing
{
  public static class GTFSObjectParser
  {
    // Color: A color encoded as a six-digit hexadecimal number. Refer to
    //   https://htmlcolorcodes.com to generate a valid value (the leading
    //   "#" is not included).
    // Example: `FFFFFF` for white, `000000` for black or `0039A6` for the
    //   A, C, E lines in NYMTA.
    public static readonly Regex ColorRegex = new Regex(@"^[0-9a-f]{6}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static Color? GetNullableColor(string input)
    {
      if (input == null) return null;
      if (!ColorRegex.TryMatch(input, out Match match)) return null;
      int col = NumberUtils.StringToInt(input, 16);
      return Color.FromArgb(255, Color.FromArgb(col));
    }
    public static Color? GetNullableColor(this GTFSPropertyCollection properties, string key) => GetNullableColor(properties[key]);

    public static Color GetColor(string input, Color? def = null)
    {
      Color? test = GetNullableColor(input);
      if (test.HasValue) return test.Value;
      if (def.HasValue) return def.Value;
      throw new ArgumentException($"{input ?? "(null)"} could not be parsed to a color.");
    }
    public static Color GetColor(this GTFSPropertyCollection properties, string key, Color? def = null) => GetColor(properties[key], def);

    public static bool IsColor(string input)
    {
      if (input == null) return false;
      return ColorRegex.IsMatch(input);
    }
    public static bool IsColor(this GTFSPropertyCollection properties, string key) => IsColor(properties[key]);

    // Date: Service day in the `YYYYMMDD` format. Since time within a
    //   service day can be above 24:00:00, a service day often contains
    //   information for the subsequent day(s).
    // Example: `20180913` for September 13th, 2018.
    public static readonly LocalDatePattern DatePattern = LocalDatePattern.CreateWithInvariantCulture("uuuuMMdd");
    public static readonly Regex DateRegex = new Regex(@"^\d{8}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static LocalDate? GetNullableDate(string input)
    {
      if (input == null) return null;
      ParseResult<LocalDate> res = DatePattern.Parse(input);
      if (res.Success) return res.Value;
      else return null;
    }
    public static LocalDate? GetNullableDate(this GTFSPropertyCollection properties, string key) => GetNullableDate(properties[key]);

    public static LocalDate GetDate(string input, LocalDate? def = null)
    {
      ParseResult<LocalDate> res = DatePattern.Parse(input);
      if (res.Success) return res.Value;
      if (def.HasValue) return def.Value;
      else throw res.Exception;
    }
    public static LocalDate GetDate(this GTFSPropertyCollection properties, string key, LocalDate? def = null) => GetDate(properties[key], def);

    public static bool IsDate(string input)
    {
      if (input == null) return false;
      return DateRegex.IsMatch(input);
    }
    public static bool IsDate(this GTFSPropertyCollection properties, string key) => IsDate(properties[key]);

    // Time in the HH:MM:SS format (H:MM:SS is also accepted). The time is
    //   measured from "noon minus 12h" of the service day (effectively
    //   midnight except for days on which daylight savings time changes
    //   occur. For more information, see the guidelines article). For
    //   times occurring after midnight, enter the time as a value greater
    //   than `24:00:00` in HH:MM:SS local time for the day on which the
    //   trip schedule begins.
    // Example: `14:30:00` for 2:30PM or `25:35:00` for 1:35AM on the next day.
    public static readonly DurationPattern TimePattern = DurationPattern.CreateWithInvariantCulture("H:mm:ss");
    public static readonly Regex TimeRegex = new Regex(@"\d+:\d\d:\d\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static Duration? GetNullableTime(string input)
    {
      if (input == null) return null;
      ParseResult<Duration> res = TimePattern.Parse(input);
      if (res.Success) return res.Value;
      else return null;
    }
    public static Duration? GetNullableTime(this GTFSPropertyCollection properties, string key) => GetNullableTime(properties[key]);

    public static Duration GetTime(string input, Duration? def = null)
    {
      ParseResult<Duration> res = TimePattern.Parse(input);
      if (res.Success) return res.Value;
      if (def.HasValue) return def.Value;
      else throw res.Exception;
    }
    public static Duration GetTime(this GTFSPropertyCollection properties, string key, Duration? def = null) => GetTime(properties[key], def);

    public static bool IsTime(string input)
    {
      if (input == null) return false;
      return TimeRegex.IsMatch(input);
    }
    public static bool IsTime(this GTFSPropertyCollection properties, string key) => IsTime(properties[key]);

    // Timezone - TZ timezone from the https://www.iana.org/time-zones.
    //   Timezone names never contain the space character but may contain
    //   an underscore. Refer to
    //   http://en.wikipedia.org/wiki/List_of_tz_zones for a list of valid
    //   values. 
    public static readonly IDateTimeZoneProvider TimezoneProvider = DateTimeZoneProviders.Tzdb;

    public static DateTimeZone GetTimeZone(string input) => TimezoneProvider.GetZoneOrNull(input);
    public static DateTimeZone GetTimeZone(this GTFSPropertyCollection properties, string key) => GetTimeZone(properties[key]);

    // Numeric parsers
    public static bool TryInt(string input, out int result)
    {
      result = 0;
      if (input == null) return false;
      return int.TryParse(input, out result);
    }
    public static bool TryInt(this GTFSPropertyCollection properties, string key, out int result) => TryInt(properties[key], out result);

    public static bool TryDecimal(string input, out decimal result)
    {
      result = 0;
      if (input == null) return false;
      return decimal.TryParse(input, out result);
    }
    public static bool TryDecimal(this GTFSPropertyCollection properties, string key, out decimal result) => TryDecimal(properties[key], out result);

    public static bool TryDouble(string input, out double result)
    {
      result = 0;
      if (input == null) return false;
      return double.TryParse(input, out result);
    }
    public static bool TryDouble(this GTFSPropertyCollection properties, string key, out double result) => TryDouble(properties[key], out result);

    public static bool IsInt(string input) => TryInt(input, out int placeholder);
    public static bool IsInt(this GTFSPropertyCollection properties, string key) => IsInt(properties[key]);

    public static bool IsDecimal(string input) => TryDecimal(input, out decimal placeholder);
    public static bool IsDecimal(this GTFSPropertyCollection properties, string key) => IsDecimal(properties[key]);

    public static bool IsDouble(string input) => TryDouble(input, out double placeholder);
    public static bool IsDouble(this GTFSPropertyCollection properties, string key) => IsDouble(properties[key]);

    public static int GetInt(this GTFSPropertyCollection properties, string key, int? def = null)
    {
      string input = properties[key];
      int? ret = GetNullableInt(input);
      if (ret.HasValue) return ret.Value;
      if (def.HasValue) return def.Value;
      throw new ArgumentException($"{input} could not be cast to a number.");
    }

    public static decimal GetDecimal(this GTFSPropertyCollection properties, string key, decimal? def = null)
    {
      string input = properties[key];
      decimal? ret = GetNullableDecimal(input);
      if (ret.HasValue) return ret.Value;
      if (def.HasValue) return def.Value;
      throw new ArgumentException($"{input} could not be cast to a number.");
    }

    public static double GetDouble(this GTFSPropertyCollection properties, string key, double? def = null)
    {
      string input = properties[key];
      double? ret = GetNullableDouble(input);
      if (ret.HasValue) return ret.Value;
      if (def.HasValue) return def.Value;
      throw new ArgumentException($"{input} could not be cast to a number.");
    }

    public static int? GetNullableInt(string input)
    {
      if (TryInt(input, out int result)) return result;
      else return null;
    }
    public static int? GetNullableInt(this GTFSPropertyCollection properties, string key) => GetNullableInt(properties[key]);

    public static decimal? GetNullableDecimal(string input)
    {
      if (TryDecimal(input, out decimal result)) return result;
      else return null;
    }
    public static decimal? GetNullableDecimal(this GTFSPropertyCollection properties, string key) => GetNullableDecimal(properties[key]);

    public static double? GetNullableDouble(string input)
    {
      if (TryDouble(input, out double result)) return result;
      else return null;
    }
    public static double? GetNullableDouble(this GTFSPropertyCollection properties, string key) => GetNullableDouble(properties[key]);

    public static bool TryNonNegativeInt(string input, out int result)
    {
      if (TryInt(input, out result) && result >= 0) return true;
      else return false;
    }
    public static bool TryNonNegativeInt(this GTFSPropertyCollection properties, string key, out int result) => TryNonNegativeInt(properties[key], out result);

    public static bool TryNonNegativeDecimal(string input, out decimal result)
    {
      if (TryDecimal(input, out result) && result >= 0) return true;
      else return false;
    }
    public static bool TryNonNegativeDecimal(this GTFSPropertyCollection properties, string key, out decimal result) => TryNonNegativeDecimal(properties[key], out result);

    public static bool TryNonNegativeDouble(string input, out double result)
    {
      if (TryDouble(input, out result) && result >= 0) return true;
      else return false;
    }
    public static bool TryNonNegativeDouble(this GTFSPropertyCollection properties, string key, out double result) => TryNonNegativeDouble(properties[key], out result);

    public static int? GetNullableNonNegativeInt(string input)
    {
      if (TryInt(input, out int result) && result >= 0) return result;
      else return null;
    }
    public static int? GetNullableNonNegativeInt(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeInt(properties[key]);

    public static decimal? GetNullableNonNegativeDecimal(string input)
    {
      if (TryDecimal(input, out decimal result) && result >= 0) return result;
      else return null;
    }
    public static decimal? GetNullableNonNegativeDecimal(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeDecimal(properties[key]);

    public static double? GetNullableNonNegativeDouble(string input)
    {
      if (TryDouble(input, out double result) && result >= 0) return result;
      else return null;
    }
    public static double? GetNullableNonNegativeDouble(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeDouble(properties[key]);

    public static bool IsNonNegativeInt(string input) => TryNonNegativeInt(input, out int placeholder);
    public static bool IsNonNegativeInt(this GTFSPropertyCollection properties, string key) => IsNonNegativeInt(properties[key]);

    public static bool IsNonNegativeDecimal(string input) => TryNonNegativeDecimal(input, out decimal placeholder);
    public static bool IsNonNegativeDecimal(this GTFSPropertyCollection properties, string key) => IsNonNegativeDecimal(properties[key]);

    public static bool IsNonNegativeDouble(string input) => TryNonNegativeDouble(input, out double placeholder);
    public static bool IsNonNegativeDouble(this GTFSPropertyCollection properties, string key) => IsNonNegativeDouble(properties[key]);

    // Misc
    public static bool GetBool(string input)
    {
      return input == "1";
    }
    public static bool GetBool(this GTFSPropertyCollection properties, string key) => GetBool(properties[key]);

    public static bool? GetNullableBool(string input)
    {
      if (input == "1") return true;
      if (input == "0") return false;
      return null;
    }
    public static bool? GetNullableBool(this GTFSPropertyCollection properties, string key) => GetNullableBool(properties[key]);

    public static bool IsBool(string input)
    {
      return (input == "0" || input == "1");
    }
    public static bool IsBool(this GTFSPropertyCollection properties, string key) => IsBool(properties[key]);
  }
}