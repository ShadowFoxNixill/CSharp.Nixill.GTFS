using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using Nixill.GTFS.Sources;
using System.Collections.Generic;
using Nixill.GTFS.Parsing;
using Nixill.GTFS.Parsing.Exceptions;
using Nixill.GTFS.Enumerations;
using NodaTime;

namespace Nixill.GTFS.Feeds
{
  public class StrictGTFSFeed : IStandardGTFSFeed
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
    public GTFSGenericCollection<Translation> Translations { get; }
    public FeedInfo FeedInfo { get; }
    public GTFSGenericCollection<Attribution> Attributions { get; }

    public StrictGTFSFeed(IGTFSDataSource source)
    {
      DataSource = source;

      Agencies = new IDEntityCollection<Agency>(DataSource, "agency", AgencyFactory);

      DefaultAgencyID = Agencies.First().ID;

      Routes = new IDEntityCollection<Route>(DataSource, "routes", RouteFactory);
      Calendars = new GTFSCalendarCollection(DataSource,
        new IDEntityCollection<Calendar>(DataSource, "calendar", CalendarFactory),
        new TwoKeyEntityCollection<CalendarDate, string, NodaTime.LocalDate>(DataSource, "calendar_dates", CalendarDateFactory)
      );
      Levels = new IDEntityCollection<Level>(DataSource, "levels", LevelFactory);
      Stops = new IDEntityCollection<Stop>(DataSource, "stops", StopFactory);
      Trips = new IDEntityCollection<Trip>(DataSource, "trips", TripFactory);
      StopTimes = new GTFSOrderedEntityCollection<StopTime>(DataSource, "stop_times", StopTimeFactory);
      FareAttributes = new IDEntityCollection<FareAttribute>(DataSource, "fare_attributes", FareAttributeFactory);
      FareRules = new GTFSGenericCollection<FareRule>(DataSource, "fare_rules", FareRuleFactory);
      ShapePoints = new GTFSOrderedEntityCollection<ShapePoint>(DataSource, "shapes", ShapePointFactory);
      Frequencies = new TwoKeyEntityCollection<Frequency, string, Duration>(DataSource, "frequencies", FrequencyFactory);
      Transfers = new TwoKeyEntityCollection<Transfer, string, string>(DataSource, "transfers", TransferFactory);
      Pathways = new IDEntityCollection<Pathway>(DataSource, "pathways", PathwayFactory);
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
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties, "");
      props.AssertExists("agency_name");
      props.AssertExists("agency_url");
      props.AssertTimeZone("agency_timezone");
      return new Agency(props);
    }

    private Route RouteFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties, DefaultAgencyID);
      props.AssertExists("route_id");
      props.AssertForeignKeyExists("agency_id", Agencies, "agency");
      // route_short_name OR route_long_name
      if (!(props.ContainsKey("route_short_name") || props.ContainsKey("route_long_name"))) throw new PropertyNullException("route_short_name and route_long_name");
      props.AssertNonNegativeInt("route_type");
      return new Route(props);
    }

    private Calendar CalendarFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("service_id");
      props.AssertDate("start_date");
      props.AssertDate("end_date");
      props.AssertBool("monday");
      props.AssertBool("tuesday");
      props.AssertBool("wednesday");
      props.AssertBool("thursday");
      props.AssertBool("friday");
      props.AssertBool("saturday");
      props.AssertBool("sunday");
      return new Calendar(props);
    }

    private CalendarDate CalendarDateFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("service_id");
      props.AssertDate("date");
      props.AssertNonNegativeInt("exception_type");
      return new CalendarDate(props);
    }

    private Stop StopFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("stop_id");
      StopLocationType type = (StopLocationType)props.GetInt("location_type", 0);
      if (type == StopLocationType.StopPlatform || type == StopLocationType.Station || type == StopLocationType.EntranceExit)
      {
        props.AssertExists("stop_name");
        props.AssertDecimal("stop_lat");
        props.AssertDecimal("stop_lon");
      }

      if (type == StopLocationType.EntranceExit || type == StopLocationType.GenericNode || type == StopLocationType.BoardingArea)
      {
        props.AssertExists("parent_station");
      }

      if (type == StopLocationType.Station && props.ContainsKey("parent_station")) throw new PropertyException("parent_station", "parent_station not allowed on Stations");

      decimal? lat = props.GetNullableDecimal("stop_lat");
      if (lat.HasValue && (lat < -90 || lat > 90)) throw new PropertyRangeException("stop_lat", "Latitude must be between -90 and 90.");
      decimal? lon = props.GetNullableDecimal("stop_lon");
      if (lon.HasValue && (lon < -180 || lon > 180)) throw new PropertyRangeException("stop_lon", "Longitude must be between -180 and 180.");

      if (props.ContainsKey("level_id"))
      {
        props.AssertForeignKeyExists("level_id", Levels, "levels");
      }

      return new Stop(props);
    }

    private Trip TripFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("trip_id");
      props.AssertForeignKeyExists("route_id", Routes, "routes");
      props.AssertForeignKeyExists("service_id", Calendars, "calendars");
      return new Trip(props);
    }

    private StopTime StopTimeFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertForeignKeyExists("trip_id", Trips, "trips");
      props.AssertForeignKeyExists("stop_id", Stops, "stops");
      props.AssertNonNegativeInt("stop_sequence");

      if (props["timepoint"] == "1")
      {
        props.AssertTime("arrival_time");
        props.AssertTime("departure_time");
      }

      return new StopTime(props);
    }

    private FareAttribute FareAttributeFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties, DefaultAgencyID);
      props.AssertExists("fare_id");
      props.AssertForeignKeyExists("agency_id", Agencies, "agencies");
      props.AssertNonNegativeDecimal("price");
      props.AssertExists("currency_type");
      props.AssertNonNegativeInt("payment_method");
      return new FareAttribute(props);
    }

    private FareRule FareRuleFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("fare_id");
      if (!(props.ContainsKey("route_id") || props.ContainsKey("origin_id") || props.ContainsKey("destination_id") ||
        props.ContainsKey("contains_id")))
        throw new PropertyNullException("route_id, origin_id, destination_id, contains_id", "One of these properties must be provided.");
      return new FareRule(props);
    }

    private ShapePoint ShapePointFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("shape_id");
      props.AssertDecimal("shape_pt_lat");
      props.AssertDecimal("shape_pt_lon");
      props.AssertInt("shape_pt_sequence");
      return new ShapePoint(props);
    }

    private Frequency FrequencyFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertForeignKeyExists("trip_id", Trips, "trips");
      props.AssertTime("start_time");
      props.AssertTime("end_time");
      props.AssertDuration("headway_secs");
      return new Frequency(props);
    }

    private Transfer TransferFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertForeignKeyExists("from_stop_id", Stops, "stops");
      props.AssertForeignKeyExists("to_stop_id", Stops, "stops");
      props.AssertInt("transfer_type");
      if (props["transfer_type"] == "2")
      {
        props.AssertDuration("min_transfer_time");
      }
      return new Transfer(props);
    }

    private Pathway PathwayFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("pathway_id");
      props.AssertForeignKeyExists("from_stop_id", Stops, "stops");
      props.AssertForeignKeyExists("to_stop_id", Stops, "stops");
      props.AssertNonNegativeInt("pathway_mode");
      props.AssertBool("is_bidirectional");
      if ((props["pathway_mode"] == "6" || props["pathway_mode"] == "7") && props["is_bidirectional"] == "1")
      {
        throw new PropertyException("is_bidirectional", "Fare gates (pathway_mode = 6) and exit gates (pathway_mode = 7) cannot be bidirectional.");
      }
      return new Pathway(props);
    }

    private Level LevelFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("level_id");
      props.AssertDouble("level_index");
      return new Level(props);
    }

    private Translation TranslationFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("table_name");
      props.AssertExists("field_name");
      props.AssertExists("language");
      props.AssertExists("translation");

      if (props.ContainsKey("field_value"))
      {
        if (props.ContainsKey("record_id") || props.ContainsKey("record_sub_id")) throw new PropertyException("record_id/record_sub_id and field_value", "Not simultaneously allowed");
        if (props["table_name"] == "feed_info") throw new PropertyException("field_value", "Not allowed if table_name == field_info");
      }
      else if (props.ContainsKey("record_id"))
      {
        if (props["table_name"] == "feed_info") throw new PropertyException("record_id", "Not allowed if table_name == field_info");
      }
      else if (props.ContainsKey("record_sub_id")) throw new PropertyException("record_sub_id", "Not allowed without record_id");
      else if (props["table_name"] != "feed_info") throw new PropertyNullException("field_value and record_id", "One of these is required unless table_name == feed_info");

      return new Translation(props);
    }

    private Attribution AttributionFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("organization_name");

      if (props.ContainsKey("agency_id"))
      {
        if (props.ContainsKey("route_id") || props.ContainsKey("trip_id")) throw new PropertyException("agency_id, route_id, and trip_id", "Only one is allowed.");
        props.AssertForeignKeyExists("agency_id", Agencies, "agency");
      }
      else if (props.ContainsKey("route_id"))
      {
        if (props.ContainsKey("trip_id")) throw new PropertyException("agency_id, route_id, and trip_id", "Only one is allowed.");
        props.AssertForeignKeyExists("route_id", Routes, "routes");
      }
      else if (props.ContainsKey("trip_id"))
      {
        props.AssertForeignKeyExists("trip_id", Trips, "trips");
      }
      else throw new PropertyNullException("agency_id, route_id, and trip_id", "One of these must be specified.");

      return new Attribution(props);
    }

    private FeedInfo FeedInfoFactory(IEnumerable<(string, string)> properties)
    {
      GTFSPropertyCollection props = new GTFSPropertyCollection(properties);
      props.AssertExists("feed_publisher_name");
      props.AssertExists("feed_publisher_url");
      props.AssertExists("feed_lang");

      return new FeedInfo(props);
    }
  }
}