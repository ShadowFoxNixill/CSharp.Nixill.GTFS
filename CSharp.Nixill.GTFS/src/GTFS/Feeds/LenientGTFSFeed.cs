using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using Nixill.GTFS.Sources;
using System.Collections.Generic;
using NodaTime;

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
    public GTFSGenericCollection<FareRule> FareRules { get; }
    public GTFSOrderedEntityCollection<ShapePoint> ShapePoints { get; }
    public TwoKeyEntityCollection<Frequency, string, Duration> Frequencies { get; }
    public TwoKeyEntityCollection<Transfer, string, string> Transfers { get; }
    public IDEntityCollection<Pathway> Pathways { get; }
    public IDEntityCollection<Level> Levels { get; }

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
      ShapePoints = new GTFSOrderedEntityCollection<ShapePoint>(DataSource, "shapes", ShapePointFactory);
      Frequencies = new TwoKeyEntityCollection<Frequency, string, Duration>(DataSource, "frequencies", FrequencyFactory);
      Transfers = new TwoKeyEntityCollection<Transfer, string, string>(DataSource, "transfers", TransferFactory);
      Pathways = new IDEntityCollection<Pathway>(DataSource, "pathways", PathwayFactory);
      Levels = new IDEntityCollection<Level>(DataSource, "levels", LevelFactory);
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
      => new FareAttribute(new GTFSPropertyCollection(properties));

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
  }
}