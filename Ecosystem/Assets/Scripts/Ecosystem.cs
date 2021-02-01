using UnityEngine;

public class Ecosystem : MonoBehaviour
{
  private void Start()
  {
    var logger = DataLogger.Instance;
    logger.Do();
  }
}
