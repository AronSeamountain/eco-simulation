using System.Collections.Generic;
using UnityEngine;

public interface IStatable
{
  /// <summary>
  /// </summary>
  /// <param name="value">bool to show stats for the entity</param>
  IList<GameObject> GetStats();
}