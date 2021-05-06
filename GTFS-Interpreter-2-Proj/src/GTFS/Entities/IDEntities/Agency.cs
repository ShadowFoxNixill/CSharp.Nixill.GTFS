using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  /// <summary>
  ///   Represents a transit agency from the feed.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     The ID of an <c>Agency</c> is its <c>agency_id</c>. As stated in the
  ///     <see href="https://developers.google.com/transit/gtfs/reference/#agencytxt">
  ///       GTFS documentation</see> (slightly modified for grammar):
  ///   </para>
  ///   <para>
  ///     Identifies a transit brand, which is often synonymous with a
  ///     transit agency. Note that in some cases, such as when a single
  ///     agency operates multiple separate services, agencies and brands
  ///     are distinct. This document uses the term "agency" in place of
  ///     "brand". A dataset may contain data from multiple agencies. This
  ///     field is required when the dataset contains data for multiple
  ///     transit agencies, otherwise it is optional.
  ///   </para>
  /// </remarks>
  public class Agency : GTFSIdentifiedEntity
  {
    /// <summary>The full name of the agency.</summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_name</c> property of the entity.
    /// </remarks>
    public string Name => Properties["agency_name"];

    /// <summary>The URL of the agency's website.</summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_url</c> property of the entity.
    /// </remarks>
    public string Url => Properties["agency_url"];

    /// <summary>Primary language used by this transit agency.</summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_lang</c> property of the
    ///   entity. This field helps GTFS consumers choose capitalization
    ///   rules and other language-specific settings for the dataset.
    /// </remarks>
    public string Language => Properties["agency_lang"];

    /// <summary>A voice telephone number for the specified agency.</summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_phone</c> property of the
    ///   entity. This field is a string value that presents the telephone
    ///   number as typical for the agency's service area. It can and
    ///   should contain punctuation marks to group the digits of the
    ///   number. Dialable text (for example, TriMet's
    ///   <c>503-238-RIDE</c>) is permitted, but the field must not
    ///   contain any other descriptive text.
    /// </remarks>
    public string PhoneNumber => Properties["agency_phone"];

    /// <summary>
    ///   URL of a web page that allows a rider to purchase tickets or
    ///   other fare instruments for that agency online.
    /// </summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_fare_url</c> property of the
    ///   entity.
    /// </remarks>
    public string FareUrl => Properties["agency_fare_url"];

    /// <summary>
    ///   Email address actively monitored by the agency’s customer
    ///   service department.
    /// </summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_email</c> property of the
    ///   entity. This email address should be a direct contact point
    ///   where transit riders can reach a customer service representative
    ///   at the agency.
    /// </remarks>
    public string Email => Properties["agency_email"];

    /// <summary>
    ///   Timezone where the transit agency is located.
    /// </summary>
    /// <remarks>
    ///   This corresponds to the <c>agency_timezone</c> property of the
    ///   entity. If multiple agencies are specified in the dataset, each
    ///   must have the same TimeZone.
    /// </remarks>
    public DateTimeZone TimeZone => Properties.GetTimeZone("agency_timezone");

    private Agency(GTFSFeed feed, GTFSPropertyCollection properties) : base(feed, properties, "agency_id")
    {
    }

    public static Agency Factory(GTFSFeed feed, IEnumerable<(string, string)> properties)
    {
      return new Agency(feed, new GTFSPropertyCollection(properties, ""));
    }
  }
}