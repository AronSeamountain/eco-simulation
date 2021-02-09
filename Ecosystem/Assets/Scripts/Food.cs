using UnityEngine;

public sealed class Food : MonoBehaviour, IEatable
{
  [SerializeField] private int saturation;

  public int Saturation()
  {
    return saturation;
  }

  public void Consume()
  {
    Destroy(gameObject);
  }
}