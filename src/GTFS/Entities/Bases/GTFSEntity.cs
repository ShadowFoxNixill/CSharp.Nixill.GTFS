using System.Collections.Generic;
using System.Linq;

namespace Nixill.GTFS.Entities
{
  public abstract class GTFSEntity
  {
    public readonly GTFSFile File;
    private Dictionary<string, string> _Properties;

    public GTFSEntity(GTFSFile file, IDictionary<string, string> properties)
    {
      File = file;
      _Properties = new Dictionary<string, string>(properties);
    }
  }
}