using UnityEngine;

namespace UI.Properties
{
  public sealed class PropertyFactory : MonoBehaviour
  {
    public static PropertyFactory SharedInstance;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject KeyValuePairTextPrefab;

    private void Awake()
    {
      SharedInstance = this;
    }

    public Bar CreateSlider()
    {
      return Instantiate(barPrefab).GetComponent<Bar>();
    }

    public KeyValuePairText CreateKeyValuePair()
    {
      return Instantiate(KeyValuePairTextPrefab).GetComponent<KeyValuePairText>();
    }
  }
}