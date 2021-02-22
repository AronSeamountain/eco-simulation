using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
  public class Bar : MonoBehaviour
  {
    public Slider slider;
    public Text text;
    public float Value { get; set; }

    public Color Color
    {
      get => Color;
      set => slider.targetGraphic.color = value;
    }

    public Color BackgroundColor { get; set; }

    public void InitializeBar(float value, float maxValue)
    {
      slider = GetComponent<Slider>();
      text = slider.GetComponentInChildren<Text>();
      Value = value;
      slider.value = Value / maxValue;
      text.text = Value + "/" + maxValue;
    }

    public void OnValueChanged(float value, float maxValue)
    {
      if (!this) return;
      Value = value;
      slider.value = Value / maxValue;
      text.text = Value + "/" + maxValue;
    }
  }
}