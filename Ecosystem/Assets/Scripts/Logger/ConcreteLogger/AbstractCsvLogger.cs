using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Animal;
using Core;

namespace Logger.ConcreteLogger
{
  /// <summary>
  ///   Singleton logger class that logs the world. Logs average values.
  /// </summary>
  public abstract class AbstractCsvLogger : ILogger
  {
    private const string Delimiter = ",";
    private readonly IList<LoggableColumn> _loggableColumns;
    private bool _firstLog;
    private IList<string> _snapshots;
    protected string Path;

    protected AbstractCsvLogger()
    {
      _loggableColumns = GetLoggableColumns();
      _snapshots = new List<string>();
      _firstLog = true;
    }

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

    protected abstract IList<LoggableColumn> GetLoggableColumns();

    #region Helper

    protected string CalcAverage(ICollection<AbstractAnimal> animals, Func<AbstractAnimal, float> animalAspect)
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
  }
}