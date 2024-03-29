﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    protected string FileName;
    protected string Path;
    protected readonly CultureInfo LoggerCultureInfo;

    protected AbstractCsvLogger()
    {
      LoggerCultureInfo = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
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
        return (animals.Sum(animalAspect) / animals.Count).ToString(LoggerCultureInfo);

      return 0.ToString();
    }

    #endregion

    #region RowHandling

    public void Clear()
    {
      File.Create(Path).Dispose();
    }

    public void Reset(int days)
    {
      if (File.Exists(Path))
      {
        var time = DateTime.Now;
        var newDirName = time.Month + "-" + time.Day + "_" + time.Hour + time.Minute + "_" + days + "Days";
        if (!Directory.Exists(newDirName))
          Directory.CreateDirectory(newDirName);
        MoveTo(newDirName);
      }

      Clear();
    }

    public void MoveTo(string newDirName)
    {
      var target = newDirName + "/" + FileName;
      if (!File.Exists(target))
        File.Move(Path, target);
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