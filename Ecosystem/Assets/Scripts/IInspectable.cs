using System.Collections.Generic;
using UnityEngine;

public interface IInspectable
{
  /// <summary>
  /// </summary>
  /// <param name="value">bool to show stats for the entity</param>
  IList<MonoBehaviour> GetStats(bool getStats);
}