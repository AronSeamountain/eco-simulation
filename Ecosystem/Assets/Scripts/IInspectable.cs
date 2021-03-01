using System.Collections.Generic;
using UI.Properties;

public interface IInspectable
{
  /// <summary>
  ///   Returns a list of displayable properties.
  /// </summary>
  IEnumerable<AbstractProperty> GetProperties();

  /// <summary>
  ///   Tells the inspectable object whether it should show its gizmos.
  /// </summary>
  /// <param name="show">True if it should show.</param>
  void ShowGizmos(bool show);
}