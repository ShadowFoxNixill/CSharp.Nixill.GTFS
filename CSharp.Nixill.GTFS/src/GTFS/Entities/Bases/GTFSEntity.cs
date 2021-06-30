using Nixill.GTFS.Collections;
using System.Linq;

namespace Nixill.GTFS.Entities
{
  public class GTFSEntity
  {
    protected GTFSPropertyCollection Properties;

    public GTFSEntity(GTFSPropertyCollection properties)
    {
      Properties = properties;
    }

    public string this[string key] => Properties[key];

    public override bool Equals(object other)
    {
      if (!(other is GTFSEntity otherEnt)) return false;

      if (Properties.Count != otherEnt.Properties.Count) return false;

      foreach (var prop in Properties)
      {
        if (otherEnt[prop.Key] != prop.Value) return false;
      }

      foreach (var prop in otherEnt.Properties)
      {
        if (this[prop.Key] != prop.Value) return false;
      }

      return true;
    }

    public override int GetHashCode()
    {
      int code = 0;

      foreach (var prop in Properties)
      {
        code ^= prop.Key.GetHashCode();
        code ^= prop.Value.GetHashCode();
      }

      return code;
    }
  }
}