using System;
using System.Collections.Generic;
using Animal;
using Core;
using Utils;

namespace Logger
{
  public sealed class LoggableColumn
  {
    private readonly Func<int, IList<AbstractAnimal>, EntityManager, string> _valueImplementation;

    public LoggableColumn(
      string columnName,
      Func<int, IList<AbstractAnimal>, EntityManager, string> valueImplementation
    )
    {
      Objects.RequireNonNull(columnName, "Column name can not be null.");
      Objects.RequireNonNull(valueImplementation, "Value implementation can not be null");

      ColumnName = columnName;
      _valueImplementation = valueImplementation;
    }

    public string ColumnName { get; }

    public string GetValue(int day, IList<AbstractAnimal> animal, EntityManager entityManager)
    {
      return _valueImplementation.Invoke(day, animal, entityManager);
    }
  }
}