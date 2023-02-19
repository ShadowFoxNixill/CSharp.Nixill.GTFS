using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using NodaTime;

namespace Nixill.GTFS.Feeds
{
  public interface IStandardGTFSFeed : IGTFSFeed
  {
    public IDEntityCollection<FareAttribute> FareAttributes { get; }
    public GTFSGenericCollection<FareRule> FareRules { get; }
    public IDEntityCollection<FareProduct> FareProducts { get; }
    public GTFSGenericCollection<FareLegRule> FareLegRules { get; }
    public GTFSGenericCollection<FareTransferRule> FareTransferRules { get; }
    public IDEntityCollection<Area> Areas { get; }
    public TwoKeyEntityCollection<StopArea, string, string> StopAreas { get; }
    public GTFSOrderedEntityCollection<ShapePoint> ShapePoints { get; }
    public TwoKeyEntityCollection<Frequency, string, Duration> Frequencies { get; }
    public GTFSGenericCollection<Transfer> Transfers { get; }
    public IDEntityCollection<Pathway> Pathways { get; }
    public IDEntityCollection<Level> Levels { get; }
    public GTFSGenericCollection<Translation> Translations { get; }
    public FeedInfo FeedInfo { get; }
    public GTFSGenericCollection<Attribution> Attributions { get; }
  }
}