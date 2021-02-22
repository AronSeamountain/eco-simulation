using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
  public class Bar : MonoBehaviour
  {
    public float Value { get; set; }

    public Slider slider;
    public Text text;

    public Color Color
    {
      get => Color;
      set => slider.targetGraphic.color = value;
    }

    public Color BackgroundColor { get; set; }

    public void InitializeBar(int value, int maxValue)
    {
      slider = GetComponent<Slider>();
      text = slider.GetComponentInChildren<Text>();
      Value = value;
      slider.value = Value / maxValue;
      text.text = Value + "/" + maxValue;
    }
    
    public void OnValueChanged(int value, int maxValue)
    {
      if (!this) return;
      Value = value;
      slider.value = Value / maxValue;
      text.text = Value + "/" + maxValue;
    }
  }
}