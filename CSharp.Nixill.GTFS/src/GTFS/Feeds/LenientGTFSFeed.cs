using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using Nixill.GTFS.Sources;
using System.Collections.Generic;
using NodaTime;
using System.Numerics;

namespace Nixill.GTFS.Feeds
{
  public class LenientGTFSFeed : IStandardGTFSFeed
  {
    public IGTFSDataSource DataSource { get; }
    public string DefaultAgencyID { get; internal set; }

    public IDEntityCollection<Agency> Agencies { get; }
    public IDEntityCollection<Route> Routes { get; }
    public GTFSCalendarCollection Calendars { get; }
    public IDEntityCollection<Stop> Stops { get; }
    public IDEntityCollection<Trip> Trips { get; }
    public GTFSOrderedEntityCollection<StopTime> StopTimes { get; }
    public IDEntityCollection<FareAttribute> FareAttributes { get; }
    public GTFSGenericCollection<Timeframe> Timeframes { get; }
    public GTFSGenericCollection<FareRule> FareRules { get; }
    public IDEntityCollection<FareMedia> FareMedia { get; }
    public TwoKeyEntityCollection<FareProduct, string, string> FareProducts { get; }
    public GTFSGenericCollection<FareLegRule> FareLegRules { get; }
    public GTFSGenericCollection<FareTransferRule> FareTransferRules { get; }
    public IDEntityCollection<Area> Areas { get; }
    public TwoKeyEntityCollection<StopArea, string, string> StopAreas { get; }
    public IDEntityCollection<Network> Networks { get; }
    public TwoKeyEntityCollection<RouteNetwork, string, string> RouteNetworks { get; }
    public GTFSOrderedEntityCollection<ShapePoint> ShapePoints { get; }
    public TwoKeyEntityCollection<Frequency, string, Duration> Frequencies { get; }
    public GTFSGenericCollection<Transfer> Transfers { get; }
    public IDEntityCollection<Pathway> Pathways { get; }
    public IDEntityCollection<Level> Levels { get; }
    public GTFSGenericCollection<Translation> Translations { get; }
    public FeedInfo FeedInfo { get; }
    public GTFSGenericCollection<Attribution> Attributions { get; }

    public LenientGTFSFeed(IGTFSDataSource source)
    {
      DataSource = source;

      Agencies = new IDEntityCollection<Agency>(DataSource, "agency", AgencyFactory);

      DefaultAgencyID = Agencies.First().ID;

      Routes = new IDEntityCollection<Route>(DataSource, "routes", RouteFactory);
      Calendars = new GTFSCalendarCollection(DataSource,
        new IDEntityCollection<Calendar>(DataSource, "calendar", CalendarFactory),
        new TwoKeyEntityCollection<CalendarDate, string, NodaTime.LocalDate>(DataSource, "calendar_dates", CalendarDateFactory)
      );
      Stops = new IDEntityCollection<Stop>(DataSource, "stops", StopFactory);
      Trips = new IDEntityCollection<Trip>(DataSource, "trips", TripFactory);
      StopTimes = new GTFSOrderedEntityCollection<StopTime>(DataSource, "stop_times", StopTimeFactory);
      FareAttributes = new IDEntityCollection<FareAttribute>(DataSource, "fare_attributes", FareAttributeFactory);
      Timeframes = new GTFSGenericCollection<Timeframe>(DataSource, "timeframes", TimeframeFactory);
      FareRules = new GTFSGenericCollection<FareRule>(DataSource, "fare_rules", FareRuleFactory);
      FareMedia = new IDEntityCollection<FareMedia>(DataSource, "fare_media", FareMediaFactory);
      FareProducts = new TwoKeyEntityCollection<FareProduct, string, string>(DataSource, "fare_products", FareProductFactory);
      FareLegRules = new GTFSGenericCollection<FareLegRule>(DataSource, "fare_leg_rules", FareLegRuleFactory);
      FareTransferRules = new GTFSGenericCollection<FareTransferRule>(DataSource, "fare_transfer_rules", FareTransferRuleFactory);
      Areas = new IDEntityCollection<Area>(DataSource, "areas", AreaFactory);
      StopAreas = new TwoKeyEntityCollection<StopArea, string, string>(DataSource, "stop_areas", StopAreaFactory);
      Networks = new IDEntityCollection<Network>(DataSource, "networks", NetworkFactory);
      RouteNetworks = new TwoKeyEntityCollection<RouteNetwork, string, string>(DataSource, "route_networks", RouteNetworkFactory);
      ShapePoints = new GTFSOrderedEntityCollection<ShapePoint>(DataSource, "shapes", ShapePointFactory);
      Frequencies = new TwoKeyEntityCollection<Frequency, string, Duration>(DataSource, "frequencies", FrequencyFactory);
      Transfers = new GTFSGenericCollection<Transfer>(DataSource, "transfers", TransferFactory);
      Pathways = new IDEntityCollection<Pathway>(DataSource, "pathways", PathwayFactory);
      Levels = new IDEntityCollection<Level>(DataSource, "levels", LevelFactory);
      Translations = new GTFSGenericCollection<Translation>(DataSource, "translations", TranslationFactory);
      Attributions = new GTFSGenericCollection<Attribution>(DataSource, "attributions", AttributionFactory);
      FeedInfo = null;

      foreach (FeedInfo info in DataSource.GetObjects("feed_info", FeedInfoFactory, new List<GTFSUnparsedEntity>()))
      {
        FeedInfo = info;
        break;
      }
    }

