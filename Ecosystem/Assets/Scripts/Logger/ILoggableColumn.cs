using System;

namespace Logger
{
  public sealed class LoggableColumn
  {
    private Func<Animal, string> _valueImplementation;

    public LoggableColumn(string columnName, Func<Animal, string> valueImplementation)
    {
      if (columnName == null) throw new NullReferenceException("Column name can not be null.");
      if (valueImplementation == null) throw new NullReferenceException("Value implementation can not be null");

      ColumnName = columnName;
      _valueImplementation = valueImplementation;
    }

    public string ColumnName { get; set; }

    public string GetValue(Animal animal)
    {
      return _valueImplementation.Invoke(animal);
    }
  }
}