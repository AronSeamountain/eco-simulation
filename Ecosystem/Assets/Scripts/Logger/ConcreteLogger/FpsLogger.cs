using System.Collections.Generic;
using System.Globalization;

namespace Logger.ConcreteLogger
{
  public sealed class FpsLogger : AbstractCsvLogger
  {
    public FpsLogger()
    {
      Path = "Assets/Logs/fps.csv";
    }

    protected override IList<LoggableColumn> GetLoggableColumns()
    {
      return new List<LoggableColumn>
      {
        new LoggableColumn("day", (day, animals, em) =>
          day.ToString()
        ),
        new LoggableColumn("frames", (day, animals, em) =>
          em.FpsDelegate.FramesSinceLastSnapshot.ToString()
        ),
        new LoggableColumn("time", (day, animals, em) =>
          em.FpsDelegate.TimeSinceLastSnapshot.ToString(CultureInfo.InvariantCulture)
        ),
        new LoggableColumn("average", (day, animals, em) =>
          (em.FpsDelegate.FramesSinceLastSnapshot / em.FpsDelegate.TimeSinceLastSnapshot).ToString(CultureInfo
            .InvariantCulture)
        )
      };
    }
  }
}