    private Agency AgencyFactory(IEnumerable<(string, string)> properties)
      => new Agency(new GTFSPropertyCollection(properties, ""));

    private Route RouteFactory(IEnumerable<(string, string)> properties)
      => new Route(new GTFSPropertyCollection(properties, DefaultAgencyID));

    private Calendar CalendarFactory(IEnumerable<(string, string)> properties)
      => new Calendar(new GTFSPropertyCollection(properties));

    private CalendarDate CalendarDateFactory(IEnumerable<(string, string)> properties)
      => new CalendarDate(new GTFSPropertyCollection(properties));

    private Stop StopFactory(IEnumerable<(string, string)> properties)
      => new Stop(new GTFSPropertyCollection(properties));

    private Trip TripFactory(IEnumerable<(string, string)> properties)
      => new Trip(new GTFSPropertyCollection(properties));

    private StopTime StopTimeFactory(IEnumerable<(string, string)> properties)
      => new StopTime(new GTFSPropertyCollection(properties));

    private FareAttribute FareAttributeFactory(IEnumerable<(string, string)> properties)
      => new FareAttribute(new GTFSPropertyCollection(properties, DefaultAgencyID));

    private Timeframe TimeframeFactory(IEnumerable<(string, string)> properties)
      => new Timeframe(new GTFSPropertyCollection(properties));

    private FareRule FareRuleFactory(IEnumerable<(string, string)> properties)
      => new FareRule(new GTFSPropertyCollection(properties));

    private FareMedia FareMediaFactory(IEnumerable<(string, string)> properties)
      => new FareMedia(new GTFSPropertyCollection(properties));

    private FareProduct FareProductFactory(IEnumerable<(string, string)> properties)
      => new FareProduct(new GTFSPropertyCollection(properties));

    private FareLegRule FareLegRuleFactory(IEnumerable<(string, string)> properties)
      => new FareLegRule(new GTFSPropertyCollection(properties));

    private FareTransferRule FareTransferRuleFactory(IEnumerable<(string, string)> properties)
      => new FareTransferRule(new GTFSPropertyCollection(properties));

    private Area AreaFactory(IEnumerable<(string, string)> properties)
      => new Area(new GTFSPropertyCollection(properties));

    private StopArea StopAreaFactory(IEnumerable<(string, string)> properties)
      => new StopArea(new GTFSPropertyCollection(properties));

    private Network NetworkFactory(IEnumerable<(string, string)> properties)
      => new Network(new GTFSPropertyCollection(properties));

    private RouteNetwork RouteNetworkFactory(IEnumerable<(string, string)> properties)
      => new RouteNetwork(new GTFSPropertyCollection(properties));

    private ShapePoint ShapePointFactory(IEnumerable<(string, string)> properties)
      => new ShapePoint(new GTFSPropertyCollection(properties));

    private Frequency FrequencyFactory(IEnumerable<(string, string)> properties)
      => new Frequency(new GTFSPropertyCollection(properties));

    private Transfer TransferFactory(IEnumerable<(string, string)> properties)
      => new Transfer(new GTFSPropertyCollection(properties));

    private Pathway PathwayFactory(IEnumerable<(string, string)> properties)
      => new Pathway(new GTFSPropertyCollection(properties));

    private Level LevelFactory(IEnumerable<(string, string)> properties)
      => new Level(new GTFSPropertyCollection(properties));

    private Translation TranslationFactory(IEnumerable<(string, string)> properties)
      => new Translation(new GTFSPropertyCollection(properties));

    private Attribution AttributionFactory(IEnumerable<(string, string)> properties)
      => new Attribution(new GTFSPropertyCollection(properties));

    private FeedInfo FeedInfoFactory(IEnumerable<(string, string)> properties)
      => new FeedInfo(new GTFSPropertyCollection(properties));
  }
}