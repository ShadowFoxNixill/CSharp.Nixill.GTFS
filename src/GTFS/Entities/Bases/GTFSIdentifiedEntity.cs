namespace Nixill.GTFS.Entities
{
  public abstract class GTFSIdentifiedEntity : GTFSEntity
  {
    public readonly string ID;

    public GTFSIdentifiedEntity(GTFSFile file, string id) : base(file)
    {
      ID = id;
    }
  }
}