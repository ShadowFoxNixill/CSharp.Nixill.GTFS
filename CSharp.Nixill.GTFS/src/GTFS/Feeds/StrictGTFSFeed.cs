using Nixill.GTFS.Collections;
using Nixill.GTFS.Entities;
using System.Linq;
using Nixill.GTFS.Sources;
using System.Collections.Generic;
using Nixill.GTFS.Parsing;
using Nixill.GTFS.Parsing.Exceptions;
using Nixill.GTFS.Enumerations;

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
      Stops = new IDEntityCollection<Stop>(DataSource, "stops", StopFactory);
      Trips = new IDEntityCollection<Trip>(DataSource, "trips", TripFactory);
      StopTimes = new GTFSOrderedEntityCollection<StopTime>(DataSource, "stop_times", StopTimeFactory);
      FareAttributes = new IDEntityCollection<FareAttribute>(DataSource, "fare_attributes", FareAttributeFactory);
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
  }
}