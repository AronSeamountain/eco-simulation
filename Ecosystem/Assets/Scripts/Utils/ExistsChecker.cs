using UnityEngine;

namespace Utils
{
  public static class ExistsChecker
  {
    /// <summary>
    ///   Checks that the mono behaviour is not null, not destroyed and not "inactive". A object that is not null but is
    ///   inactive does not "exist".
    /// </summary>
    /// <param name="monoBehaviour">The mono behaviour to check.</param>
    public static bool Exists(this MonoBehaviour monoBehaviour)
    {
      if (monoBehaviour == null) return false; // Actually null
      if (!monoBehaviour) return false; // Unity "null"

      return monoBehaviour.isActiveAndEnabled;
    }

    /// <summary>
    ///   Checks whether the mono behaviour is null, destroyed or "inactive". A object that is not null but is inactive does
    ///   not "exist".
    /// </summary>
    /// <param name="monoBehaviour">The mono behaviour to check if it does not exist.</param>
    public static bool DoesNotExist(this MonoBehaviour monoBehaviour)
    {
      return !Exists(monoBehaviour);
    }
  }
}