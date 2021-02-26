using UnityEngine;

namespace Foods
{
  public class Water : MonoBehaviour
  {
    [SerializeField] private int hydration;
    public int Hydration => hydration;
  }
}