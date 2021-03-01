using System.ComponentModel;
using UnityEngine;

namespace Foods
{
  public class Water : MonoBehaviour
  {
    [DefaultValue(1)] [SerializeField] private int saturationModifier;
    public int SaturationModifier => saturationModifier;
  }
}