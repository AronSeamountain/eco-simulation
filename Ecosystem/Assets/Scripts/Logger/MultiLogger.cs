using System.Collections.Generic;
using Core;

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
  }
}