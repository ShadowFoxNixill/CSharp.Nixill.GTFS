using System.Collections.Generic;
using System.IO;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Enumerations;
using Nixill.GTFS.Parsing;
using NodaTime;

namespace Nixill.GTFS.Entities
{
  public class CalendarDate : GTFSTwoPartEntity<string, LocalDate>
  {
    public string ServiceID => Properties["service_id"];
    public LocalDate Date => Properties.GetDate("date");
    public ExceptionType ExceptionType => (ExceptionType)Properties.GetInt("exception_type");
    public bool IsAdded => ExceptionType == ExceptionType.Added;
    public bool IsRemoved => ExceptionType == ExceptionType.Removed;

    private CalendarDate(GTFSPropertyCollection properties) : base(properties, properties["service_id"], properties.GetDate("date"))
    {
      if (!properties.IsInt("exception_type")) throw new InvalidDataException("Calendar dates must have exception types.");
    }

    public static CalendarDate Factory(IEnumerable<(string, string)> properties)
    {
      return new CalendarDate(new GTFSPropertyCollection(properties));
    }
  }
}