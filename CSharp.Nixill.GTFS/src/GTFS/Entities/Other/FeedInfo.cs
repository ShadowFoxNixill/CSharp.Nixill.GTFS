using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class FeedInfo : GTFSEntity
  {
    public FeedInfo(GTFSPropertyCollection properties) : base(properties) { }

    public string FeedPublisherName => Properties["feed_publisher_name"];
    public string FeedPublisherUrl => Properties["feed_publisher_url"];
    public string FeedLang => Properties["feed_lang"];
    public string DefaultLang => Properties["default_lang"];
    public LocalDate? FeedStartDate => Properties.GetNullableDate("feed_start_date");
    public LocalDate? FeedEndDate => Properties.GetNullableDate("feed_end_date");
    public string FeedVersion => Properties["feed_version"];
    public string FeedContactEmail => Properties["feed_contact_email"];
    public string FeedContactUrl => Properties["feed_contact_url"];
  }
}