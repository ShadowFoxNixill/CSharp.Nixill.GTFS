using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class Translation : GTFSEntity
  {
    public Translation(GTFSPropertyCollection properties) : base(properties) { }

    public string TableName => Properties["table_name"];
    public string FieldName => Properties["field_name"];
    public string Language => Properties["language"];
    public string TranslationValue => Properties["translation"];
    public string RecordID => Properties["record_id"];
    public string RecordSubID => Properties["record_sub_id"];
    public string FieldValue => Properties["field_value"];
  }
}