using System.Collections.Generic;
using UI.Properties;

public interface IInspectable
{
  /// <summary>
  /// </summary>
  /// <param name="value">bool to show stats for the entity</param>
  IList<AbstractProperty> GetStats(bool getStats);
}