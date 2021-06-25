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
    public static readonly Regex DateRegex = new Regex(@"^\d{8}$", RegexOptions.Compiled);

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
    public static readonly Regex TimeRegex = new Regex(@"^\d+:\d\d:\d\d$", RegexOptions.Compiled);

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
    private static readonly IDateTimeZoneProvider TimezoneProvider = DateTimeZoneProviders.Tzdb;

    public static DateTimeZone GetTimeZone(string input) => TimezoneProvider.GetZoneOrNull(input);
    public static DateTimeZone GetTimeZone(this GTFSPropertyCollection properties, string key) => GetTimeZone(properties[key]);

    // Numeric parsers
    public static bool IsInt(string input) => int.TryParse(input, out int placeholder);
    public static bool IsInt(this GTFSPropertyCollection properties, string key) => IsInt(properties[key]);

    public static bool IsDecimal(string input) => decimal.TryParse(input, out decimal placeholder);
    public static bool IsDecimal(this GTFSPropertyCollection properties, string key) => IsDecimal(properties[key]);

    public static bool IsDouble(string input) => double.TryParse(input, out double placeholder);
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

    /// <summary>
    ///   Returns the input as an <see cref="int" />, if it's a valid
    ///   input. Otherwise, returns <c>null</c>.
    /// </summary>
    public static int? GetNullableInt(string input)
    {
      if (int.TryParse(input, out int result)) return result;
      else return null;
    }
    public static int? GetNullableInt(this GTFSPropertyCollection properties, string key) => GetNullableInt(properties[key]);

    /// <summary>
    ///   Returns the input as a <see cref="decimal" />, if it's a valid
    ///   input. Otherwise, returns <c>null</c>.
    /// </summary>
    public static decimal? GetNullableDecimal(string input)
    {
      if (decimal.TryParse(input, out decimal result)) return result;
      else return null;
    }
    public static decimal? GetNullableDecimal(this GTFSPropertyCollection properties, string key) => GetNullableDecimal(properties[key]);

    /// <summary>
    ///   Returns the input as a <see cref="double" />, if it's a valid
    ///   input. Otherwise, returns <c>null</c>.
    /// </summary>
    public static double? GetNullableDouble(string input)
    {
      if (double.TryParse(input, out double result)) return result;
      else return null;
    }
    public static double? GetNullableDouble(this GTFSPropertyCollection properties, string key) => GetNullableDouble(properties[key]);

    /// <summary>
    ///   Returns the input as an <see cref="int" />, if it's valid and
    ///   non-negative. Otherwise, returns <c>null</c>.
    /// </summary>
    public static int? GetNullableNonNegativeInt(string input)
    {
      if (int.TryParse(input, out int result) && result >= 0) return result;
      else return null;
    }
    public static int? GetNullableNonNegativeInt(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeInt(properties[key]);

    /// <summary>
    ///   Returns the input as a <see cref="decimal" />, if it's valid and
    ///   non-negative. Otherwise, returns <c>null</c>.
    /// </summary>
    public static decimal? GetNullableNonNegativeDecimal(string input)
    {
      if (decimal.TryParse(input, out decimal result) && result >= 0) return result;
      else return null;
    }
    public static decimal? GetNullableNonNegativeDecimal(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeDecimal(properties[key]);

    /// <summary>
    ///   Returns the input as a <see cref="double" />, if it's valid and
    ///   non-negative. Otherwise, returns <c>null</c>.
    /// </summary>
    public static double? GetNullableNonNegativeDouble(string input)
    {
      if (double.TryParse(input, out double result) && result >= 0) return result;
      else return null;
    }
    public static double? GetNullableNonNegativeDouble(this GTFSPropertyCollection properties, string key) => GetNullableNonNegativeDouble(properties[key]);

    /// <summary>
    ///   Returns whether or not the input is a valid, non-negative
    ///   <see cref="int" />.
    /// </summary>
    public static bool IsNonNegativeInt(string input) => int.TryParse(input, out int placeholder) && placeholder >= 0;
    public static bool IsNonNegativeInt(this GTFSPropertyCollection properties, string key) => IsNonNegativeInt(properties[key]);

    /// <summary>
    ///   Returns whether or not the input is a valid, non-negative
    ///   <see cref="decimal" />.
    /// </summary>
    public static bool IsNonNegativeDecimal(string input) => decimal.TryParse(input, out decimal placeholder) && placeholder >= 0;
    public static bool IsNonNegativeDecimal(this GTFSPropertyCollection properties, string key) => IsNonNegativeDecimal(properties[key]);

    /// <summary>
    ///   Returns whether or not the input is a valid, non-negative
    ///   <see cref="double" />.
    /// </summary>
    public static bool IsNonNegativeDouble(string input) => double.TryParse(input, out double placeholder) && placeholder >= 0;
    public static bool IsNonNegativeDouble(this GTFSPropertyCollection properties, string key) => IsNonNegativeDouble(properties[key]);

    // Misc
    /// <summary>
    ///   Returns whether or not the input is <c>1</c>.
    /// </summary>
    /// <remarks>
    ///   Returns <c>false</c> for any other input, even <c>null</c>.
    /// </remarks>
    public static bool GetBool(string input)
    {
      return input == "1";
    }
    public static bool GetBool(this GTFSPropertyCollection properties, string key) => GetBool(properties[key]);

    /// <summary>
    ///   Returns whether the input is <c>0</c> (<c>false</c>) or <c>1</c>
    ///   (<c>true</c>).
    /// </summary>
    /// <remarks>
    ///   Returns <c>null</c> for any other input.
    /// </remarks>
    public static bool? GetNullableBool(string input)
    {
      if (input == "1") return true;
      if (input == "0") return false;
      return null;
    }
    public static bool? GetNullableBool(this GTFSPropertyCollection properties, string key) => GetNullableBool(properties[key]);

    /// <summary>
    ///   Returns whether the input is <c>0</c> or <c>1</c> (<c>true</c>)
    ///   or anything else (<c>false</c>).
    /// </summary>
    public static bool IsBool(string input)
    {
      return (input == "0" || input == "1");
    }
    public static bool IsBool(this GTFSPropertyCollection properties, string key) => IsBool(properties[key]);
  }
}