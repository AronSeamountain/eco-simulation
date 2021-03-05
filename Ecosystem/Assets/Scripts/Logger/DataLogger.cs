using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Animal;
using Core;

namespace Logger
{
  /// <summary>
  ///   Singleton logger class that logs the world.
  /// </summary>
  public sealed class DataLogger
  {
    private const string Delimiter = ",";
    private const string Path = "Assets/Logs/log.csv";
    private readonly IList<LoggableColumn> _loggableColumns;

    static DataLogger()
    {
      // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    }

    private DataLogger()
    {
      _loggableColumns = GetLoggableColumns();
    }

    public static DataLogger Instance { get; } = new DataLogger();

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
          (day, animals, em) => CalcAverage(day, animals, animal => animal.GetSaturation())
        ),
        new LoggableColumn(
          "hydration",
          (day, animals, em) => CalcAverage(day, animals, animal => animal.GetHydration())
        ),
        new LoggableColumn(
          "speed",
          (day, animals, em) => CalcAverage(day, animals, animal => animal.GetSpeedModifier())
        ),
        new LoggableColumn(
          "age",
          (day, animals, em) => CalcAverage(day, animals, animal => animal.AgeInDays)
        ),
        new LoggableColumn(
          "size",
          (day, animals, em) => CalcAverage(day, animals, animal => animal.GetSize())
        )
      };
    }

    #region Helper

    private string CalcAverage(int day, ICollection<AbstractAnimal> animals, Func<AbstractAnimal, float> animalAspect)
    {
      if (animals.Any())
        return (animals.Sum(animalAspect) / animals.Count).ToString();

      return 0.ToString();
    }

    #endregion

    #region RowHandling

    /// <summary>
    ///   Removes all the content from the file.
    /// </summary>
    private void Clear()
    {
      File.WriteAllText(Path, string.Empty);
    }

    /// <summary>
    ///   Clears the log file and appends the header.
    /// </summary>
    public void InitializeLogging()
    {
      Clear();
      AppendHeader();
    }

    private void AppendHeader()
    {
      var headerNames = _loggableColumns.Select(column => column.ColumnName).ToList();
      var header = string.Join(Delimiter, headerNames);

      AppendRow(header);
    }

    public void Snapshot(int day, IList<AbstractAnimal> animals, EntityManager entityManager)
    {
      AppendRow(CreateRow(day, animals, entityManager));
    }

    private string CreateRow(int day, IList<AbstractAnimal> animals, EntityManager entityManager)
    {
      // TODO: Could be converted to a string builder to enhance performance if we log often.
      var values = _loggableColumns.Select(column => column.GetValue(day, animals, entityManager)).ToList();
      var row = string.Join(Delimiter, values);
      return row;
    }

    /// <summary>
    ///   Appends a string on a new row.
    /// </summary>
    /// <param name="row">The string to append.</param>
    private void AppendRow(string row)
    {
      var writer = File.AppendText(Path);
      writer.WriteLine(row);
      writer.Close();
    }

    #endregion
  }
}