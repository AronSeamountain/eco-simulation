using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
  public class Bar : MonoBehaviour
  {
    public float Value { get; set; }

    public Color Color
    {
      get => Color;
      set => GetComponent<Slider>().targetGraphic.color = value;
    }

    public Color BackgroundColor { get; set; }
    
    public void OnValueChanged(int value, int maxValue)
    {
      if (this)
      {
        Value = value;
        var slider = GetComponent<Slider>();
        slider.value = value/ (float) maxValue;
        var text = slider.GetComponentInChildren<Text>();
        text.text = value + "/" + maxValue;
      }
    }
  }
}