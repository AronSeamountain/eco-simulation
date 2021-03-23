using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Animal;
using Core;
using TMPro;
using UnityEngine;

namespace Logger.ConcreteLogger
{
  /// <summary>
  ///   Singleton logger class that logs the world. Logs average values.
  /// </summary>
  public sealed class OverviewLogger : ILogger
  {
    private const string Delimiter = ",";
    private const string Path = "Assets/Logs/overview.csv";
    private readonly IList<LoggableColumn> _loggableColumns;
    private bool _firstLog;
    private IList<string> _snapshots;

    static OverviewLogger()
    {
    }

    private OverviewLogger()
    {
      _loggableColumns = GetLoggableColumns();
      _snapshots = new List<string>();
      _firstLog = true;
    }

    public static OverviewLogger Instance { get; } = new OverviewLogger();

    private IList<LoggableColumn> GetLoggableColumns()
    {
      return new List<LoggableColumn>
      {
        new LoggableColumn("day", (day, animals, em) =>
          day.ToString()
        ),
        new LoggableColumn("amount", (day, animals, em) =>
          animals.Count.ToString()
        ),
        new LoggableColumn("amount_rabbits", (day, animals, em) =>
          em.HerbivoreCount.ToString()
        ),
        new LoggableColumn("amount_wolfs", (day, animals, em) =>
          em.CarnivoreCount.ToString()
        ),
        new LoggableColumn(
          "saturation",
          (day, animals, em) => CalcAverage(animals, animal => animal.GetSaturation())
        ),
        new LoggableColumn(
          "hydration",
          (day, animals, em) => CalcAverage(animals, animal => animal.GetHydration())
        ),
        new LoggableColumn(
          "speed",
          (day, animals, em) => CalcAverage(animals, animal => animal.SpeedModifier)
        ),
        new LoggableColumn(
          "age",
          (day, animals, em) => CalcAverage(animals, animal => animal.AgeInDays)
        ),
        new LoggableColumn(
          "size",
          (day, animals, em) => CalcAverage(animals, animal => animal.SizeModifier)
        )
      };
    }

    #region Helper

    private string CalcAverage(ICollection<AbstractAnimal> animals, Func<AbstractAnimal, float> animalAspect)
    {
      if (animals.Any())
        return (animals.Sum(animalAspect) / animals.Count).ToString();

      return 0.ToString();
    }

    #endregion

    #region RowHandling

    public void Clear()
    {
      File.WriteAllText(Path, string.Empty);
    }

    private void AppendHeader()
    {
      var headerNames = _loggableColumns.Select(column => column.ColumnName).ToList();
      var header = string.Join(Delimiter, headerNames);

      var writer = File.AppendText(Path);
      writer.WriteLine(header);
      writer.Close();
    }

    #endregion

    public void Snapshot(EntityManager entityManager)
    {
      var values = _loggableColumns.Select(column => column.GetValue(entityManager)).ToList();
      var snapshot = string.Join(Delimiter, values);
      _snapshots.Add(snapshot);
    }

    public void Persist()
    {
      if (_firstLog)
      {
        Clear();
        AppendHeader();
        _firstLog = false;
      }

      var writer = File.AppendText(Path);

      foreach (var snapshot in _snapshots)
        writer.WriteLine(snapshot);

      writer.Close();

      _snapshots = new List<string>();
    }
  }
}