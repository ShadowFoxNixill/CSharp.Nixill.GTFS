using System.Collections.Generic;
using System.Linq;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSEntity
  {
    public readonly GTFSFeed Feed;
    protected Dictionary<string, string> Properties;

    public GTFSEntity(GTFSFeed feed, IDictionary<string, string> properties)
    {
      Feed = feed;
      Properties = new Dictionary<string, string>(properties);
    }

    public string GetProperty(string name)
    {
      if (Properties.ContainsKey(name))
        return Properties[name];
      else
        return null;
    }

    public string this[string key] => Properties[key];
  }
}