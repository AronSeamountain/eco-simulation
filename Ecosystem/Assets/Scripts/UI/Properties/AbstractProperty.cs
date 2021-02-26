using UnityEngine;

namespace UI.Properties
{
  public abstract class AbstractProperty : MonoBehaviour
  {
    public delegate void Cleanup();

    public Cleanup CleanupListeners;

    public void ExitCleanup()
    {
      CleanupListeners?.Invoke();
    }
  }
}