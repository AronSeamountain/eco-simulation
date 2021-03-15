using System.Collections.Generic;

namespace Logger.ConcreteLogger
{
  public sealed class DetailedCsvLogger : AbstractCsvLogger
  {
    public DetailedCsvLogger()
    {
      Path = "Assets/Logs/detailed_log.csv";
    }

    protected override IList<LoggableColumn> GetLoggableColumns()
    {
      return new List<LoggableColumn>
      {
        new LoggableColumn(
          "day",
          em => em.Days.ToString()
        )
      };
    }
  }
}