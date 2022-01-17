using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using NodaTime;

namespace Nixill.GTFS.Feeds
{
  public interface IStandardGTFSFeed : IGTFSFeed
  {
    public IDEntityCollection<FareAttribute> FareAttributes { get; }
    public GTFSGenericCollection<FareRule> FareRules { get; }
    public GTFSOrderedEntityCollection<ShapePoint> ShapePoints { get; }
    public TwoKeyEntityCollection<Frequency, string, Duration> Frequencies { get; }
    public TwoKeyEntityCollection<Transfer, string, string> Transfers { get; }
    public IDEntityCollection<Pathway> Pathways { get; }
    public IDEntityCollection<Level> Levels { get; }
    public GTFSGenericCollection<Translation> Translations { get; }
    public FeedInfo FeedInfo { get; }
    public GTFSGenericCollection<Attribution> Attributions { get; }
  }
}