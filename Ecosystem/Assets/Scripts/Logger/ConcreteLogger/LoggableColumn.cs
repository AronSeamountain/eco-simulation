using System;
using System.Collections.Generic;
using Animal;
using Core;
using Utils;

namespace Logger.ConcreteLogger
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

    public string GetValue(EntityManager entityManager)
    {
      return _valueImplementation.Invoke(entityManager.Days, entityManager.Animals, entityManager);
    }
  }
}