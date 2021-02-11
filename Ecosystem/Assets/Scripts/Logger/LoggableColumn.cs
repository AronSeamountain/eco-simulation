using System;
using System.Collections.Generic;
using Utils;

namespace Logger
{
  public sealed class LoggableColumn
  {
    private readonly Func<int, IList<Animal>, string> _valueImplementation;

    public LoggableColumn(
      string columnName,
      Func<int, IList<Animal>, string> valueImplementation
    )
    {
      Objects.RequireNonNull(columnName, "Column name can not be null.");
      Objects.RequireNonNull(valueImplementation, "Value implementation can not be null");

      ColumnName = columnName;
      _valueImplementation = valueImplementation;
    }

    public string ColumnName { get; }

    public string GetValue(int day, IList<Animal> animal)
    {
      return _valueImplementation.Invoke(day, animal);
    }
  }
}