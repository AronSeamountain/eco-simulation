using System.Collections.Generic;

namespace Logger.ConcreteLogger
{
  public sealed class OverviewLogger : AbstractCsvLogger
  {
    public OverviewLogger()
    {
      Path = "Assets/Logs/overview.csv";
    }

    protected override IList<LoggableColumn> GetLoggableColumns()
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
  }
}