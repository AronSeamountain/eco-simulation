using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Properties
{
  public sealed class KeyValuePairText : AbstractProperty
  {
    [SerializeField] private TextMeshProUGUI text;
    private string _key;

    public void Configure(string key, string value)
    {
      _key = key;
      UpdateText(_key, value);
    }

    public void OnValueChanged(string value)
    {
      UpdateText(_key, value);
    }

    private void UpdateText(string key, string value)
    {
      var str = $@"<b><align=left>{key}<line-height=0></b>
<align=right>{value}
      ";

      text.text = str;
    }
  }
}