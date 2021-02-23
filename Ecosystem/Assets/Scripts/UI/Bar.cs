using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public sealed class Bar : MonoBehaviour
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
      text.text = value + "/" + maxValue;
    }
  }
}