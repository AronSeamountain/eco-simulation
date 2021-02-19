using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
  public class Bar : MonoBehaviour
  {
    public float Value { get; set; }
    public Color Color { get; set; }
    public Color BackgroundColor { get; set; }

    public void OnValueChanged(int health, int maxHealth)
    {
      if (this) GetComponent<Slider>().value = health/ (float) maxHealth;
    }
  }
}