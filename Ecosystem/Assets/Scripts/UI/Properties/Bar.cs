using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Properties
{
  public sealed class Bar : AbstractProperty
  {
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    public void Configure(float value, float maxValue, Color backgroundColor)
    {
      slider.targetGraphic.color = backgroundColor;
      SetProgress(value, maxValue);
    }

    public void OnValueChanged(float value, float maxValue)
    {
      SetProgress(value, maxValue);
    }

    private void SetProgress(float value, float maxValue)
    {
      slider.value = value / maxValue;
      text.text = Prettifier.Round(value, 2) + "/" + Prettifier.Round(maxValue, 2);
    }
  }
}