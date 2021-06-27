using System;

namespace Nixill.GTFS.Parsing.Exceptions
{
  public class PropertyException : SystemException
  {
    public readonly string PropertyName;

    public PropertyException(string property = null, string message = null, Exception inner = null)
    : base(message ?? ((property != null) ? $"Exception in property {property}." : $"Exception in property."), inner)
    {
      PropertyName = property;
    }
  }

  public class PropertyNullException : PropertyException
  {
    public PropertyNullException(string property = null, string message = null, Exception inner = null)
    : base(property, message ?? ((property != null) ? $"Property {property} is null." : $"Null property provided."), inner) { }
  }

  public class PropertyRangeException : PropertyException
  {
    public PropertyRangeException(string property = null, string message = null, Exception inner = null)
    : base(property, message ?? ((property != null) ? $"Property {property} is out of range." : $"A property was out of range."), inner) { }
  }

  public class PropertyEnumException : PropertyException
  {
    public PropertyEnumException(string property = null, string message = null, Exception inner = null)
    : base(property, message ?? ((property != null) ? $"Property {property} has an invalid enum value." : $"A property has an invalid enum value."), inner) { }
  }

  public class PropertyTypeException : PropertyException
  {
    public PropertyTypeException(string property = null, string message = null, Exception inner = null)
    : base(property, message ?? ((property != null) ? $"Property {property} has an invalid type." : $"A property has an invalid type."), inner) { }
  }

  public class PropertyForeignKeyException : PropertyException
  {
    public PropertyForeignKeyException(string property = null, string message = null, Exception inner = null)
    : base(property, message ?? ((property != null) ? $"Property {property} has an invalid foreign key." : $"A property has an invalid foreign key."), inner) { }
  }
}