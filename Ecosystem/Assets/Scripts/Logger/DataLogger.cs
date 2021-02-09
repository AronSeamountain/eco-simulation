using System.Collections.Generic;
using System.IO;
using System.Linq;

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
      _loggableColumns = new List<LoggableColumn>
      {
        new LoggableColumn("day", (day, animals) =>
          day.ToString()
        ),
        new LoggableColumn("amount", (day, animals) =>
          animals.Count.ToString()
        ),
        new LoggableColumn("saturation", (day, animals) =>
          {
            if (animals.Any())
              return (animals.Sum(animal => animal.GetSaturation()) / animals.Count).ToString();

            return 0.ToString();
          }
        ),
        new LoggableColumn("hydration", (day, animals) =>
          {
            if (animals.Any())
              return (animals.Sum(animal => animal.GetHydration()) / animals.Count).ToString();

            return 0.ToString();
          }
        )
      };
    }

    public static DataLogger Instance { get; } = new DataLogger();

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

    public void Snapshot(int day, IList<Animal> animals)
    {
      AppendRow(CreateRow(day, animals));
    }

    private string CreateRow(int day, IList<Animal> animals)
    {
      // TODO: Could be converted to a string builder to enhance performance if we log often.
      var values = _loggableColumns.Select(column => column.GetValue(day, animals)).ToList();
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
  }
}