using System.Collections.Generic;
using Nixill.GTFS.Collections;
using Nixill.GTFS.Parsing;

namespace Nixill.GTFS.Entities
{
  public class ShapePoint : GTFSOrderedEntity
  {
    public string ShapeID => Properties["shape_id"];
    public int ShapePointSequence => Properties.GetInt("shape_pt_sequence");
    public decimal ShapePointLatitude => Properties.GetDecimal("shape_pt_lat");
    public decimal ShapePointLongitude => Properties.GetDecimal("shape_pt_lon");
    public decimal? ShapePointDistTraveled => Properties.GetNullableDecimal("shape_dist_traveled");

    public ShapePoint(GTFSPropertyCollection props) : base(props, "shape_id", "shape_pt_sequence") { }
  }
}