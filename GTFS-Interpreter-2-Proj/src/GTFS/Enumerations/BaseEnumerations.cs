namespace Nixill.GTFS.Enumerations
{
  public enum RouteType
  {
    Tram = 0,
    Metro = 1,
    Rail = 2,
    Bus = 3,
    Ferry = 4,
    CableTram = 5,
    AerialLift = 6,
    Funicular = 7,
    Trolleybus = 11,
    Monorail = 12
  }

  public enum PickupDropoffType
  {
    Available = 0,
    Unavalable = 1,
    PhoneAgency = 2,
    AskTheDriver = 3
  }

  public enum ExceptionType
  {
    Added = 1,
    Removed = 2
  }

  public enum StopLocationType
  {
    StopPlatform = 0,
    Station = 1,
    EntranceExit = 2,
    GenericNode = 3,
    BoardingArea = 4
  }

  public enum Tristate
  {
    Unknown = 0,
    Yes = 1,
    No = 2
  }
}