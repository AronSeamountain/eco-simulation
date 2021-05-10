using System;
using System.Collections.Generic;
using System.IO;
using Core;
using UnityEngine;

namespace Logger
{
  public sealed class MultiLogger : ILogger
  {
    private readonly IList<ILogger> _loggers;

    public MultiLogger(params ILogger[] loggers)
    {
      _loggers = new List<ILogger>(loggers);
    }

    public void Persist()
    {
      foreach (var logger in _loggers) logger.Persist();
    }

    public void Snapshot(EntityManager entityManager)
    {
      foreach (var logger in _loggers) logger.Snapshot(entityManager);
    }

    public void Clear()
    {
      foreach (var logger in _loggers) logger.Clear();
    }

    public void Reset(int days)
    {
      var time = DateTime.Now;
      var newDirName = time.Month + "-" + time.Day + "_" + time.Hour + time.Minute + "_" + days + "Days";
      if (!Directory.Exists(newDirName))
        Directory.CreateDirectory(newDirName);
      foreach (var logger in _loggers)
      {
        logger.MoveTo(newDirName);
        logger.Clear();
      }
    }

    public void MoveTo(string newDirName)
    {
      foreach (var logger in _loggers)
        logger.MoveTo(newDirName);
    }
  }
}