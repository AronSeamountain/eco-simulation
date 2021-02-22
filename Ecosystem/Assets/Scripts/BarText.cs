using UnityEngine;
using UnityEngine.UI;

public class BarText : MonoBehaviour
{
  [SerializeField] private string valueText = "{0}";

  private Text _text;

  // Start is called before the first frame update
  private void Start()
  {
    _text = GetComponent<Text>();

    GetComponentInParent<Slider>().onValueChanged.AddListener(ValueChanged);
  }

  private void ValueChanged(float value)
  {
    _text.text = string.Format(valueText + "/" + GetComponentInParent<Slider>().maxValue * 100, value * 100);
  }
}