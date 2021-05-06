namespace Nixill.GTFS.Enumerations
{
  /// <summary>Represents a yes/no/unknown value.</summary>
  public enum Tristate
  {
    /// <value>The value is not known / no information is provided.</value>
    Unknown = 0,
    /// <value>The value is true / allowed.</value>
    Yes = 1,
    /// <value>The value is false / not allowed.</value>
    No = 2
  }

  /// <summary>The type of transportation used on a route.</summary>
  public enum RouteType
  {
    /// <value>
    ///   Tram, Streetcar, Light rail. Any light rail or street level
    ///   system within a metropolitan area.
    /// </value>
    Tram = 0,
    /// <value>
    ///   Subway, Metro. Any underground rail system within a metropolitan
    ///   area.
    /// </value>
    Metro = 1,
    /// <value>Rail. Used for intercity or long-distance travel.</value>
    Rail = 2,
    /// <value>Bus. Used for short- and long-distance bus routes.</value>
    Bus = 3,
    /// <value>Ferry. Used for short- and long-distance boat service.</value>
    Ferry = 4,
    /// <value>
    ///   Cable tram. Used for street-level rail cars where the cable runs
    ///   beneath the vehicle, e.g., cable car in San Francisco.
    /// </value>
    CableTram = 5,
    /// <value>
    ///   Aerial lift, suspended cable car (e.g., gondola lift, aerial
    ///   tramway). Cable transport where cabins, cars, gondolas or open
    ///   chairs are suspended by means of one or more cables.
    /// </value>
    AerialLift = 6,
    /// <value>Funicular. Any rail system designed for steep inclines.</value>
    Funicular = 7,
    /// <value>
    ///   Trolleybus. Electric buses that draw power from overhead wires
    ///   using poles.
    /// </value>
    Trolleybus = 11,
    /// <value>
    ///   Monorail. Railway in which the track consists of a single rail
    ///   or a beam.
    /// </value>
    Monorail = 12
  }

  /// <summary>
  ///   Whether or not boardings or alightings are allowed at or between stops.
  /// </summary>
  public enum PickupDropoffType
  {
    /// <value>Regularly scheduled pickup or drop off.</value>
    Available = 0,
    /// <value>Pickup or drop off is unavailable.</value>
    Unavalable = 1,
    /// <value>Must phone agency to arrange pickup or drop off.</value>
    PhoneAgency = 2,
    /// <value>
    ///   Must coordinate with a driver to arrange pickup or drop off.
    /// </value>
    AskTheDriver = 3
  }

  /// <summary>
  ///   Whether service is added or removed for a given calendar and date.
  /// </summary>
  public enum ExceptionType
  {
    /// <value>Service is added for that calendar and date.</value>
    Added = 1,
    /// <value>Service is removed for that calendar and date.</value>
    Removed = 2
  }

  /// <summary>
  ///   The type of a location in the Stops table.
  /// </summary>
  public enum StopLocationType
  {
    /// <value>
    ///   Stop (or Platform). A location where passengers board or
    ///   disembark from a transit vehicle. Is called a platform when
    ///   defined within a <c>ParentStation</c>.
    /// </value>
    StopPlatform = 0,
    /// <value>
    ///   Station. A physical structure or area that contains one or more
    ///   platforms.
    /// </value>
    Station = 1,
    /// <value>
    ///   Entrance/Exit. A location where passengers can enter or exit a
    ///   station from the street. If an entrance/exit belongs to multiple
    ///   stations, it can be linked by pathways to both, but the data
    ///   provider must pick one of them as parent.
    /// </value>
    EntranceExit = 2,
    /// <value>
    ///   Generic Node. A location within a station, not matching any
    ///   other location_type, which can be used to link together pathways
    ///   defined in the <c>pathways</c> table.
    GenericNode = 3,
    /// <value>
    ///   Boarding Area. A specific location on a platform, where
    ///   passengers can board and/or alight vehicles.
    /// </value>
    BoardingArea = 4
  }

  public enum DirectionId
  {
    /// <value>Travel in one direction (e.g. outbound travel).</value>
    OneDirection = 0,
    /// <value>Travel in the opposite direction (e.g. inbound travel).</value>
    OtherDirection = 1
  }
}