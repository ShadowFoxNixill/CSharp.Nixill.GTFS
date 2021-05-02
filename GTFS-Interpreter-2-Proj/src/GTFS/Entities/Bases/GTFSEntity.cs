using System.Collections.Generic;
using System.Linq;
using Nixill.GTFS.Collections;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSEntity
  {
    public readonly GTFSFeed Feed;
    protected GTFSPropertyCollection Properties;

    public GTFSEntity(GTFSFeed feed, IDictionary<string, string> properties) : this(feed, new GTFSPropertyCollection(properties))
    { }

    public GTFSEntity(GTFSFeed feed, IEnumerable<(string, string)> properties) : this(feed, new GTFSPropertyCollection(properties))
    { }

    public GTFSEntity(GTFSFeed feed, GTFSPropertyCollection properties)
    {
      Feed = feed;
      Properties = properties;
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