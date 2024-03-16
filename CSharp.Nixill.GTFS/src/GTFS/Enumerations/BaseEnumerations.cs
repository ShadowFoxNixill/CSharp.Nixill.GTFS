using System;

namespace Nixill.GTFS.Enumerations
{
  public enum Tristate
  {
    Unknown = 0,
    Yes = 1,
    No = 2
  }

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

  public enum DirectionId
  {
    OneDirection = 0,
    OtherDirection = 1
  }

  public enum FarePaymentMethod
  {
    OnBoard = 0,
    BeforeBoarding = 1
  }

  public enum TransferType
  {
    Recommended = 0,
    Timed = 1,
    MinimumTime = 2,
    NotPossible = 3,
    InSeat = 4,
    InterSeat = 5
  }

  public enum PathwayMode
  {
    Walkway = 1,
    Stairs = 2,
    MovingSidewalk = 3,
    Escalator = 4,
    Elevator = 5,
    FareGate = 6,
    ExitGate = 7
  }

  public enum DurationLimitType
  {
    DepartToArrive = 0,
    DepartToDepart = 1,
    ArriveToDepart = 2,
    ArriveToArrive = 3
  }

  public enum FareTransferType
  {
    FirstLeg = 0,
    AllLegs = 1,
    NoLegs = 2
  }

  public enum FareMediaType
  {
    Cash = 0,
    PaperTicket = 1,
    TransitCard = 2,
    BankCard = 3,
    MobileApp = 4
  }
}