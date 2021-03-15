using System;
using System.Collections.Generic;

namespace Logger.ConcreteLogger
{
  /// <summary>
  ///   Singleton logger class that logs the world. Logs average values.
  /// </summary>
  public sealed class OverviewLogger : AbstractCsvLogger
  {
    public OverviewLogger()
    {
      Path = "Assets/Logs/log.csv";
    }

    protected override IList<LoggableColumn> GetLoggableColumns()
    {
      return new List<LoggableColumn>
      {
        new LoggableColumn(
          "day",
          em => em.Days.ToString()
        ),
        new LoggableColumn(
          "amount",
          em => em.Animals.Count.ToString()
        ),
        new LoggableColumn(
          "amount_rabbits",
          em => em.HerbivoreCount.ToString()
        ),
        new LoggableColumn(
          "amount_wolfs",
          em => em.CarnivoreCount.ToString()
        ),
        new LoggableColumn(
          "saturation",
          em => CalcAverage(em.Animals, animal => animal.GetSaturation())
        ),
        new LoggableColumn(
          "hydration",
          em => CalcAverage(em.Animals, animal => animal.GetHydration())
        ),
        new LoggableColumn(
          "speed",
          em => CalcAverage(em.Animals, animal => animal.SpeedModifier)
        ),
        new LoggableColumn(
          "age",
          em => CalcAverage(em.Animals, animal => animal.AgeInDays)
        ),
        new LoggableColumn(
          "size",
          em => CalcAverage(em.Animals, animal => animal.SizeModifier)
        )
      };
    }
  }
}