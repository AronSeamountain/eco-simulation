using System.ComponentModel;
using UnityEngine;

namespace Foods
{
  public class Water : MonoBehaviour
  {
    [DefaultValue(1)] [SerializeField] private int saturationModifier;
    public int SaturationModifier => saturationModifier;

    private void Awake()
    {
      Hide();
    }

    private void Hide()
    {
      GetComponent<MeshRenderer>().enabled = false;
    }
  }
}